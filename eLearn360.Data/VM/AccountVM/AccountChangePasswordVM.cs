using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elearn.Data.ViewModel.AccountVM
{
	public class AccountChangePasswordVM
	{
		[Required]
		public Guid UserId { get; set; }

		[Required(ErrorMessage = "Ce champs est requis")]
		[DataType(DataType.Password)]
		public string OldPassword { get; set; }

		//Regex : Password Beetween 8 et 50 character, with 1 digit, lower case, upper case

		[Required(ErrorMessage ="Ce champs est requis")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,50}$", 
			ErrorMessage = "Le mot de passe doit comporter au minimum entre 8 et 50 caractères avec au moins : 1 minuscule, 1 majuscule, un chiffre, et 1 caractère spécial")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "Les 2 mots de passes de sont pas identiques")]
		public string ConfirmPassword { get; set; }
	}
}
