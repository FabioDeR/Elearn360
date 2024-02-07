using Microsoft.AspNetCore.Identity;

namespace eLearn360.API.Config.ConfigIdentity
{
    public class CustomIdentityOptions : IdentityOptions
    {
		#region Configuration IdentityOptions == Password Required, Sign In, etc
		public void Configure(IdentityOptions options)
		{
			options.Password.RequiredLength = 8;
			options.Password.RequireDigit = true;
			options.Password.RequireLowercase = true;
			options.Password.RequireUppercase = true;
			options.Password.RequireNonAlphanumeric = true;
			options.User.RequireUniqueEmail = true;
			options.SignIn.RequireConfirmedEmail = false;
			options.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
			options.User.AllowedUserNameCharacters =
			"abcdefghijklmnopqrstuvwxyzçABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
		}
		#endregion
	}
}
