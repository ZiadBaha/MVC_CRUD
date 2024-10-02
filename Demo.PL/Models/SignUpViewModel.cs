using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
	public class SignUpViewModel
	{
		[Required(ErrorMessage ="UserName Is Required")]
        public string UserName { get; set; }

		[Required(ErrorMessage ="FName Is Required")]
        public string FName { get; set; }

		[Required(ErrorMessage = "FName Is Required")]
		public string LName { get; set; }

        [Required(ErrorMessage ="Email Is Required")]
		[EmailAddress(ErrorMessage ="Invalid Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Phone Number Is Required")]
		public string PhoneNumber { get; set; }


		[Required(ErrorMessage = "Password Is Required")]
		[MinLength(5,ErrorMessage ="Minimum length of Password is 5")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password Is Required")]
		[Compare(nameof(Password),ErrorMessage ="Confirm Password not match")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

        public bool IsAgree { get; set; }
    }
}
