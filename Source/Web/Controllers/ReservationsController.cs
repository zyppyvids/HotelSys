using Data;
using Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Reservations;
using Web.Models.Shared;

namespace Web.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly MyHotelDb _context;
        private const int PageSize = 10;

        public ReservationsController()
        {
            _context = new MyHotelDb();
        }

        // GET: Reservations
        public async Task<IActionResult> Index(ReservationsIndexViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            List<ReservationsViewModel> items = await _context.Reservations.Skip((model.Pager.CurrentPage - 1) * PageSize).Take(PageSize).Select(r => new ReservationsViewModel()
            {
                RoomNumber = r.RoomNumber,
                UserId = r.UserId,
                ClientsIds = r.ClientsIds.Split(", ", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray(),
                DateAccomodation = r.DateAccomodation,
                DateRelease = r.DateRelease,
                BreakfastIncluded = r.BreakfastIncluded,
                AllInclusive = r.AllInclusive,
                PaymentAmount = r.PaymentAmount
            }).ToListAsync();

            model.Items = items;
            model.Pager.PagesCount = (int)Math.Ceiling(await _context.Reservations.CountAsync() / (double)PageSize);

            return View(model);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ReservationsCreateViewModel model = new ReservationsCreateViewModel();

            return View(model);
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationsCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Reservation reservation = new Reservation
                {
                    RoomNumber = model.RoomNumber,
                    UserId = model.UserId,
                    ClientsIds = string.Join(", ", model.ClientsIds),
                    DateAccomodation = model.DateAccomodation,
                    DateRelease = model.DateRelease,
                    BreakfastIncluded = model.BreakfastIncluded,
                    AllInclusive = model.AllInclusive,
                    PaymentAmount = model.PaymentAmount
                };

                _context.Add(reservation);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Reservations/Edit/id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reservation reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            ReservationsEditViewModel model = new ReservationsEditViewModel
            {
                RoomNumber = reservation.RoomNumber,
                UserId = reservation.UserId,
                ClientsIds = reservation.ClientsIds.Split(", ", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray(),
                DateAccomodation = reservation.DateAccomodation,
                DateRelease = reservation.DateRelease,
                BreakfastIncluded = reservation.BreakfastIncluded,
                AllInclusive = reservation.AllInclusive,
                PaymentAmount = reservation.PaymentAmount
            };

            return View(model);
        }

        // POST: Reservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ReservationsEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Reservation reservation = new Reservation
                {
                    RoomNumber = model.RoomNumber,
                    UserId = model.UserId,
                    ClientsIds = string.Join(", ", model.ClientsIds),
                    DateAccomodation = model.DateAccomodation,
                    DateRelease = model.DateRelease,
                    BreakfastIncluded = model.BreakfastIncluded,
                    AllInclusive = model.AllInclusive,
                    PaymentAmount = model.PaymentAmount
                };

                try
                {
                    _context.Update(reservation);
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

        // GET: Reservations/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            Reservation reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
