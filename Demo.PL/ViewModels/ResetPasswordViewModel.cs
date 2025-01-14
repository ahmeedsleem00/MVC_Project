﻿using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class ResetPasswordViewModel
	{

		[Required(ErrorMessage = "Password is Required")]
		[MinLength(5, ErrorMessage = "Minimum Paswword Length is 5")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Confirm Password is Required")]

		[Compare(nameof(NewPassword), ErrorMessage = "Confirmed Password does not match Password")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
		
	}
}
