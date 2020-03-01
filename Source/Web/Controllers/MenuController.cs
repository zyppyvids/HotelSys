using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Web.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Administrator_Menu()
        {
            if (GetCookie("LoggedIn") == "true")
            {
                if (GetCookie("Username") == "HotelSysAdmin")
                {
                    return View();
                }
                else
                {
                    return Redirect("/Menu/User_Menu");
                }
            }
            else
            {
                return Redirect("/");
            }
        }

        public IActionResult User_Menu()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                if (GetCookie("Username") == "HotelSysAdmin")
                {
                    return Redirect("/Menu/Administrator_Menu");
                }
                else
                {
                    return View();
                }
            }
        }

        public IActionResult List_Users()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        public IActionResult Create_Users()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                if(GetCookie("Username") == "HotelSysAdmin")
                {
                    return RedirectToAction("Create", "Users");
                }

                return Redirect("/Menu/User_Menu");
            }
        }

        public IActionResult Edit_Users()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                if (GetCookie("Username") == "HotelSysAdmin")
                {
                    return RedirectToAction("Edit", "Users");
                }

                return Redirect("/Menu/User_Menu");
            }
        }

        public IActionResult Delete_Users()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                if (GetCookie("Username") == "HotelSysAdmin")
                {
                    return RedirectToAction("Delete", "Users");
                }

                return Redirect("/Menu/User_Menu");
            }
        }

        public IActionResult List_Rooms()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                return RedirectToAction("Index", "Rooms");
            }
        }

        public IActionResult Create_Rooms()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                return RedirectToAction("Create", "Rooms");
            }
        }

        public IActionResult Edit_Rooms()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                return RedirectToAction("Edit", "Rooms");
            }
        }

        public IActionResult Delete_Rooms()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                return RedirectToAction("Delete", "Rooms");
            }
        }

        public IActionResult List_Clients()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                return RedirectToAction("Index", "Clients");
            }
        }

        public IActionResult Create_Clients()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                return RedirectToAction("Create", "Clients");
            }
        }

        public IActionResult Edit_Clients()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                return RedirectToAction("Edit", "Clients");
            }
        }

        public IActionResult Delete_Clients()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                return RedirectToAction("Delete", "Clients");
            }
        }

        public IActionResult List_Reservations()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                return RedirectToAction("Index", "Reservations");
            }
        }

        public IActionResult Create_Reservations()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                return RedirectToAction("Create", "Reservations");
            }
        }

        public IActionResult Edit_Reservations()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                return RedirectToAction("Edit", "Reservations");
            }
        }

        public IActionResult Delete_Reservations()
        {
            if (GetCookie("LoggedIn") != "true")
            {
                return Redirect("/");
            }
            else
            {
                return RedirectToAction("Delete", "Reservations");
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