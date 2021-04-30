using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        ShoppingCartDbContext _context;
        public CommentsRepository(ShoppingCartDbContext context)
        {
            _context = context;
        }

        public Guid AddComment(Comment comment)
        {
            comment.id = Guid.NewGuid();
            _context.Comments.Add(comment);
            _context.SaveChanges();

            return comment.id;
        }

        public void DeleteComment(Guid id)
        {
            Comment c = GetComment(id);
            _context.Comments.Remove(c);
            _context.SaveChanges();
        }

        public Comment GetComment(Guid id)
        {
            return _context.Comments.SingleOrDefault(x => x.id == id);
        }

        public IQueryable<Comment> GetComments()
        {
            return _context.Comments;
        }
    }
}
