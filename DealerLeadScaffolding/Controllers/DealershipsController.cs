using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealerLead.Web.Controllers
{
	public class DealershipsController : Controller
	{
		private readonly DealerLeadDbContext _context;

		public DealershipsController(DealerLeadDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _context.Dealership.ToListAsync());
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var dealership = await _context.Dealership
				.FirstOrDefaultAsync(m => m.Id == id);
			if (dealership == null)
			{
				return NotFound();
			}
			
			return View(dealership);
		}

		public IActionResult Create()
		{
			ViewData["States"] = new SelectList(_context.SupportedState, "Abbreviation", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,StreetAddress1,StreetAddress2,City,State,Zipcode,CreatingUserId")] Dealership dealership)
		{
			DealerLeadUser creatingUser = GetDealerLeadUser();
			dealership.CreatingUserId = creatingUser.Id;

			if (ModelState.IsValid)
			{
				_context.Add(dealership);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(dealership);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			ViewData["States"] = new SelectList(_context.SupportedState, "Abbreviation", "Name");
			if (id == null)
			{
				return NotFound();
			}

			var dealership = await _context.Dealership.FindAsync(id);
			if (dealership == null)
			{
				return NotFound();
			}
			return View(dealership);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StreetAddress1,StreetAddress2,City,State,Zipcode,CreatingUserId")] Dealership dealership)
		{
			DealerLeadUser creatingUser = GetDealerLeadUser();
			dealership.CreatingUserId = creatingUser.Id;

			if (id != dealership.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					dealership.ModifyDate = DateTime.Now;
					_context.Update(dealership);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!DealershipExists(dealership.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(dealership);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var dealership = await _context.Dealership
				.FirstOrDefaultAsync(m => m.Id == id);
			if (dealership == null)
			{
				return NotFound();
			}

			return View(dealership);
		}

		// POST: SupportedMakes/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var dealership = await _context.Dealership.FindAsync(id);
			_context.Dealership.Remove(dealership);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool DealershipExists(int id)
		{
			return _context.Dealership.Any(e => e.Id == id);
		}

		private DealerLeadUser GetDealerLeadUser()
		{
			var userOid = Guid.Parse(this.User.Claims.ToList().FirstOrDefault(claim =>
				claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier"
			).Value);

			var creatingUser = _context.DealerLeadUser.FirstOrDefault(x => x.AzureADId == userOid);

			return creatingUser;
		}
	}
}
