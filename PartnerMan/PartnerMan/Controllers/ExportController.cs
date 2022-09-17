using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PartnerMan.Data;
using PartnerMan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;


namespace PartnerMan.Controllers
{
    [Authorize]
    public class ExportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExportController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Export(int? Id)
        {
            List<PartnerModel> partnerList = new List<PartnerModel>();

            if (Id.HasValue)
            {
                var patrner = _context
                    .Partners
                    .Include(p => p.Addresses)
                    .Single(p => p.Id == Id);
                partnerList.Add(patrner);
            }
            else
            {
                var patrners = _context
                    .Partners
                    .Include(p=>p.Addresses)
                    .ToList();
                partnerList.AddRange(patrners);
            }

            string output = JsonConvert.SerializeObject(partnerList);

            var b = System.Text.Encoding.UTF8.GetBytes(output);
            return File(b, "text/json", "Export.json");
            //Product deserializedProduct = JsonConvert.DeserializeObject<Product>(output);
            return View();
        }
    }
}
