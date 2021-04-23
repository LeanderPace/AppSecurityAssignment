using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class SubmissionsRepository : ISubmissionsRepository
    {
        ShoppingCartDbContext _context;
        public SubmissionsRepository(ShoppingCartDbContext context)
        {
            _context = context;
        }
        public void AddSubmission(Submission sub)
        {
            sub.id = Guid.NewGuid();
            _context.Submissions.Add(sub);
            _context.SaveChanges();
        }

        public Submission GetSubmission(Guid id)
        {
            return _context.Submissions.SingleOrDefault(x => x.id == id);
        }

        public IQueryable<Submission> GetSubmissions()
        {
            return _context.Submissions;
        }
    }
}
