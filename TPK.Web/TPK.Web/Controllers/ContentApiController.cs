﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TPK.Web.Data;
using TPK.Web.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace TPK.Web.Controllers
{
    [Route("api/content")]
    public class ContentApiController : Controller
    {
        private readonly TPKDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHostingEnvironment _environment;
        private readonly IConfiguration _configuration;

        private const string Currency = "грн";
        private const string RootCategoriesCacheKey = "RootCategoriesCacheKey";

        private readonly DateTimeOffset _cacheLifetime = DateTimeOffset.UtcNow.AddHours(12);

        public ContentApiController(TPKDbContext context, IMemoryCache memoryCache, IHostingEnvironment environment, IConfiguration configuration)
        {
            _context = context;
            _memoryCache = memoryCache;
            _environment = environment;
            _configuration = configuration;
        }

        /// <summary>
        /// Returns content depends on the situation.
        /// 1. If id is not present - return root categories.
        /// 2. If item with id is found - return this item and all items (items page).
        /// 3. If sub categories are found - return them (category page).
        /// 4. If items are found - return them (items page).
        /// </summary>
        [HttpGet]
        [Route("[action]/{id?}")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Get(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return GetRootCategories();
                }

                var itemResult = await GetItem(id.Value);
                if (itemResult != null)
                {
                    return itemResult;
                }

                return GetItemsOrSubCategories(id.Value);
            }

            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SendOrder([FromBody]SendOrderModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var items = _context.Content.Where(c => model.ItemIds.Contains(c.Id)).ToList();
            var managerEmails = _configuration.Get<string[]>();

            // TODO send order email

            return Ok();
        }

        private IActionResult GetRootCategories()
        {
            List<Content> rootCategories;
            if (!_memoryCache.TryGetValue(RootCategoriesCacheKey, out rootCategories))
            {
                rootCategories = _context.Content
                .Where(c => !c.CategoryId.HasValue && c.ContentType == ContentType.Category)
                .ToList();

                rootCategories = rootCategories.Select(AddPriceRangeToTitle).ToList();
                _memoryCache.Set(RootCategoriesCacheKey, rootCategories, _cacheLifetime);
            }

            return Ok(rootCategories);
        }

        private async Task<IActionResult> GetItem(int id)
        {
            object cacheEntry;
            if(!_memoryCache.TryGetValue(id, out cacheEntry))
            {
                var item = await _context.Content.FirstOrDefaultAsync(c => c.Id == id
                 && c.ContentType == ContentType.Item);
                if (item == null) return null;

                var items = _context.Content.Where(c => c.CategoryId == item.CategoryId
                   && c.ContentType == ContentType.Item);

               cacheEntry = new { item, data = items, contentTypeResult = ContentTypeResult.ItemDetails };

                if (!_environment.IsDevelopment())
                {
                    _memoryCache.Set(id, cacheEntry, _cacheLifetime);
                }
            }

            return Ok(cacheEntry);
        }

        private IActionResult GetItemsOrSubCategories(int id)
        {
            object cacheEntry;
            if (!_memoryCache.TryGetValue(id, out cacheEntry))
            {
                var items = _context.Content.Where(c => c.CategoryId == id && c.ContentType == ContentType.Item).ToList();
                if (items.Any())
                {
                    cacheEntry = new { data = items, contentTypeResult = ContentTypeResult.Items };
                }
                else
                {
                    var subCategories = _context.Content
                        .Where(c => c.CategoryId == id && c.ContentType == ContentType.Category)
                        .ToList();

                    subCategories = subCategories.Select(AddItemCountToTitle).ToList();
                    cacheEntry = new { data = subCategories, contentTypeResult = ContentTypeResult.Categories };

                    if (!_environment.IsDevelopment())
                    {
                        _memoryCache.Set(id, cacheEntry, _cacheLifetime);
                    }
                }
            }

            return Ok(cacheEntry);
        }

        private Content AddPriceRangeToTitle(Content category)
        {
            var items = GetCategoryItems(category.Id);
            var itemsWithPrice = items.Where(i => !String.IsNullOrEmpty(i.Price));
            if (itemsWithPrice.Any())
            {
                var minPrice = itemsWithPrice.Min(i => int.Parse(i.Price));
                var maxPrice = itemsWithPrice.Max(i => int.Parse(i.Price));

                if(minPrice != maxPrice)
                {
                    category.Title = $"{category.Title} (от {minPrice} до {maxPrice} {Currency})";
                }
            }

            return category;
        }

        private Content AddItemCountToTitle(Content category)
        {
            var items = GetCategoryItems(category.Id);
            var count = items.Count();
            if (count != 0)
            {
                category.Title = $"{category.Title} ({count} наименований)";
            }

            return category;
        }

        private IEnumerable<Content> GetCategoryItems(int id)
        {
            var items = _context.Content.Where(c => c.CategoryId == id).ToList();
            if (!items.Any()) return Enumerable.Empty<Content>();

            var item = items.FirstOrDefault();
            while (item?.ContentType != ContentType.Item)
            {
                items = _context.Content.Where(c => c.CategoryId == item.Id).ToList();
                item = items.FirstOrDefault();
            }

            return items;
        }
    }
}