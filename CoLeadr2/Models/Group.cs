using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoLeadr2.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        [Display(Name="Name")]
        public string Name { get; set; }

        public virtual List<Person> Members { get; set; }
        public virtual List<Project> Projects { get; set; }
    }

    
}