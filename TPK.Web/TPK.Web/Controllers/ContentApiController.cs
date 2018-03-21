using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public IEnumerable<Content> GetRootCategories()
        {
            return _context.Content.Where(c => !c.CategoryId.HasValue && c.ContentType == ContentType.Category).ToList();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.Content.SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            var subCategories = _context.Content.Where(c => c.CategoryId == id && c.ContentType == ContentType.Category).ToList();
            if (subCategories.Any())
            {
                return Ok(subCategories);
            }

            var items = _context.Content.Where(c => c.CategoryId == id && c.ContentType == ContentType.Item);
            return Ok(items);
        }
    }
}