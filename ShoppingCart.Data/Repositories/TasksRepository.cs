using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class TasksRepository : ITasksRepository
    {
        ShoppingCartDbContext _context;

        public TasksRepository(ShoppingCartDbContext context)
        {
            _context = context;
        }

        public Guid AddTask(Task task)
        {
            task.id = Guid.NewGuid();
            _context.Tasks.Add(task);
            _context.SaveChanges();

            return task.id;
        }

        public void DeleteTask(Guid id)
        {
            Task t = GetTask(id);
            _context.Tasks.Remove(t);
            _context.SaveChanges();
            
        }

        public Task GetTask(Guid id)
        {
            return _context.Tasks.SingleOrDefault(x => x.id == id);
        }

        public IQueryable<Task> GetTasks()
        {
            return _context.Tasks;
        }
    }
}
