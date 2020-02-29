using System;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Administrator_Menu()
        {
            return View();
        }

        public IActionResult User_Menu()
        {
            return View();
        }

        public IActionResult List_Users()
        {
            return RedirectToAction("Index", "Users");
        }

        public IActionResult Create_Users()
        {
            return RedirectToAction("Create", "Users");
        }

        public IActionResult Edit_Users()
        {
            return RedirectToAction("Edit", "Users");
        }

        public IActionResult Delete_Users()
        {
            return RedirectToAction("Delete", "Users");
        }

        public IActionResult List_Rooms()
        {
            return RedirectToAction("Index", "Rooms");
        }

        public IActionResult Create_Rooms()
        {
            return RedirectToAction("Create", "Rooms");
        }

        public IActionResult Edit_Rooms()
        {
            return RedirectToAction("Edit", "Rooms");
        }

        public IActionResult Delete_Rooms()
        {
            return RedirectToAction("Delete", "Rooms");
        }

        public IActionResult List_Clients()
        {
            return RedirectToAction("Index", "Clients");
        }

        public IActionResult Create_Clients()
        {
            return RedirectToAction("Create", "Clients");
        }

        public IActionResult Edit_Clients()
        {
            return RedirectToAction("Edit", "Clients");
        }

        public IActionResult Delete_Clients()
        {
            return RedirectToAction("Delete", "Clients");
        }

        public IActionResult List_Reservations()
        {
            return RedirectToAction("Index", "Reservations");
        }

        public IActionResult Create_Reservations()
        {
            return RedirectToAction("Create", "Reservations");
        }

        public IActionResult Edit_Reservations()
        {
            return RedirectToAction("Edit", "Reservations");
        }

        public IActionResult Delete_Reservations()
        {
            return RedirectToAction("Delete", "Reservations");
        }
    }
}