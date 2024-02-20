using Lab2.Data;
using Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Controllers
{
    [Route("Projects")]
	public class ProjectsController : Controller
	{
        private readonly AppDbContext _db;
        public ProjectsController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]
		public IActionResult Index()
		{
			return View(_db.Projects.ToList());
		}

        [HttpGet("Index/{projectId:int}")]
        public IActionResult Details(int id)
        {
            var project = _db.Projects.FirstOrDefault(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpGet("Create")]
        public IActionResult Create() 
        {
            
            return View();
        }
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Project project) 
        {
			if (ModelState.IsValid)
			{
				_db.Projects.Add(project);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
            return View(project);
        }

		[HttpGet("Edit/{id:int}")]
		public IActionResult Edit(int id)
		{
            var project = _db.Projects.Find(id);
			if (project == null)
			{
				return NotFound();
			}
			return View(project);
		}
		[HttpPost("Edit/{id:int}")]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(int id, [Bind("ProjectId, Name, Description, StartDate, EndDate, Status")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(project);
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException) 
                {
                    if (!ProjectExists(project.ProjectId))
                    {
                        return NotFound(project);
                    }
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

		private bool ProjectExists(int id)
		{
            return _db.Projects.Any(p => p.ProjectId == id);
		}
        [HttpGet("Delete/{id:int}")]
		public IActionResult Delete(int id)
		{
			var project = _db.Projects.FirstOrDefault(p => p.ProjectId == id);
			if (project == null)
			{
				return NotFound();
			}
			return View(project);
		}

        [HttpPost("DeleteConfirmed/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int ProjectId)
        {
            var project = _db.Projects.Find(ProjectId);
            if (project != null)
            {
                _db.Projects.Remove(project);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
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
