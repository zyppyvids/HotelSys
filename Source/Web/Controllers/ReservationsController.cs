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
using Microsoft.AspNetCore.Http;

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

                model.Pager.PagesCount = (int)Math.Ceiling((double)_context.Reservations.ToArray().Length / (double)model.Pager.PageSize);

                value = StringValues.Empty;
                Request.Query.TryGetValue("clientid", out value);

                List<ReservationsViewModel> items = new List<ReservationsViewModel>();

                foreach (Reservation r in _context.Reservations.ToArray().Skip((model.Pager.CurrentPage - 1) * model.Pager.PageSize).Take(model.Pager.PageSize))
                {
                    ReservationsViewModel item = new ReservationsViewModel();
                    item.Id = r.Id;
                    item.RoomNumber = r.RoomNumber;
                    User user = _context.Users.ToArray().Where(u => u.Id == r.UserId).FirstOrDefault();
                    item.UserName = user != null ? user.FirstName + " " + user.LastName : "Unknown";

                    List<string> clientsNames = new List<string>();
                    int[] clientIds = r.ClientsIds.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c)).ToArray();

                    if (!StringValues.IsNullOrEmpty(value))
                    {
                        if (!clientIds.Contains(int.Parse(value)))
                        {
                            continue;
                        }
                    }

                    foreach (int i in clientIds)
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
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                return View(GenerateReservationCraeteViewModel());
            }
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationsCreateViewModel model)
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
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
        }

        // POST: Reservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ReservationsEditViewModel model)
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
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
        }

        // GET: Reservations/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                Reservation reservation = await _context.Reservations.FindAsync(id);
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
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
    }
}
