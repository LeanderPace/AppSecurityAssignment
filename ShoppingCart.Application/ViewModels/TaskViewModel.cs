using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class TaskViewModel
    {
        public Guid id { get; set; }
        public string taskName { get; set; }
        public string description { get; set; }
        public DateTime issueDate { get; set; }
        public DateTime deadline { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
    }
}
