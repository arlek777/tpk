using System;
using System.Diagnostics;
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

        public static bool ImportToDb(TPKDbContext dbContext, string rootPath)
        {
            Content categoryContent = null;
            Content subCategoryContent = null;
            Content content = null;

            try
            {
                var web = new HtmlWeb();
                var homePage = web.Load(BaseUrl);

                var desc = homePage.DocumentNode.Descendants();
                var categories = desc.Where(n => n.Attributes.Any(a => a.Value.Contains("pngfile jg_icon")));
                foreach (var category in categories)
                {
                    var categoryA = category.ParentNode.ChildNodes.FindFirst("a");
                    categoryContent = new Content()
                    {
                        ContentType = ContentType.Category,
                        Title = categoryA.InnerText.Replace("\n", "").Trim()
                    };

                    dbContext.Content.Add(categoryContent);
                    dbContext.SaveChanges();

                    var subCategoryPage = web.Load(BaseUrl + categoryA.Attributes["href"].Value.Replace("amp;", ""));
                    var subCategories = subCategoryPage.DocumentNode.Descendants()
                        .Where(n => n.Attributes.Any(a => a.Value.Contains("pngfile jg_icon")));

                    foreach (var subCategory in subCategories)
                    {
                        var subCategoryA = subCategory.ParentNode.ChildNodes.FindFirst("a");
                        subCategoryContent = new Content()
                        {
                            CategoryId = categoryContent.Id,
                            ContentType = ContentType.Category,
                            Title = subCategoryA.InnerText.Replace("\n", "").Trim()
                        };

                        dbContext.Content.Add(subCategoryContent);
                        dbContext.SaveChanges();

                        var itemPage = web.Load(BaseUrl + subCategoryA.Attributes["href"].Value.Replace("amp;", ""));
                        var itemPageDesc = itemPage.DocumentNode.Descendants();

                        var items =
                            itemPageDesc.Where(n => n.Attributes.Any(a => a.Value.Contains("jg_catelem_photo"))).ToList();

                        var navLinks = itemPageDesc
                            .Where(n => n.Name == "a" &&
                                        n.Attributes.Any(a => a.Value.Contains("jg_pagenav")))
                            .Select(n => n.GetAttributeValue("href", ""))
                            .Distinct();

                        foreach(var navLink in navLinks)
                        {
                            var nextItemPage = web.Load(BaseUrl + navLink.Replace("amp;", ""));

                            var nextItems = nextItemPage.DocumentNode.Descendants()
                                .Where(n => n.Attributes.Any(a => a.Value.Contains("jg_catelem_photo"))).ToList();

                            items.AddRange(nextItems);
                        }

                        foreach (var item in items)
                        {
                            var itemDetailsLink = item.Attributes["href"];
                            var itemDetailsPage = web.Load(BaseUrl + itemDetailsLink.Value.Replace("amp;", ""));

                            var itemDetailsPageDesc = itemDetailsPage.DocumentNode.Descendants();

                            var photo = itemDetailsPageDesc.FirstOrDefault(n => n.Id == "jg_photo_big");
                            var photoPathToDb = $"/Img/Items/Id{++PhotoId}.jpeg";
                            var photoPathToSave = $"{rootPath}{photoPathToDb}";

                            var titlePrice = itemDetailsPageDesc.FirstOrDefault(
                                n => n.GetAttributeValue("style", "") == "text-align:center; color:#3A5163;").InnerText;

                            var description = itemDetailsPageDesc
                                .FirstOrDefault(n => n.Id == "jg_photo_description")?.InnerText?.Trim();

                            new WebClient().DownloadFile(BaseUrl + photo.Attributes["src"].Value.Replace("amp;", ""), photoPathToSave);

                            var titlePriceList = titlePrice.Trim().Replace("&nbsp;", "").Split(' ').ToList();
                            string price = String.Empty;
                            string title = String.Empty;
                            if (titlePriceList.Count == 1)
                            {
                                title = titlePriceList.FirstOrDefault();
                            }
                            else
                            {
                                var priceIndex = titlePriceList.FindIndex(s => s.ToLower().Contains("цена"));
                                price = titlePriceList[++priceIndex];
                                title = $"{titlePriceList[0]} {titlePriceList[1]} {titlePriceList[2]}";
                            }

                            content = new Content()
                            {
                                ContentType = ContentType.Item,
                                CategoryId = subCategoryContent.Id,
                                Description = description,
                                ImgSrc = photoPathToDb,
                                Title = title,
                                Price = price
                            };

                            dbContext.Content.Add(content);
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
            catch(Exception exc)
            {
                var m = exc.Message;
                var cc = categoryContent;
                var sc = subCategoryContent;
                var c = content;
            }

            return true;
        }
    }
}
