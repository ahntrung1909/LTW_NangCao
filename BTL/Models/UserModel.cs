﻿using System.ComponentModel.DataAnnotations;

namespace BTL.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage ="Hãy nhập UserName")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "Hãy nhập Email"),EmailAddress]
		public string Email { get; set; }
		[DataType(DataType.Password),Required(ErrorMessage = "Hãy nhập Password")]
		public string Password { get; set; }


	}
}
