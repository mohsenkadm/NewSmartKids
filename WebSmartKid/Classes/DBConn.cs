namespace WebSmartKid.Classes
{
    public enum ConnectionType
    {
        SqlServerLocal,
        SqlServerSmarter
    }

    public static class DBConn
    {
        public static readonly ConnectionType ConnectionType = ConnectionType.SqlServerSmarter;

        public static string ConnectionString
        {
            get
            {
                return ConnectionType switch
                {
                    ConnectionType.SqlServerLocal => "Server=.; Database=WebSmartKidDb; Trusted_Connection=True; " +
                    " MultipleActiveResultSets=True;",
                    ConnectionType.SqlServerSmarter => "Data Source=SQL8001.site4now.net;Initial Catalog=db_a8aa08_dbkids;" +
                    "User Id=db_a8aa08_dbkids_admin;Password=50nVKuo5wB2G; MultipleActiveResultSets=True;",
                    _ => "",
                };
            }
        }
    }
}
