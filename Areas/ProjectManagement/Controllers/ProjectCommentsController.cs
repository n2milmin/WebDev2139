using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Lab2.Data;
using Lab2.Areas.ProjectManagement.Models;

namespace Lab2.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManageent")]
    [Route("[area]/[controller]/[action]")]
    public class ProjectCommentsController : Controller
    {
        private readonly AppDbContext _db;

        public ProjectCommentsController(AppDbContext db)
        {
            _db = db;
        }

        // GET: ProjectManagement/ProjectComment/GetComments/{projectId}
        [HttpGet]
        public async Task<IActionResult> GetComments(int projectId)
        {
            var comments = await _db.ProjectComments
                                         .Where(c => c.ProjectId == projectId)
                                         .OrderByDescending(c => c.CreatedDate)
                                         .ToListAsync();

            return Json(comments);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] ProjectComment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CreatedDate = DateTime.Now; // Set the current time as the posting time
                _db.ProjectComments.Add(comment);
                await _db.SaveChangesAsync();
                return Json(new { success = true, message = "Comment added successfully." });
            }

            // Log ModelState errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Invalid comment data.", errors });
        }
    }
}
