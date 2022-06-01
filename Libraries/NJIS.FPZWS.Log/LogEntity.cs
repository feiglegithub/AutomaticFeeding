using System;

namespace NJIS.FPZWS.Log
{
    public class LogEntity
    {
        public DateTime DateTime { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public string Class { get; set; }

        public string LogFileName { get; set; }
        public int Thread { get; set; }
    }
}
