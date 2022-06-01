using WCS.DataBase;

namespace WCS.model
{
    public class InWareStaion
    {
        public string StationName { get; set; }
        public int StationNo { get; set; }
        public int CpuNo { get; set; }
        public int ItemNo { get; set; }
        public int HItemNo { get; set; }
        public int DeviceNo { get; set; }
        public string LedIP { get; set; }
        public int ErrorCode { get; set; }

        public InWareStaion(string name, int no, int dno, int hitem, string ip, int errorcode = 0)
        {
            this.StationName = name;
            this.StationNo = no;
            this.CpuNo = dno / 1000 - 1;
            this.ItemNo = WcsSqlB.GetItemNoByDeviceNo(dno);
            this.HItemNo = hitem;
            this.DeviceNo = dno;
            this.LedIP = ip;
            this.ErrorCode = errorcode;
        }

        //读取目标值
        public int ReadTarget()
        {
            return int.Parse(OPCExecute.AsyncRead(CpuNo, ItemNo + 1).ToString());
        }

        //读取托盘高度
        public int ReadHeight()
        {
            return int.Parse(OPCExecute.AsyncRead(CpuNo, HItemNo).ToString());
        }

        //读取外型检测信息
        public int ReadErrorCode()
        {
            return int.Parse(OPCExecute.AsyncRead(CpuNo, ItemNo + 3).ToString());
        }

        //给设备写目标值
        public void WriteDataToPLC(int workid, string pallet, int target)
        {
            OPCExecute.AsyncWrite(CpuNo, ItemNo, workid);
            OPCExecute.AsyncWrite(CpuNo, ItemNo + 2, pallet);
            OPCExecute.AsyncWrite(CpuNo, ItemNo + 1, target);
        }

        //写退回
        public void WriteBack()
        {
            OPCExecute.AsyncWrite(CpuNo, ItemNo + 1, 1);
        }

        //读取托盘号RFID
        public void ReadRFID()
        {

        }
    }
}
