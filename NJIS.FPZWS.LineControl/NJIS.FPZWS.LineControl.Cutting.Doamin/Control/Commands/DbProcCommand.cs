using NJIS.FPZWS.LineControl.PLC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NJIS.FPZWS.LineControl.Cutting.Domain.Control.Commands
{
    public abstract class DbProcCommand<TI,TO>:CuttingCommandBase<TI,TO>
    where TI:EntityBase,new ()
    where TO:EntityBase,new()
    {
        public DbProcCommand() : base("DbProcCommand") { }
        public DbProcCommand(string commandCode) : base(commandCode)
        {
        }

        protected override EntityBase Execute(IPlcConnector plc)
        {
            return Output;
        }
    }
}
