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

        /// <summary>
        /// Returns content depends on the situation.
        /// 1. If id is not present - return root categories.
        /// 2. If item with id is found - return this item (items page).
        /// 3. If sub categories are found - return them (category page).
        /// 4. If items are found - return them (items page).
        /// </summary>
        [HttpGet]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (!id.HasValue)
            {
                var rootCategories = _context.Content.Where(c => !c.CategoryId.HasValue && c.ContentType == ContentType.Category).ToList();
                return Ok(rootCategories);
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