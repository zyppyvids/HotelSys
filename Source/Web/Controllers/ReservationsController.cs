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
using Microsoft.Extensions.Primitives;

namespace Web.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly MyHotelDb _context;

        public ReservationsController()
        {
            _context = new MyHotelDb();
        }

        // GET: Reservations
        public async Task<IActionResult> Index(ReservationsIndexViewModel model)
        {
            model.Pager ??= new PagerViewModel();

            StringValues page = StringValues.Empty;
            Request.Query.TryGetValue("page", out page);
            model.Pager.CurrentPage = StringValues.IsNullOrEmpty(page) ? 1 : int.Parse(page);

            StringValues pagesize = StringValues.Empty;
            Request.Query.TryGetValue("pagesize", out pagesize);
            model.Pager.PageSize = StringValues.IsNullOrEmpty(pagesize) ? 10 : int.Parse(pagesize);

            model.Pager.PagesCount = (int)Math.Ceiling((double)_context.Reservations.ToArray().Length / (double)model.Pager.PageSize);

            List<ReservationsViewModel> items = new List<ReservationsViewModel>();

            foreach(Reservation r in _context.Reservations.ToArray().Skip((model.Pager.CurrentPage - 1) * model.Pager.PageSize).Take(model.Pager.PageSize))
            {
                ReservationsViewModel item = new ReservationsViewModel();
                item.Id = r.Id;
                item.RoomNumber = r.RoomNumber;
                User user = _context.Users.ToArray().Where(u => u.Id == r.UserId).FirstOrDefault();
                item.UserName = user != null ? user.FirstName + " " + user.LastName : "Unknown";

                List<string> clientsNames = new List<string>();
                foreach(int i in r.ClientsIds.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c)))
                {
                    Client client = _context.Clients.ToArray().Where(c => c.Id == i).FirstOrDefault();

                    if (client != null)
                    {
                        clientsNames.Add(client.FirstName + " " + client.LastName);
                    }
                }

                item.ClientsNames = clientsNames.ToArray();
                item.DateAccomodation = r.DateAccomodation.ToShortDateString();
                item.DateRelease = r.DateRelease.ToShortDateString();
                item.BreakfastIncluded = r.BreakfastIncluded;
                item.AllInclusive = r.AllInclusive;
                item.PaymentAmount = r.PaymentAmount;

                items.Add(item);
            }

            model.Items = items;

            return View(model);
        }

        private ReservationsCreateViewModel GenerateReservationCraeteViewModel(ReservationsCreateViewModel model = null)
        {
            if(model == null)
            {
                model = new ReservationsCreateViewModel();
            }
            model.AllAvailableRooms = _context.Rooms.ToArray().Select(r => r.Number).ToArray();

            model.AllUsersNames = _context.Users.ToArray().Select(user => user.FirstName + " " + user.LastName).ToArray();
            model.AllUsersIds = _context.Users.ToArray().Select(user => user.Id).ToArray();

            model.AllClientsNames = _context.Clients.ToArray().Select(client => client.FirstName + " " + client.LastName).ToArray();
            model.AllClientsIds = _context.Clients.ToArray().Select(client => client.Id).ToArray();

            return model;
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            return View(GenerateReservationCraeteViewModel());
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
                    ClientsIds = string.Join(",", model.ClientsIds),
                    DateAccomodation = model.DateAccomodation,
                    DateRelease = model.DateRelease,
                    BreakfastIncluded = model.BreakfastIncluded,
                    AllInclusive = model.AllInclusive,
                    PaymentAmount = model.PaymentAmount
                };

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(GenerateReservationCraeteViewModel(model));
        }

        private ReservationsEditViewModel GenerateReservationEditViewModel(ReservationsEditViewModel model = null)
        {
            if (model == null)
            {
                model = new ReservationsEditViewModel();
            }
            model.AllAvailableRooms = _context.Rooms.ToArray().Select(r => r.Number).ToArray();

            model.AllUsersNames = _context.Users.ToArray().Select(user => user.FirstName + " " + user.LastName).ToArray();
            model.AllUsersIds = _context.Users.ToArray().Select(user => user.Id).ToArray();

            model.AllClientsNames = _context.Clients.ToArray().Select(client => client.FirstName + " " + client.LastName).ToArray();
            model.AllClientsIds = _context.Clients.ToArray().Select(client => client.Id).ToArray();

            return model;
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

            ReservationsEditViewModel model = new ReservationsEditViewModel();
            model.RoomNumber = reservation.RoomNumber;
            model.UserId = reservation.UserId;
            model.ClientsIds = reservation.ClientsIds.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToArray();
            model.DateAccomodation = reservation.DateAccomodation;
            model.DateRelease = reservation.DateRelease;
            model.BreakfastIncluded = reservation.BreakfastIncluded;
            model.AllInclusive = reservation.AllInclusive;
            model.PaymentAmount = reservation.PaymentAmount;

            return View(GenerateReservationEditViewModel(model));
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
                    ClientsIds = string.Join(",", model.ClientsIds),
                    DateAccomodation = model.DateAccomodation,
                    DateRelease = model.DateRelease,
                    BreakfastIncluded = model.BreakfastIncluded,
                    AllInclusive = model.AllInclusive,
                    PaymentAmount = model.PaymentAmount
                };

                try
                {
                    _context.Reservations.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(GenerateReservationEditViewModel(model));
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
