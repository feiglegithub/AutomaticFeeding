using NJIS.Ini;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
   public class DbSettings:SettingBase<DbSettings>
    {
        //10.30.40.230 10.10.14.51
        public static string ConnectionString2 = "Data Source=10.30.40.230;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";
        public static string MesDataConnectionString2 = "Data Source=10.10.14.51;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=MesDataCenter;";
        public static string PlusConnectionString2 = "Data Source=10.30.40.230;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.CuttingPlus;";
        public static string HuangGangBWMSConnectionString = "Data Source=10.30.40.232;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=HuangGangBWMS;";
        //[Property("Ip")]
        //public string Ip { get; set; }
        //[Property("User")]
        //public string User { get; set; }
        //[Property("Password")]
        //public string Password { get; set; }
        //[Property("DataBase")]
        //public string DataBase { get; set; }
        [Property("ConnectionString")]
        public string ConnectionString { get; set; } //=>
                                                     //$@"Data Source={Ip};User Id={User};Password={Password};Initial Catalog={DataBase};";

        [Property("MesDataConnectionString")]
        public string MesDataConnectionString { get; set; }

        [Property("PlusConnectionString")]
        public string PlusConnectionString { get; set; }
    }
}
