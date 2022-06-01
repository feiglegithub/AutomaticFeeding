using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCS.Mod;
using WCS.OPC;

namespace WCS
{
    public partial class MachineHandTestForm : Form
    {
        public MachineHandTestForm()
        {
            InitializeComponent();
            this.Shown += MachineHandTestForm_Shown;
        }

        private void MachineHandTestForm_Shown(object sender, EventArgs e)
        {
            if (!OPCExecute.IsConn)
            {
                OPCExecute.OPCServerAdd();
            }
        }

        private List<TestClass> FinishedList { get; set; } = new List<TestClass>();

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = false;
            var tuple = GetTask(cmbFrom1, cmbTo1, txtCount1);
            if (tuple == null)
            {
                button4.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = true;
                return;
            }
             testList = CreatedList(tuple.Item1, tuple.Item2, tuple.Item3);

            tuple = GetTask(cmbFrom2, cmbTo2, txtCount2);
            if (tuple == null)
            {
                button4.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = true;
                return;
            }
            testList = testList.Concat( CreatedList(tuple.Item1, tuple.Item2, tuple.Item3)).ToList();

            tuple = GetTask(cmbFrom3, cmbTo3, txtCount3);
            if (tuple == null)
            {
                button4.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = true;
                return;
            }
            testList = testList.Concat(CreatedList(tuple.Item1, tuple.Item2, tuple.Item3)).ToList();

            //Run();
            BeginRun();
            btnStop.Enabled = true;
            //button4.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button4.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = false;
            var tuple = GetTask(cmbFrom1, cmbTo1, txtCount1);
            if (tuple == null)
            {
                button4.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = true;
                return;
            }

            testList = CreatedList(tuple.Item1, tuple.Item2, tuple.Item3);
            BeginRun();
            //Run();
            //button4.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = true;
            //btnStop.Enabled = false;

        }


        private void button2_Click(object sender, EventArgs e)
        {
            button4.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = false;
            var tuple = GetTask(cmbFrom2, cmbTo2, txtCount2);
            if (tuple == null)
            {
                button4.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = true;
                return;
            }
            testList = CreatedList(tuple.Item1, tuple.Item2, tuple.Item3);
            //Run();
            BeginRun();
            btnStop.Enabled = true;
            //button4.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button4.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = false;
            var tuple = GetTask(cmbFrom3, cmbTo3, txtCount3);
            if (tuple == null)
            {
                button4.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = true;
                return;
            }
            testList = CreatedList(tuple.Item1, tuple.Item2, tuple.Item3);
            //Run();
            BeginRun();
            btnStop.Enabled = true;
            //button4.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = false;
        }

        Tuple<int,int,int> GetTask(ComboBox cmbFrom,ComboBox cmbTo,TextBox txtCount)
        {
            int count = 0;
            if (!int.TryParse(txtCount.Text.ToString(), out count))
            {
                MessageBox.Show(this, "请输入正确数量");
                return null;
            }

            int from = 0;
            if (!int.TryParse(cmbFrom.SelectedItem.ToString(), out from))
            {
                MessageBox.Show(this, "请输入正确起始工位");
                return null;
            }

            int to = 0;
            if (!int.TryParse(cmbTo.SelectedItem.ToString(), out to))
            {
                MessageBox.Show(this, "请输入正确目标工位");
                return null;
            }
            return new Tuple<int, int, int>(from, to, count);
        }

        private Thread th = null;

        List<TestClass> testList { get; set; } = new List<TestClass>();

        private void  BeginRun()
        {
            th = new Thread(Run) { IsBackground = true };
            th.Start(testList);
        }

        private void Run(object arg)
        {
            List<TestClass> testList = (List<TestClass>)arg;
            while (testList.FindAll(item=>!item.IsFinished).Count>0)
            {
                if (!OpcHsc.RMCanDo(1))
                {
                    this.Invoke((Action)(() => textBox1.Text = "机械手正忙"));
                    //textBox1.Text = "机械手正忙";
                    System.Threading.Thread.Sleep(20);
                    continue;
                }

                if (OpcHsc.RStop())
                {
                    OpcHsc.RClearFeed();
                    OpcHsc.ClearMainpulatorTask();

                    var test = testList.FirstOrDefault(item => item.IsStart);
                    if(test!=null)
                    {
                        test.IsFinished = true;
                        test.IsStart = false;
                        test.FinishedTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff");
                        this.Invoke((Action)(() =>
                        {
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = FinishedList;
                        }));
                        //FinishedList.Add(test);
                       
                        System.Threading.Thread.Sleep(20);
                    }
                    
                }
                this.Invoke((Action)(()=> textBox1.Text = "机械手空闲"));
                OpcHsc.RClearFeed();
                OpcHsc.ClearMainpulatorTask();

                var test1 = testList.FirstOrDefault(item =>! item.IsFinished);
                if (test1 == null) break;
                if (OpcHsc.TestWriteToMainpulator(test1.From, test1.To))
                {
                    test1.IsStart = true;
                    test1.StarTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff");
                    this.Invoke((Action)(() =>
                    {
                        FinishedList.Add(test1);
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = FinishedList;
                    }));

                    System.Threading.Thread.Sleep(20);
                }
            }
            this.Invoke((Action)(() =>
            {
                button4.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = true;
                btnStop.Enabled = false;
            }
            ));
            
        }

        List<TestClass> CreatedList(int from ,int to,int count)
        {
            List<TestClass> testList = new List<TestClass>();
            for (int i=0;i<count;i++)
            {
                testList.Add(new TestClass() { From = from, To = to,IsFinished=false });
            }

            return testList;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(th != null && th.IsAlive)
            {
                th.Abort();

            }

            button4.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = true;
            btnStop.Enabled = false;
        }

        private void MachineHandTestForm_Load(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            OpcHsc.RClearFeed();
            OpcHsc.ClearMainpulatorTask();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            
            int station = Convert.ToInt32(comboBox1.SelectedItem.ToString());
            Sorting.GetInstance().ReturnByHand(station);
        }
    }

    public class TestClass
    {
        public int From { get; set; }
        public int To { get; set; }
        public bool IsFinished { get; set; }
        public bool IsStart { get; set; } = false;
        public string StarTime { get; set; }
        public string FinishedTime { get; set; }
    }




}
