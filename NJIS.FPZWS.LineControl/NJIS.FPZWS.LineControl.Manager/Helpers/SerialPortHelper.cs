using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using NJIS.FPZWS.LineControl.Manager.LocalServices;

namespace NJIS.FPZWS.LineControl.Manager.Helpers
{
    public class SerialPortHelper: SingletonPublic<SerialPortHelper>
    {
        private static readonly SerialPort _serialPort = new SerialPort();
        //{
        //    BaudRate = CuttingSerialPortSetting.Current.BaudRate,
        //    Parity = (Parity)Enum.Parse(typeof(Parity), CuttingSerialPortSetting.Current.Parity),
        //    StopBits = (StopBits)Enum.Parse(typeof(StopBits), CuttingSerialPortSetting.Current.StopBits),
        //DataBits = CuttingSerialPortSetting.Current.DataBits,
        //    PortName = CuttingSerialPortSetting.Current.PortName
        //};

        private string ReadString { get; set; } = "";

        private static readonly object _ObjLock = new object();


        public SerialPortHelper()
        {
            try
            {
                _serialPort.BaudRate = CuttingSerialPortSetting.Current.BaudRate;
                _serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), CuttingSerialPortSetting.Current.Parity);
                _serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), CuttingSerialPortSetting.Current.StopBits);
                _serialPort.DataBits = CuttingSerialPortSetting.Current.DataBits;
                _serialPort.PortName = CuttingSerialPortSetting.Current.PortName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public string CurSerialPortName => _serialPort.PortName;

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
            byte[] readByteBbuffer = new byte[_serialPort.BytesToRead];
            var ret = _serialPort.Read(readByteBbuffer, 0, readByteBbuffer.Length);
            if (ret > 0)
            {
                ReadString = Encoding.ASCII.GetString(readByteBbuffer, 0, readByteBbuffer.Length);
                _serialPort.DiscardInBuffer();
            }
        }

        public List<string> GetPortNames()
        {
            return SerialPort.GetPortNames().ToList();
        }

        public void Open()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
                _serialPort.DataReceived += _serialPort_DataReceived;
            }
        }

        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                _serialPort.DataReceived -= _serialPort_DataReceived;
            }
        }

        public bool UpdateSerialSettings(CuttingSerialPortSetting serialPortSetting = null)
        {
            if (serialPortSetting == null)
            {
                serialPortSetting = CuttingSerialPortSetting.Current;
            }

            if (_serialPort.IsOpen)
            {
                this.Close();
            }
            _serialPort.BaudRate = serialPortSetting.BaudRate;
            _serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), serialPortSetting.Parity) ;
            _serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), serialPortSetting.StopBits) ;
            _serialPort.DataBits = serialPortSetting.DataBits;
            _serialPort.PortName = serialPortSetting.PortName;
            return true;
        }

        public string ReadData()
        {
            Open();
            return ReadString;
        }

        public void ClearBuffer()
        {
            ReadString = "";
            if (_serialPort.IsOpen)
            {
                _serialPort.DiscardInBuffer();
            }
        }




    }
}
