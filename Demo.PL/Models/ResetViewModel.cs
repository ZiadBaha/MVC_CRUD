using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
	public class ResetViewModel
	{
		[Required(ErrorMessage = "Password Is Required")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }
		[Required(ErrorMessage = "Confirm Password Is Required")]
		[Compare(nameof(NewPassword), ErrorMessage = "Confirm Password not match")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
		

	}
}
