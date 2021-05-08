using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class MemberViewModel
    {
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name ="First Name")]
        [Required(ErrorMessage ="Name cannot be left empty")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LecturerEmail { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}
