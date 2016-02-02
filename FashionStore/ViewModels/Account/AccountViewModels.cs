using System.ComponentModel.DataAnnotations;
using FashionStore.App_GlobalResources;

namespace FashionStore.ViewModels.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessageResourceName = "NameInValid", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^[A-Za-z]+\w{2,20}",
            ErrorMessageResourceName = "NameInValid", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "EmailInValid", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$",
            ErrorMessageResourceName = "EmailInValid", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "PhoneInValid", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^[0-9]{10,15}$",
            ErrorMessageResourceName = "PhoneInValid", ErrorMessageResourceType = typeof(Resource))]
        public string PhoneNumber { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

  

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {

        [Required(ErrorMessageResourceName = "NameInValid", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^[A-Za-z]+\w{2,20}",
           ErrorMessageResourceName = "NameInValid", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "PaswdInValid", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^(?=[a-zA-Z])(?=[a-zA-Z0-9]*)(?!.*\s).{6,20}$",
           ErrorMessageResourceName = "PaswdInValid", ErrorMessageResourceType = typeof(Resource))]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceName = "NameInValid", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^[A-Za-z]+\w{2,20}",
            ErrorMessageResourceName = "NameInValid", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "EmailInValid", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$",
            ErrorMessageResourceName = "EmailInValid", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "PhoneInValid", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^[0-9]{10,15}$",
            ErrorMessageResourceName = "PhoneInValid", ErrorMessageResourceType = typeof(Resource))]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessageResourceName = "PaswdInValid", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^(?=[a-zA-Z])(?=[a-zA-Z0-9]*)(?!.*\s).{6,20}$",
           ErrorMessageResourceName = "PaswdInValid", ErrorMessageResourceType = typeof(Resource))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "ConfirmPswdInValid", ErrorMessageResourceType = typeof(Resource))]
        [Compare("Password", ErrorMessageResourceName = "ConfirmPswdInValid", ErrorMessageResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }
    }
    public class QuickOrderViewModel
    {
        [Required(ErrorMessageResourceName = "NameInValid", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^[A-Za-z]+\w{2,20}",
            ErrorMessageResourceName = "NameInValid", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "EmailInValid", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$",
            ErrorMessageResourceName = "EmailInValid", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "PhoneInValid", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^[0-9]{10,15}$",
            ErrorMessageResourceName = "PhoneInValid", ErrorMessageResourceType = typeof(Resource))]
        public string PhoneNumber { get; set; }

     
    }
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string UserId { get; set; }
        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
