using Data;
using Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Clients;
using Web.Models.Shared;

namespace Web.Controllers
{
    public class ClientsController : Controller
    {
        private readonly MyHotelDb _context;

        public ClientsController()
        {
            _context = new MyHotelDb();
        }

        // GET: Clients
        public async Task<IActionResult> Index(ClientsIndexViewModel model)
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

                model.Pager.PagesCount = (int)Math.Ceiling((double)_context.Clients.ToArray().Length / (double)model.Pager.PageSize);

                value = StringValues.Empty;
                Request.Query.TryGetValue("sort", out value);
                model.CurrentSort = StringValues.IsNullOrEmpty(value) ? 0 : int.Parse(value);

                List<ClientsViewModel> items = await _context.Clients.Skip((model.Pager.CurrentPage - 1) * model.Pager.PageSize).Take(model.Pager.PageSize).Select(c => new ClientsViewModel()
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email,
                    Adult = c.Adult
                }).ToListAsync();

                switch (model.CurrentSort)
                {
                    case 1: //FamilyName
                        model.Items = items.OrderBy(i => i.LastName).ToList();
                        break;

                    default://FirstName
                        model.Items = items.OrderBy(i => i.FirstName).ToList();
                        break;
                }

                return View(model);
            }
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                ClientsCreateViewModel model = new ClientsCreateViewModel();

                return View(model);
            }
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientsCreateViewModel model)
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    Client client = new Client
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        Adult = model.Adult
                    };

                    _context.Add(client);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                return View(model);
            }
        }

        // GET: Clients/Edit/id
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

                Client client = await _context.Clients.FindAsync(id);
                if (client == null)
                {
                    return NotFound();
                }

                ClientsEditViewModel model = new ClientsEditViewModel
                {
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    PhoneNumber = client.PhoneNumber,
                    Email = client.Email,
                    Adult = client.Adult
                };

                return View(model);
            }
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientsEditViewModel model)
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    Client client = await _context.Clients.FindAsync(model.Id);
                    client.FirstName = model.FirstName;
                    client.LastName = model.LastName;
                    client.PhoneNumber = model.PhoneNumber;
                    client.Email = model.Email;
                    client.Adult = model.Adult;

                    try
                    {
                        _context.Update(client);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ClientExists(client.Id))
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

        // GET: Clients/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                Client client = await _context.Clients.FindAsync(id);
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id != id);
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
    }
}
