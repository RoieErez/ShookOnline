using MarketMatch.Models;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Globalization;

namespace ShookOnline.Models
{
    public class User : ObjectMapping<User>
    {
        public readonly string  providerKey ;
        public string userName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string location { get; }
        /*copy constructor for social login*/
        public User(dynamic user)
        {
            providerKey = user.id;
            email = user.email;
            userName = user.first_name + " " + user.last_name;
            password = null;
            location= RegionInfo.CurrentRegion.DisplayName;
        }

        /*copy constructor*/
        public User(User user)
        {
            email = user.email;
            userName = user.userName;
            password = user.password;
            providerKey = null;
            location = RegionInfo.CurrentRegion.DisplayName;
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
            location = RegionInfo.CurrentRegion.DisplayName;
        }

        /*copy login user*/
        public User(UserLogin u)
        {
            userName = u.UserName;
            password = u.Password;
            providerKey = null;
            email = null;
            //email = getEmail();
            location = RegionInfo.CurrentRegion.DisplayName;

        }

        public bool checkLogin(bool flag)
        {
            using (SqlManager manager = SqlManager.getSqlManagerInstance())
            {
                manager.openConnection();
                Collection<User> collection = new Collection<User>();
                collection = flag ? manager.DataReader("select username,email from users where providerkey='" + providerKey + "'") : manager.DataReader("select username,email from users where email='" + email + "'and password='" + password + "'");
                
                if (collection != null && collection.Count == 1)
                    return true;
                if (flag)
                {
                    userRegister();
                    return true;
                }
                else
                    return false;
            }
        }

        public void userRegister()
        {
            using (SqlManager manager = SqlManager.getSqlManagerInstance())
            {
                manager.openConnection();
                manager.ExecuteQueries("insert into users values('" + providerKey + "','" + userName + "','" + password + "','" + email + "')");
            }
                
        }

        public override User Map(IDataRecord record)
        {
            User usr = new User();
            usr.userName = record.GetString(0);
            usr.email = record.GetString(1);
            return usr;
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
        [StringLength(12, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        [RegularExpression("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$")]
        public string Email { get; set; }

    }


    public class UserLogin
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(12, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

    }


}