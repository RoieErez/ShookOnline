using ShookOnline.Models;
using System;
using System.Collections.ObjectModel;
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
            _conn = new SqlConnection(_connectionString);
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

        public Collection<User> DataReader(string Query_)
        {
            if (_conn != null && _conn.State == ConnectionState.Open)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(Query_, _conn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    
                    Collection<User> collection = new User().MapAll(dr);

                    return collection;
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