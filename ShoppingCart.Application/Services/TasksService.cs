using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class TasksService : ITasksService
    {
        private IMapper _autoMapper;
        private ITasksRepository _taskRepo;

        public TasksService(IMapper autoMapper, ITasksRepository taskRepo)
        {
            _autoMapper = autoMapper;
            _taskRepo = taskRepo;
        }

        public void AddTask(TaskViewModel model)
        {
            _taskRepo.AddTask(_autoMapper.Map<Task>(model));
        }

        public void DeleteTask(Guid id)
        {
            _taskRepo.DeleteTask(id);
        }

        public TaskViewModel GetTask(Guid id)
        {
            var task = _taskRepo.GetTask(id);

            if (task == null)
                return null;
            else
            {
                var res = _autoMapper.Map<TaskViewModel>(task);
                return res;
            }     
        }

        public IQueryable<TaskViewModel> GetTasks()
        {
            return _taskRepo.GetTasks().ProjectTo<TaskViewModel>(_autoMapper.ConfigurationProvider);
        }
    }
}
