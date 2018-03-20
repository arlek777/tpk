using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPK.Web.Data;
using TPK.Web.Models;

namespace TPK.Web.Controllers
{
    [Route("api/сategory")]
    public class CategoryApiController : Controller
    {
        private readonly TPKDbContext _context;

        public CategoryApiController(TPKDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Category> GetCategories()
        {
            return _context.Category;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var category = await _context.Category.SingleOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryContent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.Category.SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            var subCategories = _context.Category.Where(c => c.ParentCategoryId == id).ToList();
            if (subCategories.Any())
            {
                return Ok(subCategories);
            }

            var items = _context.Item.Where(i => i.CategoryId == id);
            return Ok(items);
        }
    }
}