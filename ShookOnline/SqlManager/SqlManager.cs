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
        private static string _connectionString;
        private static SqlConnection _conn;
        private SqlManager()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
        }

        public async static Task<SqlManager> getSqlManagerInstance()
        {
            if (_instance == null){
                _instance = new SqlManager();
                try
                {
                    _conn = new SqlConnection(_connectionString);
                    await _conn.OpenAsync();
                }
                catch (Exception) { CloseConnection(); }
            }
            return _instance;

        }



        public static void CloseConnection()
        {
            if (_conn != null && _conn.State == ConnectionState.Open)
                _conn.Close();
        }


        public void ExecuteQueries(string Query_)
        {
            if (_conn != null && _conn.State == ConnectionState.Open)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(Query_, _conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception) { }
                
            }
        }

        public SqlDataReader DataReader(string Query_)
        {
            if (_conn != null && _conn.State == ConnectionState.Open)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(Query_, _conn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    return dr;
                }
                catch (Exception) { }
            }
            return null;
        }

        ~SqlManager()
        {
            if (_conn != null && _conn.State == ConnectionState.Open)
                _conn.Close();
        }

    }
}