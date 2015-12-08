using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoLeadr2.Models.ViewModels
{
    public class ProjectCreateViewModel
    {
        [Key]
        public int ProjectId { get; set; }
        [Display(Name = "Project")]
        public Project Project { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "End Date")]
        public string EndDate { get; set; }

        [Display(Name = "Project Groups")]
        public List<Group> ProjectGroups { get; set; }
        [Display(Name = "Project Members")]
        public List<Person> ProjectMembers { get; set; }


        [Display(Name = "All Groups")]
        public List<Group> AllAvailableGroups { get; set; }
        [Display(Name = "All Members")]
        public List<Person>AllAvailablePeople { get; set; }

        //no one sees this
        public int[] SelectedGroupIds { get; set; }

        public int[] SelectedPersonIds { get; set; }

        public List<ProjectTask> ProjectTasks { get; set; }


        //METHODS!!!
        
        public List<Group> GetAllGroups()
        {
            CoLeadr2Context db = new CoLeadr2Context();

            List<Group> AllGroups = new List<Group>();
            foreach (Group g in db.Groups)
            {
                AllGroups.Add(g);
            }

            return AllGroups;
        }

        public List<Person> GetAllMembers()
        {
            CoLeadr2Context db = new CoLeadr2Context();

            List<Person> AllMembers = new List<Person>();
            foreach (Person p in db.People)
            {
                AllMembers.Add(p);
            }

            return AllMembers;
        }
    }
}