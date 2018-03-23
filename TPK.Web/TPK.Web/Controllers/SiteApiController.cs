using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TPK.Web.Data;
using TPK.Web.Models;

namespace TPK.Web.Controllers
{
    [Route("api/site")]
    public class SiteApiController : Controller
    {
        private readonly TPKDbContext _context;

        public SiteApiController(TPKDbContext context)
        {
            _context = context;
        }

        [Route("[action]")]
        [HttpGet]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
        public Site Get()
        {
            return _context.Site.FirstOrDefault();
        }
    }
}