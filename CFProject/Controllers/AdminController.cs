using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CFProject.Models;
using Microsoft.AspNetCore.Http;

namespace CFProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly TaskContext _context;

        public AdminController(TaskContext context)
        {
            _context = context;
        }

        // GET: Admin
        public IActionResult Index()
        {
            var taskContext = _context.UserTask.Include(u => u.Project).Include(u => u.User).Include(u => u.Project.Company).ToList();
            return View(taskContext);
        }

        public IActionResult Search()
        {
            string searchTerm = Request.Form["SearchTerm"];
            var searchResults = from p in _context.UserTask.Include(b => b.Project).Include(p => p.User) 
                                where p.Project.Title.Contains(searchTerm) 
                                select p;
            return View("Index", searchResults.ToList());
        }

        
        // GET: Admin/Create
        public IActionResult Create()
        {
            ViewData["TaskId"] = new SelectList(_context.Project, "TaskId", "Title");
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Name");
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,TaskId")] UserTask userTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TaskId"] = new SelectList(_context.Project, "TaskId", "Title", userTask.TaskId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Name", userTask.UserId);
            return View(userTask);
        }



        // GET: Admin/Delete/5
       
        public IActionResult GoToDelete()
        {
            int userId = Convert.ToInt32(Request.Form["UserId"]);
            int taskId = Convert.ToInt32(Request.Form["TaskId"]);

            var userTask = _context.UserTask
                .Include(u => u.Project)
                .Include(u => u.User)
                .Where(u => u.TaskId == taskId);

            if (userTask == null)
            {
                return NotFound();
            }

            return View("Delete", userTask.ToList());
        }


        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        {
            int userId = Convert.ToInt32(Request.Form["UserId"]);
            int taskId = Convert.ToInt32(Request.Form["TaskId"]);
            var userTask = await _context.UserTask.FindAsync(taskId, userId);
            _context.UserTask.Remove(userTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromTask()
        {
            int userId = Convert.ToInt32(Request.Form["UserId"]);
            int taskId = Convert.ToInt32(Request.Form["TaskId"]);

            var connection = _context.UserTask.Find(userId, taskId);
            if(connection == null)
            {
                TempData["RemoveConnection"] = "NotFound";
                return View("Delete");
            }

            _context.UserTask.Remove(connection);
            User user = _context.User.Find(userId);
            TempData["Username"] = user.Name;
            _context.SaveChanges();
            TempData["RemoveConnection"] = "Success";
            return RedirectToAction("GoToDelete");
        }

        private bool UserTaskExists(int id)
        {
            return _context.UserTask.Any(e => e.UserId == id);
        }
    }
}
