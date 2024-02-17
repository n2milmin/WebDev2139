using Lab2.Data;
using Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Controllers
{
    public class TaskController : Controller
    {
        private readonly AppDbContext _db;

        public TaskController(AppDbContext context) 
        {
            _db = context;
        }
            
        public IActionResult Index(int projectId)
        {
            var tasks = _db.ProjectTasks 
                .Where(task => task.ProjectId == projectId)
                .ToList();
            ViewBag.ProjectId = projectId;
            return View(tasks);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var task = _db.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefault(t => t.ProjectTaskId == id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }
        
        public IActionResult Create(int projectId)
        {
            var project = _db.Projects.Find(projectId);
            if (project == null)
            {
                return NotFound();
            }

            var task = new ProjectTask
                {  ProjectId = projectId };
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                _db.ProjectTasks.Add(task);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new {projectId = task.ProjectId});
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var task = _db.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefault(t => t.ProjectTaskId == id);
            if (task == null)
            {
                return NotFound();
            }
            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (id != task.ProjectTaskId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Update(task);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpGet("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var task = _db.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefault(t => t.ProjectTaskId == id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed (int ProjectTaskId)
        {
            var task = _db.ProjectTasks.Find(ProjectTaskId);
            if (task != null)
            {
                _db.ProjectTasks.Remove(task);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }
            return NotFound();
        }

        
    }
}
