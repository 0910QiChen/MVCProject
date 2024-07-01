using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVCProject.Models
{
    public class User
    {
        [Display(Name = "UserID:")]
        public int userid { get; set; }

        [Display(Name = "UserName:")]
        [Required(ErrorMessage = "UserName is required!")]
        [MinLength(6, ErrorMessage = "Username must be at least 6 characters!")]
        public string username { get; set; }

        [Display(Name = "Email:")]
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Invalid email format!")]
        public string email { get; set; }

        [Display(Name = "Password:")]
        [Required(ErrorMessage = "Password is required!")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters!")]
        public string password { get; set; }

        [Display(Name = "Confirm Password:")]
        [Required(ErrorMessage = "Confirm Password is required!")]
        [Compare("password", ErrorMessage = "Passwords do not match!")]
        public string confirmedPassword { get; set; }

        [Display(Name = "Your Profile Pic")]
        public HttpPostedFileBase profilePic { get; set; }
        
        public string profilePicPath { get; set; }
    }
}