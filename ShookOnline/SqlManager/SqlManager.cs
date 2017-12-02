using ShookOnline.Models;
using System;
using System.Data;
using System.Data.SqlClient;


namespace MarketMatch.Models
{
    public class SqlManager : IDisposable
    {
        private static SqlManager _instance;
        private string _connectionString;
        private SqlConnection _conn;
        private SqlManager()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;           
        }

        public  static SqlManager getSqlManagerInstance()
        {
            if (_instance == null){
                _instance = new SqlManager();
               
            }
            return _instance;

        }
        
       public void openConnection()
       {
            if (_conn != null && _conn.State == ConnectionState.Closed)
            {
                _conn = new SqlConnection(_connectionString);
                _conn.Open();
            }
       }
 
       public void closeConnection()
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
                    Collection<T> collection = new ObjectMapping().MapAll(dr);
                    return dr;
                }
                catch (Exception) { }
            }
            return null;
        }

        

        public void Dispose()
        {
            closeConnection();
        }
    }
}