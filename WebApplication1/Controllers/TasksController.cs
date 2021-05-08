using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using WebApplication1.Utility;

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
        [Authorize(Roles = "teacher")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "teacher")]
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

        [Authorize(Roles = "teacher")]
        public IActionResult Delete(string id)
        {
            try
            {
                string urlEnc = Encryption.SymmetricDecrypt(id);
                Guid decId = Guid.Parse(urlEnc);

                _taskService.DeleteTask(decId);
                TempData["feedback"] = "Product was deleted successfully";
                _logger.LogInformation("Successfully Deleted Task " + decId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["danger"] = "Something went wrong";
                _logger.LogError(ex.Message);
                return RedirectToAction("Index");
            }
        }
        
    }
}
