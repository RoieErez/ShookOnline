using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShookOnline.Models
{
    public class IUser 
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }



        /*default constructor*/
        public IUser() { }

        /*copy register user*/
        public IUser(UserRegister u)
        {
            UserName = u.UserName;
            Password = u.Password;
            Email = u.Email;
           
        }

        /*copy login user*/
        public IUser(UserLogin u)
        {
            UserName = u.UserName;
            Password = u.Password;
            Email = null;

        }

        

       

       
    }

    public class EUser
    {
        [Key]
        public string ProviderKey { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        /*copy constructor for social login*/
        public EUser(dynamic user)
        {
            ProviderKey = user.id;
            Email = user.email;
            UserName = user.first_name + " " + user.last_name;

          
        }
        public EUser() { }

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