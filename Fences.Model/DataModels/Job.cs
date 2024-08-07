using SchoolRegister.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fences.Model.DataModels
{
    public class Job
    {
        public int Id { get; set; }
        public User User { get; set; } = null!;
        public int UserId { get; set; }
        public string Town { get; set; } = null!;
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string ZipCode { get; set; } = null!;
        public JobType JobType { get; set; }
        public string? Description { get; set; }
    }
}
