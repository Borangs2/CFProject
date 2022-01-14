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
            try
            {
                var loginUsers = from u in _context.User
                                 where u.Name.Trim().ToLower().Contains(Name)
                                 where u.Password.Contains(Password)
                                 select u;

                User user = loginUsers.ToList()[0];

                HttpContext.Session.SetInt32("UserRoleId", user.Role.RoleId);

                HttpContext.Session.SetInt32("UserId", user.UserId);
                TempData["Login"] = "Success";
                return RedirectToPage("Index", "Home");
            }
            catch
            {
                TempData["Login"] = "NotFound";
                return View("Create");
            }

        }



        // GET: Login


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

    }
}
