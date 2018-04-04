using System;
using System.ComponentModel.DataAnnotations;

namespace TPK.Web.Models
{
    public class Admin
    {
        [Key]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
