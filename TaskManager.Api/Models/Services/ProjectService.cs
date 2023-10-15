using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Api.Models.Abstractions;
using TaskManager.Api.Models.Data;
using TaskManager.Common.Models;

namespace TaskManager.Api.Models.Services
{
    public class ProjectService : AbstractionService, ICommonService<ProjectModel>
    {
        private readonly ApplicationContext _db;

        public ProjectService(ApplicationContext db)
        {
            _db = db;
        }

        public bool Create(ProjectModel model)
        {
            bool result = DoAction(delegate ()
            {
                Project newProject = new Project(model);
                _db.Projects.Add(newProject);
                _db.SaveChanges();
            });
            return result;
        }

        public bool Delete(int id)
        {
            bool result = DoAction(delegate ()
            {
                Project newProject = _db.Projects.FirstOrDefault(p => p.Id == id);
                _db.Projects.Remove(newProject);
                _db.SaveChanges();
            });
            return result;
        }

        public ProjectModel Get(int id)
        {
            Project newProject = _db.Projects.Include(p => p.Users).Include(d => d.Desks).FirstOrDefault(p => p.Id == id);
            var projectModel = newProject?.ToDto();
            if (projectModel != null)
            {
                projectModel.UsersIds = newProject.Users.Select(u => u.Id).ToList();
                projectModel.DesksIds = newProject.Desks.Select(d => d.Id).ToList();
            }
            return projectModel;
        }

        public async Task<IEnumerable<ProjectModel>> GetByUserId(int userId)
        {
            List<ProjectModel> result = new List<ProjectModel>();
            var admin = _db.ProjectAdmins.FirstOrDefault(a => a.UserId == userId);
            if (admin != null)
            {
                var projectForAdmin = await _db.Projects.Where(p => p.AdminId == admin.Id).Select(p => p.ToDto()).ToListAsync();
                result.AddRange(projectForAdmin);
            }
            var projectsForUser = await _db.Projects.Include(p => p.Users).Where(p => p.Users.Any(u => u.Id == userId)).Select(p => p.ToDto()).ToListAsync();
            result.AddRange(projectsForUser);
            return result;
        }

        public bool Update(int id, ProjectModel model)
        {
            bool result = DoAction(delegate ()
            {
                Project newProject = _db.Projects.FirstOrDefault(p => p.Id == id);
                newProject.Name = model.Name;
                newProject.Description = model.Description;
                newProject.Photo = model.Photo;
                newProject.Status = model.Status;
                //newProject.AdminId = model.AdminId;
                _db.Projects.Update(newProject);
                _db.SaveChanges();
            });
            return result;
        }

        public IQueryable<CommonModel> GetAll()
        {
            return  _db.Projects.Select(p => p.ToDto() as CommonModel);           
        }

        public void AddUsersToProject(int id, List<int> userIds)
        {
            Project project = _db.Projects.FirstOrDefault(p => p.Id == id);
            foreach (int userId in userIds)
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == userId);
                if (project.Users.Contains(user) == false)
                {
                    project.Users.Add(user);
                }
            }
            _db.SaveChanges();
        }

        public void RemoveUsersFromProject(int id, List<int> userIds)
        {
            Project project = _db.Projects.Include(p => p.Users).FirstOrDefault(p => p.Id == id);
            foreach (int userId in userIds)
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == userId);
                if (project.Users.Contains(user))
                {
                    project.Users.Remove(user);
                }
            }
            _db.SaveChanges();
        }
    }
}
