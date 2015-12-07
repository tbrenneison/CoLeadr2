using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoLeadr2.Models
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }

        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "State")]
        public string State { get; set; }
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Display(Name = "Group Memberships")]
        public virtual List<Group> Groups { get; set; }
        [Display(Name = "Projects")]
        public virtual List<Project> Projects { get; set; }


        //METHODS!!!

        public List<int> GetGroupTags()
        {
            CoLeadr2Context db = new CoLeadr2Context();
            //gets all the ids of the groups this person has membership in and puts them in a list
            Person person = db.People.Find(this.PersonId);
            List<int> GroupTags = new List<int>();
            foreach(Group g in person.Groups)
            {
                GroupTags.Add(g.GroupId); 
            }

            return GroupTags; 
        }

        //adds person to a group
            //and adds person to that group's projects with removewithgroup flag == true
        public void AddToGroup(int GroupId)
        {
            CoLeadr2Context db = new CoLeadr2Context();

            Person person = db.People.Find(this.PersonId);
            Group group = db.Groups.Find(GroupId);

            group.Members.Add(person);
            foreach(Project project in group.Projects)
            {
                CreateNewRecord(this.PersonId, project.ProjectId, true); 
            }
            
            db.SaveChanges();
        }


        //clears out the person's group memberships entirely 
        public void ClearGroups()
        {
            CoLeadr2Context db = new CoLeadr2Context();
            Person person = db.People.Find(this.PersonId);

            //you've got to remove them this way (via list) because otherwise messes with the enumerator 
            //hella exceptions
            List<Group> thesegroups = new List<Group>();
            foreach (Group g in person.Groups)
            {
                thesegroups.Add(g);
            }
            foreach (Group g in thesegroups)
            {
                person.Groups.Remove(g);
            }
            db.SaveChanges();
        }

        //create new personprojectrecord
        public void CreateNewRecord(int PersonId, int ProjectId, bool RemoveWithGroup)
        {
            CoLeadr2Context db = new CoLeadr2Context(); 

            PersonProjectRecord ppr = new PersonProjectRecord();
            ppr.PersonId = PersonId;
            ppr.ProjectId = ProjectId;
            ppr.RemoveWithGroup = RemoveWithGroup;

            db.PersonProjectRecords.Add(ppr);
            db.SaveChanges(); 
        }

     

    }
}