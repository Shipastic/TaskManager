using System.Collections.Generic;

namespace TaskManager.Common.Models
{
    public class ProjectModel : CommonModel
    {
        public ProjectStatus Status { get; set; }
        public int? AdminId { get; set; }
        public List<UserModel> Users { get; set; } = new List<UserModel>();
        public List<DeskModel> Desk { get; set; } = new List<DeskModel>();
    }
}
