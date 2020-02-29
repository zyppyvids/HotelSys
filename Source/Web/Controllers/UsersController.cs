using Data;
using Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Web.Models.Shared;
using Web.Models.Users;

namespace Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly MyHotelDb _context;

        public UsersController()
        {
            _context = new MyHotelDb();
        }

        // GET: Users
        public async Task<IActionResult> Index(UsersIndexViewModel model)
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                model.Pager ??= new PagerViewModel();

                StringValues value = StringValues.Empty;
                Request.Query.TryGetValue("page", out value);
                model.Pager.CurrentPage = StringValues.IsNullOrEmpty(value) ? 1 : int.Parse(value);

                value = StringValues.Empty;
                Request.Query.TryGetValue("pagesize", out value);
                model.Pager.PageSize = StringValues.IsNullOrEmpty(value) ? 10 : int.Parse(value);

                model.Pager.PagesCount = (int)Math.Ceiling((double)_context.Users.ToArray().Length / (double)model.Pager.PageSize);

                value = StringValues.Empty;
                Request.Query.TryGetValue("sort", out value);
                model.CurrentSort = StringValues.IsNullOrEmpty(value) ? 0 : int.Parse(value);

                List<UsersViewModel> items = await _context.Users.Skip((model.Pager.CurrentPage - 1) * model.Pager.PageSize).Take(model.Pager.PageSize).Select(u => new UsersViewModel()
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
                    Active = u.Active,
                    Email = u.Email
                }).ToListAsync();

                switch (model.CurrentSort)
                {
                    case 1: //FirstName
                        model.Items = items.OrderBy(i => i.FirstName).ToList();
                        break;

                    case 2: //MiddleName
                        model.Items = items.OrderBy(i => i.MiddleName).ToList();
                        break;

                    case 3: //FamilyName
                        model.Items = items.OrderBy(i => i.LastName).ToList();
                        break;

                    case 4: //Email
                        model.Items = items.OrderBy(i => i.Email).ToList();
                        break;

                    default: //Usernames
                        model.Items = items.OrderBy(i => i.Username).ToList();
                        break;
                }

                return View(model);
            }
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                UsersCreateViewModel model = new UsersCreateViewModel();

                return View(model);
            }
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsersCreateViewModel model)
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
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
        }

        // GET: Users/Edit/id
        public async Task<IActionResult> Edit(int? id)
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
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
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsersEditViewModel model)
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    User user = await _context.Users.FindAsync(model.Id);
                    user.Username = model.Username;

                    if (model.Password != null)
                    {
                        user.PasswordHash = GetPasswordHash(model.Password);
                    }
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
        }

        // GET: Users/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                User user = await _context.Users.FindAsync(id);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
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

        private string GetCookie(string key)
        {
            try
            {
                return Request.Cookies[key];
            }
            catch (KeyNotFoundException)
            {
                return "false";
            }
        }

        private void SetCookie(string key, string value, int? expireTime = null)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
            {
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);

                Response.Cookies.Append(key, value, option);
            }
            else
            {
                Response.Cookies.Append(key, value);
            }
        }

        private void RemoveCookie(string key)
        {
            Response.Cookies.Delete(key);
        }

        public IActionResult Back_To_Menu()
        {
            //if (administator)
            //{
            //return RedirectToAction("Administrator_Menu", "Menu"); 
            //}
            //else
            //{
            //return RedirectToAction("User_Menu", "Menu");
            //}
            return RedirectToAction("Administrator_Menu", "Menu");
        }
    }
}
