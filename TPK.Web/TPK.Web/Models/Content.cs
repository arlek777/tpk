using System.ComponentModel.DataAnnotations;

namespace TPK.Web.Models
{
    public enum ContentType
    {
        Category,
        Item
    }

    public enum ContentTypeResult
    {
        Categories,
        Items,
        ItemDetails
    }

    public class Content
    {
        [Key]
        public int Id { get; set; }
        public int? CategoryId { get; set; }

        [Required]
        public string Title { get; set; }
        public string ImgSrc { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }

        [Required]
        public ContentType ContentType { get; set; }
    }
}