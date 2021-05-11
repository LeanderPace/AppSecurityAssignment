using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly IMembersService _membersService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITasksService taskService, IMembersService membersService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _membersService = membersService;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            try
            {
                string email;

                if (User.IsInRole("teacher"))
                {
                    email = User.Identity.Name;
                }
                else
                {
                    var member = _membersService.GetMember(User.Identity.Name);
                    email = member.LecturerEmail;
                }

                var taskList = _taskService.GetTasks(email);
                return View(taskList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpGet]
        [Authorize(Roles = "teacher")]
        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [Authorize(Roles = "teacher")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaskViewModel data)
        {
            try
            {
                data.email = User.Identity.Name;

                if (ModelState.IsValid)
                {
                    if(data.deadline > DateTime.Now || data.deadline < data.issueDate)
                    {
                        _taskService.AddTask(data);

                        TempData["Message"] = "Task created successfully";
                        _logger.LogInformation("Task created successfully |" + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("deadline", "Incorrect deadline date");
                        return View(data);
                    }         
                }
                else
                {
                    ModelState.AddModelError("", "Error creating task");
                    _logger.LogError("Error creating task |" + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                    return View(data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                return RedirectToAction("Error");
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
                _logger.LogInformation("Successfully Deleted Task |" + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["danger"] = "Something went wrong";
                _logger.LogError(ex.Message + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                return RedirectToAction("Index");
            }
        }
        public static string GetIpAddress()
        {
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ipEntry.AddressList;
            return addr[1].ToString();
        }
    }
}
