using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPK.Web.Data;
using TPK.Web.Models;

namespace TPK.Web.Controllers
{
    [Authorize]
    public class SiteController : Controller
    {
        private readonly TPKDbContext _context;

        public SiteController(TPKDbContext context)
        {
            _context = context;
        }

        // GET: Site
        public IActionResult Index()
        {
            return View(_context.Site.ToList());
        }

        // GET: Site/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var site = await _context.Site.SingleOrDefaultAsync(m => m.Id == id);
            if (site == null)
            {
                return NotFound();
            }
            return View(site);
        }

        // POST: Site/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Site site)
        {
            if (id != site.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dbSite = await _context.Site.SingleOrDefaultAsync(m => m.Id == id);
                    dbSite.Address = site.Address;
                    dbSite.ContactText = site.ContactText;
                    dbSite.HomeText = site.HomeText;
                    dbSite.Name = site.Name;
                    dbSite.Name2 = site.Name2;
                    dbSite.Phone1 = site.Phone1;
                    dbSite.Phone2 = site.Phone2;
                    dbSite.Email = site.Email;

                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    if (!SiteExists(site.Id))
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
            return View(site);
        }

        private bool SiteExists(int id)
        {
            return _context.Site.Any(e => e.Id == id);
        }
    }
}
