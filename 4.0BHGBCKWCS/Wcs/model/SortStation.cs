
namespace WCS.model
{
    public class SortStation
    {
        //2001,,2002,.2004
        public int Code { get; set; }  //工位编号  2001,2002,2004
        public string ProductCode { get; set; } //花色  PA_18_903S
        public int Amount { get; set; }  //数量    30
        public bool HaveUpProtect { get; set; }  //是否有上保护板
    }
}
