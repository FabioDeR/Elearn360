using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.IdentityConfig
{
    public class MyIdentityContext<TUser, TRole, TUserRole, TKey> : IdentityDbContext<TUser, TRole, TKey, IdentityUserClaim<TKey>, TUserRole,
	IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
	where TUser : IdentityUser<TKey>
		where TRole : IdentityRole<TKey>
		where TUserRole : IdentityUserRole<TKey>
		where TKey : IEquatable<TKey>

	{
		public MyIdentityContext(DbContextOptions options) : base(options)
		{
		}
	}
}
