using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DealerLead;
using DealerLead.Web.ViewModels;

namespace DealerLead.Web.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly DealerLeadDbContext _context;

        public VehiclesController(DealerLeadDbContext context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            List<Vehicle> vehicleList = await _context.Vehicle.ToListAsync();
            VehicleViewModel vehicleVM = new VehicleViewModel();
            var vehicleData = _context.Vehicle
                .Join(
                    _context.Dealership,
                        vehicle => vehicle.DealershipId,
                        dealership => dealership.Id,
                    (vehicle, dealership) => new
                    {
                        Id = vehicle.Id,
                        ModelId = vehicle.ModelId,
                        MSRP = vehicle.MSRP,
                        StockNumber = vehicle.StockNumber,
                        Color = vehicle.Color,
                        DealershipId = vehicle.DealershipId,
                        CreateDate = vehicle.CreateDate,
                        ModifyDate = vehicle.ModifyDate,
                        SellDate = vehicle.SellDate,
                        Dealership = dealership
                    }
                ).Join(
                    _context.SupportedModel,
                    vehicle => vehicle.Id,
                    model => model.Id,
                    (vehicle, model) => new
                    {
                        Id = vehicle.Id,
                        ModelId = vehicle.ModelId,
                        MSRP = vehicle.MSRP,
                        StockNumber = vehicle.StockNumber,
                        Color = vehicle.Color,
                        DealershipId = vehicle.DealershipId,
                        CreateDate = vehicle.CreateDate,
                        ModifyDate = vehicle.ModifyDate,
                        SellDate = vehicle.SellDate,
                        Dealership = vehicle.Dealership,
                        Model = model
                    }
                ).ToList();

            var thing2 = vehicleData;
            //vehicleVM.Vehicles = vehicleData;

            return View(vehicleData);
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
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
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MSRP,StockNumber,Color,SellDate")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MSRP,StockNumber,Color,SellDate")] Vehicle vehicle)
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
