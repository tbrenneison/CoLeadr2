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

        public string GetMemberIds()
        {
            CoLeadr2Context db = new CoLeadr2Context();
            Group group = db.Groups.Find(this.GroupId);

            List<int> Ids = new List<int>(); 
            foreach(Person member in group.Members)
            {
                Ids.Add(member.PersonId); 
            }

            string MemberIds = string.Join(",", Ids);

            return MemberIds; 
        }


    }  
}