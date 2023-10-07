using System.Collections.Generic;

namespace TaskManager.Common.Models
{
    public class ProjectModel : CommonModel
    {
        public ProjectStatus Status { get; set; }
        public int? AdminId { get; set; }
        public List<int> UsersIds { get; set; }
        public List<int> DesksIds{ get; set; }
    }
}
