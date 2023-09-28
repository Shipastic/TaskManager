using System.Collections.Generic;
using TaskManager.Common.Models;

namespace TaskManager.Api.Models
{
    public class Project : Common
    {
        public int Id { get; set; }
        public int? AdminId { get; set; }
        public ProjectAdmin Admin { get; set; }
        public List<User> Users { get; set; } = new List<User>();
        public List<Desk> Desk { get; set; } = new List<Desk>();
        public ProjectStatus Status { get; set; }
        public Project()
        {

        }
        public Project(ProjectModel projectModel) : base(projectModel)
        {
            Id = projectModel.Id;
            AdminId = projectModel.AdminId;
            Status = projectModel.Status;
        }
    }
}
