using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TPK.Web.Data;
using TPK.Web.Models;

namespace TPK.Web.Controllers
{
    [Route("api/content")]
    public class ContentApiController : Controller
    {
        private readonly TPKDbContext _context;

        public ContentApiController(TPKDbContext context)
        {
            _context = context;
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult GetRootCategories()
        {
            var result = _context.Content.Where(c => !c.CategoryId.HasValue && c.ContentType == ContentType.Category).ToList();
            return Ok(result);
        }

        [Route("[action]/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetContent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _context.Content.FirstOrDefaultAsync(c => c.Id == id && c.ContentType == ContentType.Item);
            if(item != null)
            {
                return Ok(item);
            }

            var subCategories = _context.Content.Where(c => c.CategoryId == id && c.ContentType == ContentType.Category).ToList();
            if (subCategories.Any())
            {
                return Ok(new { data = subCategories, type = ContentType.Category });
            }

            var items = _context.Content.Where(c => c.CategoryId == id && c.ContentType == ContentType.Item);
            return Ok(new { data = items, type = ContentType.Item });
        }
    }
}