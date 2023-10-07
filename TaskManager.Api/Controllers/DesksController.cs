using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize]
    public class DesksController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UserService _userService;

        public DesksController(ApplicationContext db)
        {
            _db = db;
            _userService = new UserService(db);
        }


        [HttpGet]
        public async Task<IEnumerable<CommonModel>> GetDesksForCurrentUser()
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);

        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

        }

        [HttpGet("project/{projectId}")]
        public IActionResult GetProjectDesks(int projectId)
        {

        }


        [HttpPost]
        public IActionResult Create([FromBody] DeskModel deskModel)
        {

        }


        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] DeskModel deskModel)
        {
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
        }
    }
}
