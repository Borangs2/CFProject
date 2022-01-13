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
    public class LoginController : Controller
    {
        private readonly TaskContext _context;

        public LoginController(TaskContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public IActionResult Login()
        {
            var loginUsers = from u in _context.User
                             where u.Name.Trim().ToLower().Contains(Name)
                             where u.Password.Contains(Password)
                             select u;
            User user = loginUsers.ToList()[0];

            HttpContext.Session.SetInt32("UserId", user.UserId);
            return RedirectToPage("Index", "Home");
        }



        // GET: Login
        public async Task<IActionResult> Index()
        {
            var taskContext = _context.User.Include(u => u.Manager).Include(u => u.Role);
            return View(await taskContext.ToListAsync());
        }

        // GET: Login/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.Manager)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Login/Create
        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(_context.User, "UserId", "Name");
            ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "RoleName");
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Name,Password,ManagerId,RoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_context.User, "UserId", "Name", user.ManagerId);
            ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        // GET: Login/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["ManagerId"] = new SelectList(_context.User, "UserId", "Name", user.ManagerId);
            ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        // POST: Login/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Name,Password,ManagerId,RoleId")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_context.User, "UserId", "Name", user.ManagerId);
            ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        // GET: Login/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.Manager)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}
