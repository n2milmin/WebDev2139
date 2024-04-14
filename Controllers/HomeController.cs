using Lab2.Models;
using Lab2.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lab2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISessionService _sessionService;

        public HomeController(ILogger<HomeController> logger, ISessionService session)
        {
            _logger = logger;
            _sessionService = session;
        }

        public IActionResult Index()
        {
            const string sessionKey = "VisitCount";
            int visitCount = _sessionService.GetSessionData<int?>(sessionKey) ?? 0;
            _sessionService.SetSessionData(sessionKey, visitCount + 1);

            ViewBag.visitCount = visitCount;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GeneralSearch(string searchType, string searchString)
        {
            if (searchType == "Projects")
            {
                return RedirectToAction("Search", "Projects", new { searchString });
            }
            else if (searchType == "Tasks")
            {
                int defaultProjectId = 1;
                return RedirectToAction("Search", "Task", new { projectId = defaultProjectId, searchString });
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult NotFound(int statusCode)
        {
            if (statusCode == 404)
                return View("NotFound");
            return View("Error");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
