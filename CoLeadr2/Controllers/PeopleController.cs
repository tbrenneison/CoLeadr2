using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CoLeadr2.Models;
using CoLeadr2.Models.ViewModels;

namespace CoLeadr2.Controllers
{
    public class PeopleController : Controller
    {
        private CoLeadr2Context db = new CoLeadr2Context();

        // GET: People
        public ActionResult Index()
        {
            return View(db.People.ToList());
        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonId,FirstName,LastName,PhoneNumber,StreetAddress,City,State,PostCode,EmailAddress")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonId,FirstName,LastName,PhoneNumber,StreetAddress,City,State,PostCode,EmailAddress")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Get: People/MembershipManager
        public ActionResult MembershipManager(int? id)
        {
            Person person = db.People.Find(id);
            PersonGroupingViewModel viewmodel = new PersonGroupingViewModel();
            viewmodel.Person = person;
            viewmodel.PersonId = person.PersonId;
            viewmodel.FirstName = person.FirstName;
            viewmodel.LastName = person.LastName;
            viewmodel.Memberships = person.Groups;
            //because alphabetical is easier to look at 
            viewmodel.AllAvailableGroups = db.Groups.OrderBy(m => m.Name).ToList();
            return View(viewmodel);
        }

        //POST: People/MembershipManager
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MembershipManager(PersonGroupingViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                Person person = db.People.Find(viewmodel.PersonId);
                

                //determine what groups the person was already in by comparing existing and checked groups:
                List<Group> existingGroups = new List<Group>(); 
                foreach(Group g in person.Groups)
                {
                    existingGroups.Add(g); 
                }
                List<Group> checkedGroups = new List<Group>();
                if (viewmodel.SelectedGroupIds != null)
                {
                    foreach (var GroupId in viewmodel.SelectedGroupIds)
                    {
                        Group thisGroup = db.Groups.Find(GroupId);
                        checkedGroups.Add(thisGroup);
                    }
                }
                //add/remove people from groups 
                foreach (Group g in db.Groups)
                {
                    if (existingGroups.Contains(g) == true && checkedGroups.Contains(g) == false)
                    {
                        person.RemoveFromGroup(g.GroupId);
                    }
                    else if (existingGroups.Contains(g) == false && checkedGroups.Contains(g) == true)
                    {
                        person.AddToGroup(g.GroupId);
                    }
                    else
                    {
                        //do nothing
                    }
                }
                db.SaveChanges();


                /*
                List<PersonProjectRecord> toRemove = new List<PersonProjectRecord>();
                if (viewmodel.SelectedGroupIds != null)
                {
                    //see if viewmodel selectedIds match previous Group data
                    //clear out pprs for groups that do not match viewmodel Ids
                    foreach (Group group in person.Groups)
                    {
                        //if a group is not in the viewmodel selectedIds
                        if (viewmodel.SelectedGroupIds.Contains(group.GroupId) != true)
                        {
                            //and if the person is on a project that group is on and their rmg is set to true
                            foreach (Project p in group.Projects)
                            {
                                foreach (PersonProjectRecord ppr in p.PersonProjectRecords)
                                {
                                    if (ppr.PersonId == person.PersonId && ppr.RemoveWithGroup == true)
                                    {
                                        //add to "toRemove" list
                                        toRemove.Add(ppr);
                                    }
                                }
                            }
                        }
                    }
                   
                }
                //else if no groups are selected (remove all unflagged pprs)
                else if(viewmodel.SelectedGroupIds == null)
                {
                    foreach(PersonProjectRecord ppr in db.PersonProjectRecords)
                    {
                        if(ppr.PersonId == person.PersonId && ppr.RemoveWithGroup == true)
                        {
                            toRemove.Add(ppr);
                        }
                    }
                }
                else
                {
                    //do nothing
                }
                //actually remove pprs
                foreach (PersonProjectRecord ppr in toRemove)
                {
                    db.PersonProjectRecords.Remove(ppr);
                }
                db.SaveChanges();

                if (person.Groups != null && viewmodel.SelectedGroupIds != null)
                {
                    List<Group> existingGroups = new List<Group>();
                    foreach (int id in viewmodel.SelectedGroupIds)
                    {
                        Group group = db.Groups.Find(id);
                        existingGroups.Add(group);
                    }
                }
               
                //determine if any group memberships have changed
                bool hasChanged = false;
                    //if the selected groups and previous groups are not the same set hasChanged to true
                if (viewmodel.SelectedGroupIds != null)
                {
                    foreach (var Groupid in viewmodel.SelectedGroupIds)
                    {
                        Group group = db.Groups.Find(Groupid);
                        if (person.Groups.Contains(group) != true)
                        {
                            hasChanged = true;
                        }
                    }
                }
                    //if the person is being added to groups and previously was not in any groups set hasChanged to true
                if( person.Groups == null && viewmodel.SelectedGroupIds != null)
                {
                    hasChanged = true; 
                }

                if (hasChanged == true)
                {

                    //person should only be in groups with selected ids, so clear out all their groups 
                    person.ClearGroups();
                    //if there are selected ids, add the person to those groups
                    if (viewmodel.SelectedGroupIds != null)
                    {
                        foreach (int id in viewmodel.SelectedGroupIds)
                        {
                            person.AddToGroup(id);
                        }
                    }
                }
               */
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Details");
            }
           
        }
    }
}
