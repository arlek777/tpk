namespace TPK.Web.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int ParentCategoryId { get; set; }
        public string Title { get; set; }
        public string ImgSrc { get; set; }
    }
}