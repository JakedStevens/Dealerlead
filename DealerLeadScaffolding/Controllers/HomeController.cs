using DealerLead.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		private string GetOid()
		{
			var user = this.User;

			var claimsList = user.Claims.ToList();
			var oidClaim = claimsList.FirstOrDefault(claim =>
				claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier"
			);
			var oidValue = oidClaim.Value;
			return oidValue;
		}

		[AllowAnonymous]
		public IActionResult Index()
		{
			return View();
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
