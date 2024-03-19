using Microsoft.AspNetCore.Mvc;
using Lab2.Areas.ProjectManagement.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Lab2.Data;

namespace Lab2.Areas.ProjectManagement.Components.ProjectSummary
{
    public class ProjectSummaryViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        public ProjectSummaryViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int projectId)
        {
            var project = await _db.Projects
                .Include(x => x.Tasks)
                .SingleOrDefaultAsync(x => x.ProjectId == projectId);

            if (project == null)
            {
                return Content("Project not found.");
            }

            return View(project);
        }
    }
}
