﻿using BusMeApp.Managers;
using BusMeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusMeApp.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        // GET: Reservations
        private DbManager db = new DbManager();
        public ActionResult Index()
        {
            if(User.IsInRole("Administrator"))
            {
                var reservations = db.GetReservations();
                return View(reservations);
            }
            else
            {
                string name = User.Identity.Name;
                var reservations = db.GetReservations(name);
                return View(reservations);
            }  
        }

        public ActionResult Create()
        {
            ViewBag.PassengerId = new SelectList(db.GetPassengers(), "Id", "Id");
            ViewBag.BusRouteId = new SelectList(db.GetBusRoutes(), "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PassengerId = new SelectList(db.GetPassengers(), "Id", "Id");
                ViewBag.BusRouteId = new SelectList(db.GetBusRoutes(), "Id", "Id");
                return View(reservation);
            }
            if (db.AddReservation(reservation))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Content("No more available seats");
            }
        }

        public ActionResult Edit(int id)
        {
            Reservation reservation = db.GetReservation(id);
            ViewBag.PassengerId = new SelectList(db.GetPassengers(), "Id", "Id");
            ViewBag.BusRouteId = new SelectList(db.GetBusRoutes(), "Id", "Id");
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PassengerId = new SelectList(db.GetPassengers(), "Id", "Id");
                ViewBag.BusRouteId = new SelectList(db.GetBusRoutes(), "Id", "Id");
                return View(reservation);
            }
            db.UpdateReservation(reservation);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Reservation reservation = db.GetReservation(id);
            ViewBag.PassengerId = new SelectList(db.GetPassengers(), "Id", "Id");
            ViewBag.BusRouteId = new SelectList(db.GetBusRoutes(), "Id", "Id");
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            db.DeleteReservation(id);
            return RedirectToAction("Index");
        }
    }
}