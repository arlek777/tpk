using System;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using TPK.Web.Data;
using TPK.Web.Models;

namespace TPK.Web.Infrastructure
{
    public static class OldTpkSiteImporter
    {
        private static string BaseUrl = "http://tpk-granit.com.ua";
        private static int PhotoId = 0;

        public static void ImportToDb(TPKDbContext dbContext, string imgFolder)
        {
            var web = new HtmlWeb();
            var homePage = web.Load(BaseUrl);

            var desc = homePage.DocumentNode.Descendants();
            var categories = desc.Where(n => n.Attributes.Any(a => a.Value.Contains("pngfile jg_icon")));
            foreach (var category in categories)
            {
                var categoryA = category.ParentNode.ChildNodes.FindFirst("a");
                var categoryContent = new Content()
                {
                    ContentType = ContentType.Category,
                    Title = categoryA.InnerText.Replace("\n", "").Trim()
                };

                dbContext.Content.Add(categoryContent);
                dbContext.SaveChanges();

                var categoryPage = web.Load(BaseUrl + categoryA.Attributes["href"].Value.Replace("amp;", ""));
                var subCategories = categoryPage.DocumentNode.Descendants()
                    .Where(n => n.Attributes.Any(a => a.Value.Contains("pngfile jg_icon")));

                foreach (var subCategory in subCategories)
                {
                    var subCategoryA = subCategory.ParentNode.ChildNodes.FindFirst("a");
                    var subCategoryContent = new Content()
                    {
                        CategoryId = categoryContent.Id,
                        ContentType = ContentType.Category,
                        Title = subCategoryA.InnerText.Replace("\n", "").Trim()
                    };

                    dbContext.Content.Add(subCategoryContent);
                    dbContext.SaveChanges();

                    var subCategoryPage = web.Load(BaseUrl + subCategoryA.Attributes["href"].Value.Replace("amp;", ""));

                    var items = subCategoryPage.DocumentNode.Descendants()
                        .Where(n => n.Attributes.Any(a => a.Value.Contains("jg_catelem_photo")));
                    foreach (var item in items)
                    {
                        var itemLink = item.Attributes["href"];
                        var itemPage = web.Load(BaseUrl + itemLink.Value.Replace("amp;", ""));

                        var photo = itemPage.DocumentNode.Descendants().FirstOrDefault(n => n.Id == "jg_photo_big");
                        var path = $"{imgFolder}/Id{++PhotoId}.jpeg";

                        var titlePrice = itemPage.DocumentNode.Descendants().FirstOrDefault(
                            n => n.GetAttributeValue("style", "") == "text-align:center; color:#3A5163;").InnerText;

                        var description = itemPage.DocumentNode.Descendants()
                            .FirstOrDefault(n => n.Id == "jg_photo_description").InnerText.Trim();

                        new WebClient().DownloadFile(BaseUrl + photo.Attributes["src"].Value.Replace("amp;", ""), path);

                        var titlePriceList = titlePrice.Trim().Split(" ").ToList();
                        var priceIndex = titlePriceList.FindIndex(s => s.Contains("Цена"));
                        var price = titlePriceList[++priceIndex];
                        var title = $"{titlePriceList[0]} {titlePriceList[1]} {titlePriceList[2]}";

                        var content = new Content()
                        {
                            ContentType = ContentType.Item,
                            CategoryId = subCategoryContent.Id,
                            Description = description,
                            ImgSrc = path,
                            Title = title,
                            Price = price
                        };

                        dbContext.Content.Add(content);
                        dbContext.SaveChanges();
                    }
                }
            }
        }
    }
}
