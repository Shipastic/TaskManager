﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Api.Models.Abstractions;
using TaskManager.Api.Models.Data;
using TaskManager.Common.Models;

namespace TaskManager.Api.Models.Services
{
    public class DesksService : AbstractionService, ICommonService<DeskModel>
    {

        private readonly ApplicationContext _db;
        public DesksService(ApplicationContext db)
        {
            _db = db;
        }
        public bool Create(DeskModel model)
        {
            bool result = DoAction(delegate ()
            {
                Desk newDesk = new Desk(model);
                _db.Desks.Add(newDesk);
                _db.SaveChanges();
            });
            return result;

        }

        public bool Delete(int id)
        {
            bool result = DoAction(delegate ()
            {
                Desk newDesk = _db.Desks.FirstOrDefault(d => d.Id == id);
                _db.Desks.Remove(newDesk);
                _db.SaveChanges();
            });
            return result;
        }

        public DeskModel Get(int id)
        {
            Desk desk = _db.Desks.Include(d => d.Tasks).FirstOrDefault(d => d.Id == id);
            var deskModel = desk?.ToDto();
            if (deskModel != null)
            {
                deskModel.TasksIds = desk.Tasks.Select(t => t.Id).ToList();
            }
            return deskModel;
        }

        public bool Update(int id, DeskModel model)
        {
            bool result = DoAction(delegate ()
            {
                Desk desk = _db.Desks.FirstOrDefault(d => d.Id == id);
                desk.Name = model.Name;
                desk.Description = model.Description;
                desk.Photo = model.Photo;
                desk.AdminId = model.AdminId;
                desk.IsPrivate = model.IsPrivate;
                desk.Columns = JsonConvert.SerializeObject(model.Columns);
                desk.ProjectId = model.ProjectId;
                _db.Desks.Update(desk);
                _db.SaveChanges();
            });
            return result;
        }

        public IQueryable<CommonModel> GetAll()
        {
            return _db.Desks.Select(d => d.ToDto() as CommonModel);
        }

        public IQueryable<CommonModel> GetProjectDesks(int projectId, int userId)
        {
            return _db.Desks.Where(d => (d.ProjectId == projectId && 
                                  (d.AdminId == userId || d.IsPrivate == false))).Select(d => d.ToDto() as CommonModel);
        }
    }
}