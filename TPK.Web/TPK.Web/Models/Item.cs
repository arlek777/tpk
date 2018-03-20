namespace TPK.Web.Models
{
    public class Item
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public string ImgSrc { get; set; }
    }
}
