using Data;
using Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Rooms;
using Web.Models.Shared;

namespace Web.Controllers
{
    public class RoomsController : Controller
    {
        private readonly MyHotelDb _context;
        private const int PageSize = 10;

        public RoomsController()
        {
            _context = new MyHotelDb();
        }

        // GET: Rooms
        public async Task<IActionResult> Index(RoomsIndexViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            List<RoomsViewModel> items = await _context.Rooms.Skip((model.Pager.CurrentPage - 1) * PageSize).Take(PageSize).Select(r => new RoomsViewModel()
            {
                Number = r.Number,
                Type = r.Type,
                Capacity = r.Capacity,
                Free = r.Free,
                BedPriceAdult = r.BedPriceAdult,
                BedPriceChild = r.BedPriceChild
            }).ToListAsync();

            model.Items = items;
            model.Pager.PagesCount = (int)Math.Ceiling(await _context.Rooms.CountAsync() / (double)PageSize);

            return View(model);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            RoomsCreateViewModel model = new RoomsCreateViewModel();

            return View(model);
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomsCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Room room = new Room()
                {
                    Number = model.Number,
                    Type = model.Type,
                    Capacity = model.Capacity,
                    Free = model.Free,
                    BedPriceAdult = model.BedPriceAdult,
                    BedPriceChild = model.BedPriceChild
                };

                _context.Add(room);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Rooms/Edit/id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Room room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            RoomsEditViewModel model = new RoomsEditViewModel
            {
                Number = room.Number,
                Type = room.Type,
                Capacity = room.Capacity,
                Free = room.Free,
                BedPriceAdult = room.BedPriceAdult,
                BedPriceChild = room.BedPriceChild
            };

            return View(model);
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoomsEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Room room = new Room
                {
                    Number = model.Number,
                    Type = model.Type,
                    Capacity = model.Capacity,
                    Free = model.Free,
                    BedPriceAdult = model.BedPriceAdult,
                    BedPriceChild = model.BedPriceChild
                };

                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Rooms/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            Room room = await _context.Rooms.FindAsync(id);
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
