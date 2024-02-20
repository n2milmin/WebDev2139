using Lab2.Data;
using Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Controllers
{
    [Route("Task")]
    public class TaskController : Controller
    {
        private readonly AppDbContext _db;

        public TaskController(AppDbContext context) 
        {
            _db = context;
        }

        [HttpGet("Index/{projectId:int}")]
        public IActionResult Index(int projectId)
        {
            var tasks = _db.ProjectTasks 
                .Where(task => task.ProjectId == projectId)
                .ToList();
            ViewBag.ProjectId = projectId;
            return View(tasks);
        }
        [HttpGet("Details/{id:int}")]
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
        [HttpGet("Create/{projectId:int}")]
        public IActionResult Create(int projectId)
        {
            var project = _db.Projects.Find(projectId);
            if (project == null)
            {
                return NotFound();
            }

            var task = new ProjectTask
            {
                ProjectId = projectId 
            };
            return View(task);
        }

        [HttpPost("Create/{projectId:int}")]
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

        [HttpGet("Edit/{id:int}")]
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

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task)
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
        public IActionResult DeleteConfirmed (int projectTaskId)
        {
            var task = _db.ProjectTasks.Find(projectTaskId);
            if (task != null)
            {
                _db.ProjectTasks.Remove(task);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }
            return NotFound();
        }

        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var projectsQuery = from p in _db.Projects
                                select p;

            bool searchPerformed = !String.IsNullOrEmpty(searchString);

            if (searchPerformed)
            {
                projectsQuery = projectsQuery.Where(p => p.Name.Contains(searchString)
                                               || p.Description.Contains(searchString));
            }

            var projects = await projectsQuery.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", projects); // Reuse the Index view to display results
        }

        [HttpGet("Search/{projectId:int}/{searchString?}")]
        public async Task<IActionResult> Search(int projectId, string searchString)
        {
            var tasksQuery = _db.ProjectTasks.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                tasksQuery = tasksQuery
                    .Where(t => t.Title.Contains(searchString)
                            || t.Description.Contains(searchString));
            }
            var tasks = await tasksQuery.ToListAsync();
            ViewBag.ProjectId = projectId;
            return View("Index", tasks);
        }
    }
}
