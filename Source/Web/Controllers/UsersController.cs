using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Data.Entity;
using Web.Models.Users;
using Web.Models.Shared;
using System.Security.Cryptography;
using System.Text;

namespace Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly MyHotelDb _context;
        private const int PageSize = 10;

        public UsersController()
        {
            _context = new MyHotelDb();
        }

        // GET: Users
        public async Task<IActionResult> Index(UsersIndexViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            List<UsersViewModel> items = await _context.Users.Skip((model.Pager.CurrentPage - 1) * PageSize).Take(PageSize).Select(u => new UsersViewModel()
            {
                Id = u.Id,
                Username = u.Username,
                FirstName = u.FirstName,
                MiddleName = u.MiddleName,
                LastName = u.LastName,
                PersonalID = u.PersonalID,
                PhoneNumber = u.PhoneNumber,
                DateAppointment = u.DateAppointment,
                DateDismissal = u.DateDismissal,
                Email = u.Email
            }).ToListAsync();

            model.Items = items;
            model.Pager.PagesCount = (int)Math.Ceiling(await _context.Users.CountAsync() / (double)PageSize);

            return View(model);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            UsersCreateViewModel model = new UsersCreateViewModel();

            return View(model);
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsersCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Username = model.Username,
                    PasswordHash = GetPasswordHash(model.Password),
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    PersonalID = model.PersonalID,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    DateAppointment = model.DateAppointment,
                    Active = model.Active,
                    DateDismissal = model.DateDismissal
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Users/Edit/id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            UsersEditViewModel model = new UsersEditViewModel
            {
                Username = user.Username,
                Password = GetPasswordHash(user.PasswordHash),
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                PersonalID = user.PersonalID,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                DateAppointment = user.DateAppointment,
                Active = user.Active,
                DateDismissal = user.DateDismissal
            };

            return View(model);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsersEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FindAsync(model.Id);
                user.Username = model.Username;
                user.PasswordHash = GetPasswordHash(model.Password);
                user.FirstName = model.FirstName;
                user.MiddleName = model.MiddleName;
                user.LastName = model.LastName;
                user.PersonalID = model.PersonalID;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
                user.DateAppointment = model.DateAppointment;
                user.Active = model.Active;
                user.DateDismissal = model.DateDismissal;

                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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

            return View(model);
        }

        // GET: Users/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            User user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id != id);
        }

        private string GetPasswordHash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
