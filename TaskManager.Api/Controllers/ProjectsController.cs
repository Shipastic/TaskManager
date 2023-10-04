using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Api.Models.Data;
using TaskManager.Api.Models.Services;
using TaskManager.Common.Models;

namespace TaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UserService _userService;
        private readonly ProjectService _projectService;
        public ProjectsController(ApplicationContext db)
        {
            _db = db;
            _userService = new UserService(db);
            _projectService = new ProjectService(db);
        }

        [HttpGet]
        public async Task<IEnumerable<ProjectModel>> Get()
        {
            return await _db.Projects.Select(p => p.ToDto()).ToListAsync();
        }
        [HttpPost]
        public IActionResult Create([FromBody] ProjectModel projectModel)
        {
            if (projectModel != null)
            {
                var user = _userService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    var admin = _db.ProjectAdmins.FirstOrDefault(a => a.UserId == user.Id);
                    if (admin == null)
                    {
                        admin = new Models.ProjectAdmin(user);
                        _db.ProjectAdmins.Add(admin);
                    }
                    projectModel.AdminId = admin.Id;
                }

                bool result = _projectService.Create(projectModel);

                return result ? Ok() : NotFound();
            }
            return BadRequest();
            
        }

        [HttpPatch]
        public IActionResult Update(int id, [FromBody] ProjectModel projectModel)
        {
            if (projectModel != null)
            {
                bool result = _projectService.Update(id, projectModel);

                return result ? Ok() : NotFound();
            }
            return BadRequest();
            
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            bool result = _projectService.Delete(id);

            return result ? Ok() : NotFound();
            
        }
    }
}
