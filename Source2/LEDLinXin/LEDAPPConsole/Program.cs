using CommomLY1._0;
using LXLedControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LEDAPPConsole
{
    class Program
    {
        static string connstr = "Persist Security Info =true; Password=!Q@W#E$R5t6y7u8i;User ID = sa ; Initial Catalog = HGWCSB; Data Source =.";
        static string sql1 = "select c.*,l.LIP from [dbo].[LED_Content] c left join LED_Config l on c.LPort=l.LPort and c.LType=l.LType where LStatus=1";
        static Dictionary<string, int> times = new Dictionary<string, int>();
        static int Lid = 0;
        static string ip = "";
        static  string content = "";
        static string Ltype = "";
        static void Main(string[] args)
        {
            SendLedContent();
        }

        static void SendLedContent()
        {
            Console.WriteLine("LED服务启动。。。");
            while (true)
            {
                try
                {
                    var dt = SqlBase.GetSqlDs(sql1, connstr).Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        Lid = Convert.ToInt32(dr["LID"].ToString());
                        if (dr["LIP"] == DBNull.Value)
                        {
                            SqlBase.ExecSql($"update [LED_Content] set LStatus=998,LSendDate=GETDATE() where LID={Lid}", connstr);
                            continue;
                        }

                        ip = dr["LIP"].ToString();
                        content = dr["LContent"].ToString();

                        if (!times.ContainsKey(ip))
                        {
                            times.Add(ip, 0);
                        }

                        if (!ping(ip))
                        {
                            times[ip]++;
                            Console.WriteLine($"Ping：{ip}失败！次数：{times[ip]}  {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\r\n");
                            SqlBase.ExecSql($"update [LED_Content] set LStatus=999,LSendDate=GETDATE() where LID={Lid}", connstr);
                            continue;
                        }

                        var msg = string.Empty;
                        Ltype = dr["LType"].ToString();
                        var result = LXLedCenter.sendTextToLED(ip, content, Ltype.Equals("2") ? 20 : 12, out msg);
                        if (result)
                        {
                            SqlBase.ExecSql($"update [LED_Content] set LStatus=200,LSendDate=GETDATE() where LID={Lid}", connstr);
                            Console.WriteLine($"发送LED成功，IP：{ip}，发送时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        }
                        else
                        {
                            SqlBase.ExecSql($"update [LED_Content] set LStatus=999,LSendDate=GETDATE() where LID={Lid}", connstr);
                            Console.WriteLine($"发送LED失败，IP：{ip}，异常原因：{msg}，发送时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        }
                    }


                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(2000);
                }
            }
        }

        //Ping IP地址
        public static bool ping(string ip)
        {
            Ping p = new System.Net.NetworkInformation.Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            string data = "Test Data!";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 1000;
            PingReply reply = p.Send(ip, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
                return true;
            else
                return false;
        }
    }
}
