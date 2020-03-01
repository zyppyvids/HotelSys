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
using Web.Models.Rooms;
using Web.Models.Shared;

namespace Web.Controllers
{
    public class RoomsController : Controller
    {
        private readonly MyHotelDb _context;

        public RoomsController()
        {
            _context = new MyHotelDb();
        }

        // GET: Rooms
        public async Task<IActionResult> Index(RoomsIndexViewModel model)
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

                model.Pager.PagesCount = (int)Math.Ceiling((double)_context.Rooms.ToArray().Length / (double)model.Pager.PageSize);

                List<RoomsViewModel> items = await _context.Rooms.Skip((model.Pager.CurrentPage - 1) * model.Pager.PageSize).Take(model.Pager.PageSize).Select(r => new RoomsViewModel()
                {
                    Number = r.Number,
                    Type = r.Type,
                    Capacity = r.Capacity,
                    Free = r.Free,
                    BedPriceAdult = r.BedPriceAdult,
                    BedPriceChild = r.BedPriceChild
                }).ToListAsync();

                model.Items = items;

                return View(model);
            }
        }

        // GET: Rooms/Edit/id
        public async Task<IActionResult> Edit(int? roomnumber)
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                if (roomnumber == null)
                {
                    return NotFound();
                }

                Room room = await _context.Rooms.FindAsync(roomnumber);
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
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoomsEditViewModel model)
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
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
        }

        // GET: Rooms/Delete/id
        public async Task<IActionResult> Delete(int roomnumber)
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                Room room = await _context.Rooms.FindAsync(roomnumber);
                _context.Rooms.Remove(room);
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
