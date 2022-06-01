namespace NJIS.FPZWS.LineControl.Manager.Presenters
{
    public class PatternDetailPresenter: SearchPresenterBase
    {

        public PatternDetailPresenter()
        {
            Register<string>(GetData, (sender, batchName) =>
            {
                ExecuteBase(this,sender,batchName,(o,arg)=> Contract.GetPatternDetailsByBatchName(batchName),data=>Send(BindingData,sender,data));
            });
        }
    }
}
