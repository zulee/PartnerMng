using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PartnerMan.Data;
using PartnerMan.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Import(IFormFile file)
        {
            try
            {
                if (file.Length > 100000 )
                {
                    throw new ArgumentOutOfRangeException("File", "A fájl mérete túl nagy!");
                }
                string content = string.Empty;
                using (var sr = new StreamReader(file.OpenReadStream()))
                {
                    content = sr.ReadToEnd();
                }
                
                List<PartnerModel> partnerList =
                    JsonConvert.DeserializeObject<List<PartnerModel>>(content);

                _context.Partners.AddRange(partnerList);
                _context.SaveChanges();
            }
            catch (Exception)
            {

            }
            return View(file);
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
        }
    }
}
