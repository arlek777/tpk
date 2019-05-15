using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TPK.Web.Models
{
    public class CustomerContactsModel
    {
        [Required]
        public string FullName { get; set; }

        public string Email { get; set; }
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }
    }

    public class SendOrderModel
    {
        [Required]
        public IEnumerable<int> ItemIds { get; set; }
        public CustomerContactsModel CustomerContacts { get; set; }
    }
}