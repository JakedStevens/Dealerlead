using DealerLead.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DealerLead.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly DealerLeadDbContext _context;

		public HomeController(ILogger<HomeController> logger, DealerLeadDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		private Guid GetOid()
		{
			var user = this.User;

			var claimsList = user.Claims.ToList();
			var oidClaim = claimsList.FirstOrDefault(claim =>
				claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier"
			);
			if (oidClaim != null)
			{
				var oidValue = oidClaim.Value;
				return Guid.Parse(oidValue);
			}
			else
			{
				return Guid.Empty;
			}

		}

		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
			Guid oid = GetOid();
			List<DealerLeadUser> userList = await _context.DealerLeadUser.ToListAsync();
			bool userExists = userList.Any(user => user.AzureADId == oid);

			if (userExists)
			{
				return View(true);
			}
			else
			{
				return await Register(oid);
			}
		}

		public async Task<IActionResult> Register(Guid oid)
		{
			var newUser = new DealerLeadUser() { AzureADId = oid };
			_context.Add(newUser);
			await _context.SaveChangesAsync();
			return View(true);
		}

		//[Authorize]
		//public IActionResult Privacy()
		//{
		//	string oid = GetOid();
		//	return View();
		//}

		[AllowAnonymous]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
