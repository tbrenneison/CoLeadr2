using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoLeadr2.Models
{
    public class ProjectTask
    {
        public int ProjectTaskId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }

    }
}