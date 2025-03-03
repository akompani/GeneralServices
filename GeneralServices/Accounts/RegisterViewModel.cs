using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace GeneralServices.Accounts
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "ایمیل")]
        [DataType(DataType.EmailAddress)]
        [Remote("IsEmailExist", "Account", HttpMethod = "POST", AdditionalFields = "__RequestVerificationToken")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "شماره موبایل")]
        [DataType(DataType.PhoneNumber)]
        [Remote("IsPhoneExist", "Account", HttpMethod = "POST", AdditionalFields = "__RequestVerificationToken")]
        public string PhoneNumber { get; set; }


        [Required]
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        
        [Required]
        [Display(Name = "رمز عبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "تکرار رمز عبور")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}
