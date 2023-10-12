using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class DesksController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UserService _userService;
        private readonly DesksService _desksService;

        public DesksController(ApplicationContext db)
        {
            _db = db;
            _userService = new UserService(db);
            _desksService = new DesksService(db);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommonModel>>> GetDesksForCurrentUser()
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                var result = await _desksService.GetAll(user.Id).ToListAsync();
                return result == null ? NoContent() : Ok(result);
            }
            return Array.Empty<CommonModel>();
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var desk = _desksService.Get(id);
            return desk == null ? NotFound() : Ok(desk);
        }

        [HttpGet("project")]
        public async Task<ActionResult<IEnumerable<CommonModel>>> GetProjectDesks(int projectId)
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                var result = await _desksService.GetProjectDesks(projectId, user.Id).ToListAsync();
                return result == null ? NoContent() : Ok(result);
            }
            return Unauthorized(Array.Empty<CommonModel>());
        }


        [HttpPost]
        public IActionResult Create([FromBody] DeskModel deskModel)
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (deskModel != null)
                {
                    deskModel.AdminId = user.Id;
                    bool result = _desksService.Create(deskModel);
                    return result ? Ok() : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }


        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] DeskModel deskModel)
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (deskModel != null)
                {
                    bool result = _desksService.Update(id, deskModel);
                    return result ? Ok() : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _desksService.Delete(id);

            return result ? Ok() : NotFound();
        }
    }
}
