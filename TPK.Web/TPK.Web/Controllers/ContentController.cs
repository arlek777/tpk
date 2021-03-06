﻿using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPK.Web.Data;
using TPK.Web.Infrastructure;
using TPK.Web.Models;

namespace TPK.Web.Controllers
{
    [Authorize]
    public class ContentController : Controller
    {
        private readonly TPKDbContext _context;
        private readonly IHostingEnvironment _environment;

        public ContentController(TPKDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost]
        public IActionResult Import()
        {
            //var files = Directory.GetFiles(Path.Combine(_environment.WebRootPath, @"Img\Items"));
            //var thumbPath = Path.Combine(_environment.WebRootPath, @"Thumb\Img\Items");
            //foreach (var file in files)
            //{
            //    var fileInfo = new FileInfo(file);
            //    var bitmap = ImageHelper.CreateThumbnail(file, 300, 300);
            //    bitmap.Save(Path.Combine(thumbPath, fileInfo.Name), ImageFormat.Jpeg);
            //}

            //OldTpkSiteImporter.ImportToDb(_context, _environment.WebRootPath);
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            return View(_context.Content.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Content content)
        {
            if (ModelState.IsValid)
            {
                content.ImgSrc = SaveImg(content.ImageFile);
                _context.Content.Add(content);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(content);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Content.SingleOrDefaultAsync(m => m.Id == id);
            if (content == null)
            {
                return NotFound();
            }
            return View(content);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Content content)
        {
            if (id != content.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dbContent = await _context.Content.SingleOrDefaultAsync(m => m.Id == id);
                    dbContent.Description = content.Description;
                    dbContent.ImgSrc = SaveImg(content.ImageFile);
                    dbContent.Price = content.Price;
                    dbContent.Title = content.Title;

                    await _context.SaveChangesAsync();
                }
                catch (Exception exc)
                {
                    if (!CategoryExists(content.Id))
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
            return View(content);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Content
                .SingleOrDefaultAsync(m => m.Id == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var content = await _context.Content.SingleOrDefaultAsync(m => m.Id == id);
            _context.Content.Remove(content);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Content.Any(e => e.Id == id);
        }

        private string SaveImg(IFormFile file)
        {
            var imgSrc = "/Img/Items/" + file.FileName;
            var fullImgSrc = _environment.WebRootPath + "/Img/Items/" + file.FileName;
            var stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Close();

            System.IO.File.WriteAllBytes(fullImgSrc, stream.ToArray());

            var bitmap = ImageHelper.CreateThumbnail(fullImgSrc, 300, 300);
            bitmap.Save(_environment.WebRootPath + "Thumb/Img/Items/" + file.FileName);

            return imgSrc;
        }
    }
}
