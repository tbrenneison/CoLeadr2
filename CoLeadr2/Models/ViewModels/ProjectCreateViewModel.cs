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
        [Display(Name = "All Groups")]
        public List<Group> AllAvailableGroups { get; set; }
        //no one sees this
        public int[] SelectedGroupIds { get; set; }

        public List<string>ProjectMembers { get; set; }
    }
}