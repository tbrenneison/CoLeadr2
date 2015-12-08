using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CoLeadr2.Models;

namespace CoLeadr2.Controllers
{
    public class ProjectTasksController : Controller
    {
        private CoLeadr2Context db = new CoLeadr2Context();

        // GET: ProjectTasks
        public ActionResult TaskList(int? projectId)
        {
            Project project = db.Projects.Find(projectId);
            ViewBag.Project = project.Name;
            ViewBag.ProjectId = project.ProjectId; 
            return View(project.ProjectTasks.ToList());
        }

        // GET: ProjectTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.ProjectTasks.Find(id);
            Project project = db.Projects.Find(projectTask.ProjectId);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.Project = project.Name;
            ViewBag.ProjectId = project.ProjectId;
            return View(projectTask);
        }

        //GET: Projects/AddProjectTask
        public ActionResult AddProjectTask(int? projectId)
        {
            Project project = db.Projects.Find(projectId);
            ViewBag.Project = project.Name;
            ViewBag.ProjectId = project.ProjectId; 
            return View();

            /*
            create, create, create, create ... another button "i'm done"
            */
        }

        // POST: ProjectTasks/AddProjectTask
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProjectTask([Bind(Include = "ProjectTaskId,ProjectId,Description,IsComplete")] ProjectTask projectTask)
        {
            if (ModelState.IsValid)
            {
                db.ProjectTasks.Add(projectTask);
                db.SaveChanges();
                return RedirectToAction("TaskList", "ProjectTasks", new { projectId = projectTask.ProjectId});
            }

            return View(projectTask);
        }


        // GET: ProjectTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.ProjectTasks.Find(id);
            Project project = db.Projects.Find(projectTask.ProjectId); 
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.Project = project.Name;
            ViewBag.ProjectId = project.ProjectId;
            return View(projectTask);
        }

        // POST: ProjectTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectTaskId,ProjectId,Description,IsComplete")] ProjectTask projectTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TaskList", new { projectId = projectTask.ProjectId});
            }
            return View(projectTask);
        }

        // GET: ProjectTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.ProjectTasks.Find(id);
            Project project = db.Projects.Find(projectTask.ProjectId);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.Project = project.Name;
            ViewBag.ProjectId = project.ProjectId;
            return View(projectTask);
        }

        // POST: ProjectTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectTask projectTask = db.ProjectTasks.Find(id);
            int projectId = projectTask.ProjectId; 
            db.ProjectTasks.Remove(projectTask);
            db.SaveChanges();
            return RedirectToAction("TaskList", new { projectId = projectId});
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
