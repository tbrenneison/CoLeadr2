using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoLeadr2.Models.ViewModels
{
    public class PersonGroupingViewModel
    {
        [Key]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<Group> Memberships { get; set; }
        public List<Group> AllAvailableGroups { get; set; }
        public int[] SelectedGroupIds { get; set; }


    }
}