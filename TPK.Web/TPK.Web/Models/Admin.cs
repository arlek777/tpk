using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPK.Web.Models
{
    public class Admin
    {
        [Key]
        public Guid Id { get; set; }
        public string UserName { get; set; }

        public string HashPassword { get; set; }

        [NotMapped]
        public string Password { get; set; }
    }
}
