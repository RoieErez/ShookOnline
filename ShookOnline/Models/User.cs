using MarketMatch.Models;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;

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

        /*default constructor*/
        public User()
        {
            userName = password = email = providerKey = null;
        }

        /*copy register user*/
        public User(UserRegister u)
        {
            userName = u.UserName;
            password = u.Password;
            email = u.Email;
            providerKey = null;
        }

        /*copy login user*/
        public User(UserLogin u)
        {
            userName = u.UserName;
            password = u.Password;
            providerKey = null;
            email = null;
            //email = getEmail();

        }

        private string getEmail()
        {

            SqlDataReader dr = SqlManager.getSqlManagerInstance().DataReader("select email from users where email='" + email + "'and password='" + password + "'");
            
            
            return dr?.GetString(0) ;
        }

        public bool checkLogin(bool flag)
        {
            SqlManager manager =  SqlManager.getSqlManagerInstance();
            SqlDataReader dr;
            //social login
            if (flag)
                dr = manager.DataReader("select id from users where providerkey='" + providerKey + "'");
            //local login
            else
                dr = manager.DataReader("select id from users where email='" + email + "'and password='" + password +"'");
            if (dr != null)
            {
                if (dr.HasRows)
                    return true;
                dr.Close();
            }
            if (flag)
            {
                userRegister();
                return true;
            }
            else
                return false;
            
        }

        public void userRegister()
        {
            SqlManager manager =  SqlManager.getSqlManagerInstance();
            manager.ExecuteQueries("insert into users values('" + providerKey + "','" + userName + "','" + password + "','" + email + "')");
        }
    }

    public class UserRegister
    {
        [Required]
        [StringLength(50 , ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(12, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }


    public class UserLogin
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

    }


}