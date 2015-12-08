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

            //viewmodel
            ProjectCreateViewModel viewmodel = new ProjectCreateViewModel();
            viewmodel.Name = project.Name;
            viewmodel.EndDate = project.EndDate;
            viewmodel.ProjectGroups = project.ProjectGroups.ToList();
            viewmodel.ProjectMembers = project.ProjectMembers.ToList();
            viewmodel.ProjectId = project.ProjectId;
            viewmodel.ProjectTasks = project.ProjectTasks.ToList();

            

            return View(viewmodel);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ProjectCreateViewModel viewmodel = new ProjectCreateViewModel();

            viewmodel.AllAvailableGroups = viewmodel.GetAllGroups();
            viewmodel.AllAvailablePeople = viewmodel.GetAllMembers();

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
                if (viewmodel.SelectedGroupIds != null)
                {
                    foreach (int groupId in viewmodel.SelectedGroupIds)
                    {
                        Group group = db.Groups.Find(groupId);
                        GroupsForProject.Add(group);
                    }
                }
                project.ProjectGroups = GroupsForProject;

                //add people to project 
                List<Person> MembersForProject = new List<Person>(); 
                if (viewmodel.SelectedPersonIds != null)
                {
                    foreach (int personId in viewmodel.SelectedPersonIds)
                    {
                        Person person = db.People.Find(personId);
                        MembersForProject.Add(person); 
                    }
                }
                project.ProjectMembers = MembersForProject; 

                db.Projects.Add(project);
                db.SaveChanges();

                return RedirectToAction("AddProjectTask", "ProjectTasks", new { projectId = project.ProjectId } );
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
            viewmodel.ProjectMembers = project.ProjectMembers.ToList(); 

            List<Group> AllGroups = new List<Group>(); 
            foreach(Group g in db.Groups)
            {
                AllGroups.Add(g); 
            }
            viewmodel.AllAvailableGroups = AllGroups;

            List<Person> AllPeople = new List<Person>(); 
            foreach(Person p in db.People)
            {
                AllPeople.Add(p); 
            }
            viewmodel.AllAvailablePeople = AllPeople;

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

                //clear all project members and add selected members
                project.ClearMembers();
                List<Person> ProjMembers = new List<Person>(); 
                if(viewmodel.SelectedPersonIds != null)
                {
                    foreach(int PersonId in viewmodel.SelectedPersonIds)
                    {
                        ProjMembers.Add(db.People.Find(PersonId)); 
                    }
                }
                project.ProjectMembers = ProjMembers; 


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
