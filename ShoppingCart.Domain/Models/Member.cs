using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class Member
    {
        [Key]
        public string Email { get; set; }
        public string LecturerEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
    }
}
