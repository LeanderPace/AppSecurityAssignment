using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class SubmissionViewModel
    {
        public Guid id { get; set; }

        public string studentName { get; set; }

        public string studentSurname { get; set; }

        public string email { get; set; }

        public string file { get; set; }

        public string signature { get; set; }

        public TaskViewModel task { get; set; }
    }
}
