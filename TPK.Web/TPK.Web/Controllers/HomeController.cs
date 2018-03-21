using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPK.Web.Data;
using TPK.Web.Infrastructure;

namespace TPK.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly TPKDbContext _dbContext;
        private readonly IHostingEnvironment _environment;

        public HomeController(TPKDbContext dbContext, IHostingEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Import()
        {
            OldTpkSiteImporter.ImportToDb(_dbContext, _environment.ContentRootPath + "/Img/Items");
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
