using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManager.Api.Models.Abstractions;
using TaskManager.Api.Models.Data;
using TaskManager.Common.Models;

namespace TaskManager.Api.Models.Services
{
    public class TasksService : AbstractionService, ICommonService<TaskModel>
    {
        private readonly ApplicationContext _db;
        public TasksService(ApplicationContext db)
        {
            _db = db;
        }

        public bool Create(TaskModel model)
        {
            bool result = DoAction(delegate ()
            {
                Task task = new Task(model);
                _db.Tasks.Add(task);
                _db.SaveChanges();
            });
            return result;
        }

        public bool Delete(int id)
        {
            bool result = DoAction(delegate ()
            {
                Task task = _db.Tasks.FirstOrDefault( t => t.Id == id);
                _db.Tasks.Remove(task);
                _db.SaveChanges();
            });
            return result;
        }

        public TaskModel Get(int id)
        {
            Task task = _db.Tasks.FirstOrDefault(t => t.Id == id);
            return task?.ToDto();
        }

        public IQueryable<CommonModel> GetTaskForUser(int userId)
        {
            return _db.Tasks.Where(t => t.CreatorId == userId || t.ExecutorId == userId)
                            .Select(t => t.ToDto() as CommonModel);
        }

        public bool Update(int id, TaskModel taskModel)
        {
            bool result = DoAction(delegate ()
            {
                Task task = _db.Tasks.FirstOrDefault(t => t.Id == id);

                task.Name = taskModel.Name;
                task.Description = taskModel.Description;
                task.Photo = taskModel.Photo;
                task.StartDate = taskModel.CreationDate;
                task.EndDate = taskModel.EndDate;
                task.File = taskModel.File;
                task.DeskId = taskModel.DeskId;
                task.Column = taskModel.Column;
                task.CreatorId = taskModel.CreatorId;
                task.ExecutorId = taskModel.ExecutorId;

                _db.Tasks.Update(task);
                _db.SaveChanges();
            });
            return result;
        }

        public IQueryable<CommonModel> GetAll(int deskId)
        {
            return _db.Tasks.Where(t => t.DeskId == deskId)
                            .Select(t => t.ToDto() as CommonModel);
        }
    }
}
