using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NJIS.FPZWS.MqttClient
{
    public class Program
    {
        [System.STAThread]
        public static void Main(string[] args)
        {
            var form = new MainForm();
            Application.Run(form);
        }
    }
}
