using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ISubmissionsRepository
    {
        Submission GetSubmission(Guid id);
        IQueryable<Submission> GetSubmissions();
        void AddSubmission(Submission sub);
    }
}
