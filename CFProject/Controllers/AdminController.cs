﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CFProject.Models;

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

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTask = await _context.UserTask
                .Include(u => u.Project)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userTask == null)
            {
                return NotFound();
            }

            return View(userTask);
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
        [BindProperty]
        public int TaskId { get; set; }
        [BindProperty]
        public int UserId { get; set; }
        public async Task<IActionResult> Delete()
        {
            var userTask = await _context.UserTask
                .Include(u => u.Project)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.TaskId == TaskId && m.UserId == UserId);
            if (userTask == null)
            {
                return NotFound();
            }

            return View(userTask);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        {
            var userTask = await _context.UserTask.FindAsync(TaskId, UserId);
            _context.UserTask.Remove(userTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserTaskExists(int id)
        {
            return _context.UserTask.Any(e => e.UserId == id);
        }
    }
}
