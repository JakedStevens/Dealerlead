using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DealerLead;
using DealerLead.Web.Models;

namespace DealerLead.Web
{
    public class VehiclesController : Controller
    {
        private readonly DealerLeadDbContext _context;
        private readonly UserService _userService;

        public VehiclesController(DealerLeadDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var vehicles = _context.Vehicle.Include(v => v.Dealership).Include(v => v.Model);
            DealerLeadUser user = _userService.GetDealerLeadUser(this.User);
            return View(await vehicles.Where(v => v.Dealership.CreatingUserId == user.Id).ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Dealership)
                .Include(v => v.Model)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            DealerLeadUser user = _userService.GetDealerLeadUser(this.User);
            ViewData["DealershipId"] = new SelectList(_context.Dealership.Where(d => d.CreatingUserId == user.Id), "Id", "Name");
            ViewData["ModelId"] = new SelectList(_context.SupportedModel, "Id", "Name");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ModelId,MSRP,StockNumber,Color,DealershipId,SellDate")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            DealerLeadUser user = _userService.GetDealerLeadUser(this.User);
            ViewData["DealershipId"] = new SelectList(_context.Dealership.Where(d => d.CreatingUserId == user.Id), "Id", "Name", vehicle.DealershipId);
            ViewData["ModelId"] = new SelectList(_context.SupportedModel, "Id", "Name", vehicle.ModelId);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            DealerLeadUser user = _userService.GetDealerLeadUser(this.User);
            ViewData["DealershipId"] = new SelectList(_context.Dealership.Where(d => d.CreatingUserId == user.Id), "Id", "Name", vehicle.DealershipId);
			ViewData["ModelId"] = new SelectList(_context.SupportedModel, "Id", "Name", vehicle.ModelId);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModelId,MSRP,StockNumber,Color,DealershipId,SellDate")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
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
            DealerLeadUser user = _userService.GetDealerLeadUser(this.User);
            ViewData["DealershipId"] = new SelectList(_context.Dealership.Where(d => d.CreatingUserId == user.Id), "Id", "Name", vehicle.DealershipId);
            ViewData["ModelId"] = new SelectList(_context.SupportedModel, "Id", "Name", vehicle.ModelId);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Dealership)
                .Include(v => v.Model)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }
    }
}
