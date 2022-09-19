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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using PartnerMan.Data.Migrations;

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
                // A model binder nem oldotta fel a column + order property-ket, ezért a workaround.
                string orderCol = Request.Query["order[0][column]"].ToString();
                string orderDirAsc = Request.Query["order[0][dir]"].ToString();
                string orderColName = Request.Query[$"columns[{orderCol}][data]"].ToString();

                switch (orderColName)
                {
                    case "displayName":
                        orderColName = "LastName";
                        break;  
                    default:
                        break;
                }

                int total;
                List<PartnerModel> bejelentesek = new ();

                total = _context.Partners.Count();

                string search = Request.Query["search[value]"].ToString();

                var tmp_e = _context
                        .Partners
                        .Include(a =>a.Addresses)
                        .AsQueryable()
                        .Where(p=>
                            p.FirstName.Contains(search) ||
                            p.MiddleName.Contains(search) ||
                            p.LastName.Contains(search) ||
                            p.Addresses.Any(
                                a=>
                                a.PostalCode.Contains(search) ||
                                a.City.Contains(search) ||
                                a.Address.Contains(search) )
                        )
                        .OrderBy($"{orderColName} {orderDirAsc}")
                        .Skip(param.start)
                        .Take(param.length)
                        .AsNoTracking();

                //total = tmp_e.Count();


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


        // GET: PartnerModels/Create
        public IActionResult Create()
        {
            var model = new PartnerModel();
            //model.Addresses.Add(new AddressModel());
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartnerModel partnerModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(partnerModel);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception)
            {

            }
            return PartialView(partnerModel);
        }

        [HttpPost]
        public IActionResult AddNewAddress(PartnerModel partnerModel,int isEdit = 1)
        {
            partnerModel.Addresses.Add(new AddressModel());
            if (isEdit == 1)
            {
                return PartialView("Edit",partnerModel);
            }
            else
            {
                return PartialView("Create",partnerModel);
            }
        }

        // GET: PartnerModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerModel = await _context
                .Partners
                .Include(m => m.Addresses)
                .SingleAsync(p => p.Id == id);

            if (partnerModel == null)
            {
                return NotFound();
            }
            return PartialView(partnerModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PartnerModel partnerModel)
        {
            if (id != partnerModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var partner = _context.Partners
                        .Include(p => p.Addresses)
                        .Single(p => p.Id == id);

                    partner.Title = partnerModel.Title;
                    partner.Comment = partnerModel.Comment;
                    partner.LastName = partnerModel.LastName;
                    partner.FirstName = partnerModel.FirstName;
                    partner.MiddleName = partnerModel.MiddleName;

                    foreach (AddressModel address in partner.Addresses)
                    {
                        _context.Entry(address).State = EntityState.Deleted;
                    }
                    partner.Addresses.AddRange(partnerModel.Addresses);
                    await _context.SaveChangesAsync();
                    return Ok();
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
                catch (Exception ex)
                {

                }
            }
            return PartialView(partnerModel);
        }

        // GET: PartnerModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerModel = await _context.Partners
                .Include(p => p.Addresses)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partnerModel == null)
            {
                return NotFound();
            }

            return PartialView(partnerModel);
        }

        // POST: PartnerModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var partnerModel = await _context
                    .Partners
                    .Include(p=>p.Addresses)
                    .SingleAsync(p => p.Id == id);
                _context.Partners.Remove(partnerModel);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return new ContentResult() { Content = "Hiba történt az elem törlése közben!" };
            }
        }

        private bool PartnerModelExists(int id)
        {
            return _context.Partners.Any(e => e.Id == id);
        }
    }
}
