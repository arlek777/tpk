using System.ComponentModel.DataAnnotations;

namespace TPK.Web.Models
{
    public enum ContentType
    {
        Category,
        Item
    }

    public class Content
    {
        [Key]
        public int Id { get; set; }
        public int? CategoryId { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string ImgSrc { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }

        [Required]
        public ContentType ContentType { get; set; }
    }
}