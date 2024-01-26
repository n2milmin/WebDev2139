using Lab2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Controllers
{
	public class ProjectsController : Controller
	{
        [HttpGet]
		public IActionResult Index()
		{
            var projects = new List<Project>()
            { new Project { ProjectId = 1, Name = "Project", Description = "First Project" } };
			return View(projects);
		}

        [HttpGet]
        public IActionResult Details(int id)
        {
            var projects = new List<Project>()
            { new Project { ProjectId = id, Name = "Project" + id, Description = "Details of the project" + id } };
            return View(projects);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Project project) 
        {
            return RedirectToAction("Index");
        }
    }
}
