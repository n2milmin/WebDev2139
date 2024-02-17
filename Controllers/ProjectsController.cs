using Lab2.Data;
using Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Controllers
{
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

        [HttpGet]
        public IActionResult Details(int id)
        {
            var project = _db.Projects.FirstOrDefault(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            
            return View();
        }
        [HttpPost]
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

		[HttpGet]
		public IActionResult Edit(int id)
		{
            var project = _db.Projects.Find(id);
			if (project == null)
			{
				return NotFound();
			}
			return View(project);
		}
		[HttpPost]
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

		public IActionResult Delete(int id)
		{
			var project = _db.Projects.FirstOrDefault(p => p.ProjectId == id);
			if (project == null)
			{
				return NotFound();
			}
			return View(project);
		}

        [HttpPost, ActionName("DeleteConfirmed")]
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

       
	}
}
