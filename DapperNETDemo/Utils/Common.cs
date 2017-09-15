using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace DapperNETDemo.Utils
{
    public class Common
    {
        public enum Type
        {
            A,
            B,
            C
        }

        public static IDbConnection OpenConnection()
        {
            IDbConnection connection = new SqlConnection("Data Source=139.198.13.12,4124;Initial Catalog=DapperDemoDB;Persist Security Info=True;User ID=dapper;Password=w123456");
            connection.Open();
            return connection;
        }
    }
}
