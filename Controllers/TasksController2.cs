using Lab2.Data;
using Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Controllers
{
    public class TasksController2 : Controller
    {
        private readonly AppDbContext _db;
        public TasksController2(AppDbContext context)
        {
            _db = context;
        }
        public IActionResult Index(int projectId)
        {
            var taskName = "MyTask";
            var results = _db.ProjectTasks
                .Where(x => x.Title == taskName)
                .ToList();           
            results = _db.ProjectTasks
                .Where(x => x.Title.StartsWith(taskName))
                .ToList();
            results = _db.ProjectTasks
                .Where(x => x.Title.Contains(taskName))
                .ToList();
            results = _db.ProjectTasks
                .Where(x => x.Title.Contains(taskName) 
                        || x.Description.Contains(taskName))
                .ToList();
            

            return View(results);


            var tasks = _db.ProjectTasks
                .Where(t => t.ProjectId == projectId)
                .ToList();
            ViewBag.ProjectId = projectId;
            return View(tasks);
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
