using System;
using TaskManager.Common.Models;

namespace TaskManager.Api.Models
{
    public class Common
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[] Photo { get; set; }
        public Common()
        {
            CreationDate = DateTime.Now;
        }
        public Common(CommonModel commonModel)
        {
            Name = commonModel.Name;
            Description = commonModel.Description;
            CreationDate = commonModel.CreationDate;
            Photo = commonModel.Photo;
        }
    }
}
