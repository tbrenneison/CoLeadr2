using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CoLeadr2.Models
{
    public class PersonProjectRecord
    {
        public int PersonProjectRecordId { get; set; }
        public int PersonId { get; set; }
        public int ProjectId { get; set; }
        public bool RemoveWithGroup { get; set; }

        
        public string GetMemberName()
        {
            CoLeadr2Context db = new CoLeadr2Context();
            Person person = db.People.Find(this.PersonId);

            StringBuilder name = new StringBuilder();
            name.Append(person.FirstName).Append(" ").Append(person.LastName);
            string FullName = name.ToString();
            return FullName; 
        }
        
    }

}