using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Api.Models.Data;
using TaskManager.Api.Models.Services;
using TaskManager.Common.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UserService _userService;
        private readonly TasksService _tasksService;
        private readonly DesksService _desksService;

        public TasksController(ApplicationContext db)
        {
            _db = db;
            _tasksService = new TasksService(db);
            _desksService = new DesksService(db);
            _userService = new UserService(db);
        }

        [HttpGet]
        public async Task<IEnumerable<CommonModel>> GetTasksByDesk(int deskId)
        {
            return await _tasksService.GetAll(deskId).ToListAsync();
        }

        [HttpGet("user")]
        public async Task<IEnumerable<CommonModel>> GetTasksForCurrentUSer()
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                return await _tasksService.GetTaskForUser(user.Id).ToListAsync();
            }
            return Array.Empty<CommonModel>();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var task = _tasksService.Get(id);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TaskModel model)
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (model != null)
                {
                    model.CreatorId = user.Id;
                    bool result = _tasksService.Create(model);
                    return result ? Ok() : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _tasksService.Delete(id);

            return result ? Ok() : NotFound();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] TaskModel taskModel)
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (taskModel != null)
                {
                    bool result = _tasksService.Update(id, taskModel);
                    return result ? Ok() : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }
    }
}
