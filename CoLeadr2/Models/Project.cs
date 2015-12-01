using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoLeadr2.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "End Date")]
        public string EndDate { get; set; }

        [Display(Name = "Project Groups")]
        public virtual ICollection<Group> ProjectGroups { get; set; }
        [Display(Name = "Project Tasks")]
        public virtual ICollection<ProjectTask> ProjectTasks { get; set; }
        //no one sees these
        public virtual ICollection<PersonProjectRecord> PersonProjectRecords { get; set; }

        //METHODS!!! 
        //clear all project groups 
        public void ClearGroups()
        {
            CoLeadr2Context db = new CoLeadr2Context();
            Project project = db.Projects.Find(this.ProjectId);

            //you've got to remove them this way (via list) because otherwise messes with the enumerator 
            //hella exceptions
            List<Group> thesegroups = new List<Group>();
            foreach (Group g in project.ProjectGroups)
            {
                thesegroups.Add(g);
            }
            foreach (Group g in thesegroups)
            {
                project.ProjectGroups.Remove(g);
            }
            db.SaveChanges();
        }
    }
}