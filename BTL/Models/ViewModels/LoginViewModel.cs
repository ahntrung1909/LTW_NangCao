using System.ComponentModel.DataAnnotations;

namespace BTL.Models.ViewModels
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Hãy nhập UserName")]
		public string UserName { get; set; }
	
		[DataType(DataType.Password), Required(ErrorMessage = "Hãy nhập Password")]
		public string Password { get; set; }

		public string ReturnUrl {  get; set; }

	}
}
