using System.Collections.Generic;
using TaskManager.Common.Models;
using Newtonsoft.Json;

namespace TaskManager.Api.Models
{
    public class Desk : Common
    {
        public int Id { get; set; }
        public bool IsPrivate { get; set; }
        public string Columns { get; set; }
        public User Admin { get; set; }
        public int AdminId { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public List<Task> Tasks { get; set; } = new List<Task>();

        public DeskModel ToDto()
        {
            return new DeskModel()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                AdminId = this.AdminId,
                CreationDate = this.CreationDate,
                Photo = this.Photo,
                ProjectId = this.ProjectId,
                IsPrivate = this.IsPrivate,
                Columns = JsonConvert.DeserializeObject<string[]>(this.Columns)
            };
        }

        public Desk() { }

        public Desk(DeskModel deskModel) : base(deskModel)
        {
            Id = deskModel.Id;
            AdminId = deskModel.AdminId;
            IsPrivate = deskModel.IsPrivate;
            AdminId = deskModel.AdminId;
            ProjectId = deskModel.ProjectId;
            Columns = JsonConvert.SerializeObject(deskModel.Columns);
        }

        public CommonModel ToShortDto()
        {
            return new CommonModel()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                CreationDate = this.CreationDate,
                Photo = this.Photo
            };
        }
    }
}