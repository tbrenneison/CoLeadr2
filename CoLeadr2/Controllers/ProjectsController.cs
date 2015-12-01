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
    public class ProjectsController : Controller
    {
        private CoLeadr2Context db = new CoLeadr2Context();

        // GET: Projects
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            //make a list of member names based on project pprs
            List<string> MemberNames = new List<string>(); 
            foreach(PersonProjectRecord ppr in project.PersonProjectRecords)
            {
                MemberNames.Add(ppr.GetMemberName());
            }

            //viewmodel
            ProjectCreateViewModel viewmodel = new ProjectCreateViewModel();
            viewmodel.Name = project.Name;
            viewmodel.EndDate = project.EndDate;
            viewmodel.ProjectGroups = project.ProjectGroups.ToList();
            viewmodel.ProjectMembers = MemberNames;
            viewmodel.ProjectId = project.ProjectId; 

            

            return View(viewmodel);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ProjectCreateViewModel viewmodel = new ProjectCreateViewModel();
            List<Group> AllGroups = new List<Group>(); 
            foreach(Group g in db.Groups)
            {
                AllGroups.Add(g); 
            }
            viewmodel.AllAvailableGroups = AllGroups;
            return View(viewmodel);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ProjectId,Name,EndDate")] Project project)
        public ActionResult Create(ProjectCreateViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                Project project = new Project();
                project.Name = viewmodel.Name;
                project.EndDate = viewmodel.EndDate;

                //add groups to project 
                    //you need to assign from an existing list
                List<Group> GroupsForProject = new List<Group>(); 
                foreach(int groupId in viewmodel.SelectedGroupIds)
                {
                    Group group = db.Groups.Find(groupId);
                    GroupsForProject.Add(group); 
                }
                project.ProjectGroups = GroupsForProject; 

                db.Projects.Add(project);
                db.SaveChanges();

                //create pprs for every individual in the group
                    //with RemoveWithGroup flag == true
                foreach(Group group in GroupsForProject)
                {
                    foreach(Person member in group.Members)
                    {
                        member.CreateNewRecord(member.PersonId, project.ProjectId, true);
                    }
                }
                


                return RedirectToAction("Index");
            }

            return View(viewmodel);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            ProjectCreateViewModel viewmodel = new ProjectCreateViewModel();
            viewmodel.Name = project.Name;
            viewmodel.EndDate = project.EndDate;
            viewmodel.Project = project;
            viewmodel.ProjectId = project.ProjectId; 
            viewmodel.ProjectGroups = project.ProjectGroups.ToList();

            List<Group> AllGroups = new List<Group>(); 
            foreach(Group g in db.Groups)
            {
                AllGroups.Add(g); 
            }
            viewmodel.AllAvailableGroups = AllGroups; 

            return View(viewmodel);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ProjectId,Name,EndDate")] Project project)
        public ActionResult Edit(ProjectCreateViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                Project project = db.Projects.Find(viewmodel.ProjectId);

                project.Name = viewmodel.Name;
                project.EndDate = viewmodel.EndDate;

                //if selected Groups in viewmodel do not match existing groups
                //clear out pprs with rwg flag set to true
                List<PersonProjectRecord> toRemove = new List<PersonProjectRecord>(); 
                if(viewmodel.SelectedGroupIds != null)
                {
                    foreach(Group group in project.ProjectGroups)
                    {
                        //if the group is not in the selected groups
                        if(viewmodel.SelectedGroupIds.Contains(group.GroupId) != true)
                        {
                            foreach(Project p in group.Projects)
                            {
                                foreach(PersonProjectRecord ppr in p.PersonProjectRecords)
                                {
                                    //if projectId matchs project and RMG is true
                                    if(ppr.ProjectId == project.ProjectId && ppr.RemoveWithGroup == true)
                                    {
                                        toRemove.Add(ppr); 
                                    }
                                }
                            }
                        }
                    }
                }
                else if(viewmodel.SelectedGroupIds == null)
                {
                    foreach (PersonProjectRecord ppr in db.PersonProjectRecords)
                    {
                        if (ppr.ProjectId == project.ProjectId && ppr.RemoveWithGroup == true)
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

                //compare selected groupIds to existing groups and create pprs for newly-added groups
                if(viewmodel.SelectedGroupIds != null)
                {
                    //list of existing groups
                    List<int> ExistingGroupIds = new List<int>(); 
                    foreach(Group g in project.ProjectGroups)
                    {
                        ExistingGroupIds.Add(g.GroupId); 
                    }
                    foreach(var Id in viewmodel.SelectedGroupIds)
                    {   //if the group is newly-added
                        if(ExistingGroupIds.Contains(Id) != true)
                        {   //create a ppr for its members
                            Group group = db.Groups.Find(Id); 
                            foreach(Person member in group.Members)
                            {
                                member.CreateNewRecord(member.PersonId, project.ProjectId, true); 
                            }
                        }
                    }
                }

                //clear all project groups and add the selected groups
                project.ClearGroups(); 
                List<Group> ProjGroups = new List<Group>();
                if (viewmodel.SelectedGroupIds != null)
                {
                    foreach (int GroupId in viewmodel.SelectedGroupIds)
                    {
                        ProjGroups.Add(db.Groups.Find(GroupId));
                    }
                }
                project.ProjectGroups = ProjGroups; 

                //save changes
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewmodel);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
    }
}
