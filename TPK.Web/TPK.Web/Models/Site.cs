using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TPK.Web.Models
{
    public class Site
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string HomeText { get; set; }
        public string ContactText { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
    }
}