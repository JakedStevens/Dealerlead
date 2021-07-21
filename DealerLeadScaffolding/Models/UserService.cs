using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DealerLead.Web.Models
{
	public class UserService
	{
		private readonly DealerLeadDbContext _context;

		public UserService(DealerLeadDbContext context)
		{
			_context = context;
		}

		public DealerLeadUser GetDealerLeadUser(ClaimsPrincipal user)
		{
			var userOid = Guid.Parse(user.Claims.ToList().FirstOrDefault(claim =>
				claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier"
			).Value);

			return _context.DealerLeadUser.FirstOrDefault(x => x.AzureADId == userOid);
		}
	}
}
