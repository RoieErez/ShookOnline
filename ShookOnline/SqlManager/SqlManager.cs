using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace MarketMatch.Models
{
    public class SqlManager
    {
        private static SqlManager _instance;
        private static string ConnectionString;
        private static SqlConnection conn;
        private SqlManager()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
        }

        public async static Task<SqlManager> getSqlManagerInstance()
        {
            if (_instance == null){
                _instance = new SqlManager();
                try
                {
                    conn = new SqlConnection(ConnectionString);
                    await conn.OpenAsync();
                }
                catch (Exception) { CloseConnection(); }
            }
            return _instance;

        }



        public static void CloseConnection()
        {
            if (conn != null && conn.State == ConnectionState.Open)
                conn.Close();
        }


        public void ExecuteQueries(string Query_)
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(Query_, conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception) { }
                
            }
        }

        public SqlDataReader DataReader(string Query_)
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(Query_, conn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    return dr;
                }
                catch (Exception) { }
            }
            return null;
        }

        ~SqlManager()
        {
            if (conn != null && conn.State == ConnectionState.Open)
                conn.Close();
        }

    }
}