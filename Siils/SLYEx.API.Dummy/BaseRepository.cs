using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SLYEx.API.Dummy
{
    public abstract class BaseRepository
    {
        protected IDbConnection OpenConnection()
        {
            var connection = new SqlConnection(GetConnectionString());
            connection.Open();
            return connection;
        }

        protected string GetConnectionString()
        {
            var connectionStringSetting = ConfigurationManager.ConnectionStrings[ConnectionStringName];

            if (connectionStringSetting == null)
            {
                throw new Exception(string.Format("ConnectionString {0} does not exist", ConnectionStringName));
            }

            return connectionStringSetting.ConnectionString;
        }

        protected string ConnectionStringName
        {
            get { return "LoanBookConnectionString"; }
        }
    }
}