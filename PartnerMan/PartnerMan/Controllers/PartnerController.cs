using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PartnerMan.Data;
using PartnerMan.Models;
using PartnerMan.PartnerMan.DataTables;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;

namespace PartnerMan.Controllers
{
    [Authorize]
    public class PartnerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartnerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PartnerModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Partners.ToListAsync());
        }

        public  IActionResult GetPartnerRows(JQDTParams param)
        {
            try
            {
                string orderCol = Request.Query["order[0][column]"].ToString();
                string orderDirAsc = Request.Query["order[0][dir]"].ToString();
                string orderColName = Request.Query[$"columns[{orderCol}][data]"].ToString();


                int total;
                List<PartnerModel> bejelentesek = new ();

                total = _context.Partners.Count();

                var tmp_e = _context
                        .Partners
                        .AsQueryable().OrderBy($"{orderColName} {orderDirAsc}")
                        .Skip(param.start)
                        .Take(param.length)
                        .AsNoTracking();

                

                return Json(new
                {
                    data = tmp_e.ToArray(),
                    recordsTotal = total,
                    recordsFiltered = total,
                    draw = param.draw
                });
            }
            catch (Exception ex)
            {
                return Json(null);
            }
        }

        // GET: PartnerModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerModel = await _context.Partners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partnerModel == null)
            {
                return NotFound();
            }

            return View(partnerModel);
        }

        // GET: PartnerModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PartnerModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] PartnerModel partnerModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(partnerModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(partnerModel);
        }

        // GET: PartnerModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerModel = await _context.Partners.FindAsync(id);
            if (partnerModel == null)
            {
                return NotFound();
            }
            return View(partnerModel);
        }

        // POST: PartnerModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] PartnerModel partnerModel)
        {
            if (id != partnerModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partnerModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartnerModelExists(partnerModel.Id))
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
            return View(partnerModel);
        }

        // GET: PartnerModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerModel = await _context.Partners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partnerModel == null)
            {
                return NotFound();
            }

            return View(partnerModel);
        }

        // POST: PartnerModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partnerModel = await _context.Partners.FindAsync(id);
            _context.Partners.Remove(partnerModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartnerModelExists(int id)
        {
            return _context.Partners.Any(e => e.Id == id);
        }
    }
}
