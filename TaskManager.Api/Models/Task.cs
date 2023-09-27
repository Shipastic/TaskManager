using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Api.Models
{
    public class Task : Common
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte[] File { get; set; }
        public int DeskId { get; set; }
        public Desk Desk { get; set; }
        public string Column { get; set; }
        public int? CreatorId { get; set; }
        public User Creator { get; set; }
        public int? ExecutorId { get; set; }

    }
}
