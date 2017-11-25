using MarketMatch.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ShookOnline.Models
{
    public class User
    {
        public readonly string  providerKey ;
        public string userName { get; set; }
        public string password { get; set; }
        public string email { get; set; }

        /*copy constructor for social login*/
        public User(dynamic user)
        {
            providerKey = user.id;
            email = user.email;
            userName = user.first_name + " " + user.last_name;
            password = null;
        }

        /*copy constructor*/
        public User(User user)
        {
            email = user.email;
            userName = user.userName;
            password = user.password;
            providerKey = null;
        }

        public async Task checkSocialLogin()
        {
            SqlManager manager = await SqlManager.getSqlManagerInstance();
            SqlDataReader dr = manager.DataReader("select id from users where providerkey='" + providerKey + "'");
            if (dr != null) { 
                if (dr.HasRows)
                    return;
                dr.Close();
            }
            await userRegister();
        }

        private async Task userRegister()
        {
            SqlManager manager = await SqlManager.getSqlManagerInstance();
            manager.ExecuteQueries("insert into users values('" + providerKey + "','" + userName + "','" + null + "','" + email + "')");
        }
    }
}