using Data;
using Data.Entity;
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
            model.Pager ??= new PagerViewModel();

            StringValues page = StringValues.Empty;
            Request.Query.TryGetValue("page", out page);
            model.Pager.CurrentPage = StringValues.IsNullOrEmpty(page) ? 1 : int.Parse(page);

            StringValues pagesize = StringValues.Empty;
            Request.Query.TryGetValue("pagesize", out pagesize);
            model.Pager.PageSize = StringValues.IsNullOrEmpty(pagesize) ? 10 : int.Parse(pagesize);

            model.Pager.PagesCount = (int)Math.Ceiling((double)_context.Clients.ToArray().Length / (double)model.Pager.PageSize);

            List<ClientsViewModel> items = await _context.Clients.Skip((model.Pager.CurrentPage - 1) * model.Pager.PageSize).Take(model.Pager.PageSize).Select(c => new ClientsViewModel()
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Adult = c.Adult
            }).ToListAsync();

            model.Items = items;

            return View(model);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            ClientsCreateViewModel model = new ClientsCreateViewModel();

            return View(model);
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientsCreateViewModel model)
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

        // GET: Clients/Edit/id
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientsEditViewModel model)
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

        // GET: Clients/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            Client client = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id != id);
        }
    }
}
