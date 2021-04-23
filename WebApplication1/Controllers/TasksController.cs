using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;

namespace WebApplication1.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITasksService _taskService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITasksService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var taskList = _taskService.GetTasks();
            return View(taskList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TaskViewModel data)
        {
            data.email = User.Identity.Name;

            if (ModelState.IsValid)
            {
                _taskService.AddTask(data);

                TempData["Message"] = "Task created successfully";
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Error creating task");
                return View(data);
            }
        }
    }
}
