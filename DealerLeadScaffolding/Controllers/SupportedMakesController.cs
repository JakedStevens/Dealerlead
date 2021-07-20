using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DealerLead;
using Microsoft.AspNetCore.Authorization;

namespace DealerLead.Web.Controllers
{
    [Authorize]
    public class SupportedMakesController : Controller
    {
        private readonly DealerLeadDbContext _context;

        public SupportedMakesController(DealerLeadDbContext context)
        {
            _context = context;
        }

        // GET: SupportedMakes
        public async Task<IActionResult> Index()
        {
            return View(await _context.SupportedMake.ToListAsync());
        }

        // GET: SupportedMakes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
			if (id == null)
			{
				return NotFound();
			}

			var supportedMake = await _context.SupportedMake
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supportedMake == null)
            {
                return NotFound();
            }

            return View(supportedMake);
        }

        // GET: SupportedMakes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SupportedMakes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] SupportedMake supportedMake)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supportedMake);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supportedMake);
        }

        // GET: SupportedMakes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportedMake = await _context.SupportedMake.FindAsync(id);
            if (supportedMake == null)
            {
                return NotFound();
            }
            return View(supportedMake);
        }

        // POST: SupportedMakes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] SupportedMake supportedMake)
        {
            if (id != supportedMake.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    supportedMake.ModifyDate = DateTime.Now;
                    _context.Update(supportedMake);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupportedMakeExists(supportedMake.Id))
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
            return View(supportedMake);
        }

        // GET: SupportedMakes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportedMake = await _context.SupportedMake
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supportedMake == null)
            {
                return NotFound();
            }

            return View(supportedMake);
        }

        // POST: SupportedMakes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supportedMake = await _context.SupportedMake.FindAsync(id);
            _context.SupportedMake.Remove(supportedMake);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupportedMakeExists(int id)
        {
            return _context.SupportedMake.Any(e => e.Id == id);
        }
    }
}
