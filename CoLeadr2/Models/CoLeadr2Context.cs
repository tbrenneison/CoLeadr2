using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CoLeadr2.Models
{
    public class CoLeadr2Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public CoLeadr2Context() : base("name=CoLeadr2Context")
        {
        }

        public System.Data.Entity.DbSet<CoLeadr2.Models.Person> People { get; set; }

        public System.Data.Entity.DbSet<CoLeadr2.Models.Group> Groups { get; set; }

        public System.Data.Entity.DbSet<CoLeadr2.Models.ViewModels.PersonGroupingViewModel> PersonGroupingViewModels { get; set; }

        public System.Data.Entity.DbSet<CoLeadr2.Models.Project> Projects { get; set; }

        public System.Data.Entity.DbSet<CoLeadr2.Models.PersonProjectRecord> PersonProjectRecords { get; set; }

        public System.Data.Entity.DbSet<CoLeadr2.Models.ProjectTask> ProjectTasks { get; set; }
    }
}
