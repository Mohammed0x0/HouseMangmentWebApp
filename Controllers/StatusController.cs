using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using HouseMangment.Entity;
using Rotativa;


namespace HouseMangment.Controllers

{
    public class StatusController : Controller

    {
        // Use proper access modifiers for class-level variables
        public bool hisadmin = UsersController.hisadmin;
        public int myid = UsersController.myid;
        public bool logged = UsersController.logged;

        private WhereHouseEntities db = new WhereHouseEntities();
        public ActionResult Index()
        {
            // Check if the user is an admin
            if (hisadmin == true)
            {
                // Retrieve a list of active statuses and include related device and user information
                var status = db.Status.Where(x => x.isActive == true).Include(s => s.Devices).Include(s => s.Users);
                return View(status.ToList());
            }

            else
            {
                return RedirectToAction("login" ,"users");
            }
        }

        public ActionResult HestoryAssign()
        {
            // Check if the user is an admin
            if (hisadmin == true)
            {
                var status = db.Status.Where(x => x.isActive == false).Include(s => s.Devices).Include(s => s.Users);
                return View(status.ToList());
            }

            else
            {
                return RedirectToAction("login", "users");
            }
        }

        // GET: Status/Create
        public ActionResult Create()
        {
            // Check if the user is an admin
            if (hisadmin == true)
            {
                ViewBag.Device_id = new SelectList(db.Devices.Where(a => a.isActive == true && !db.Status.Where(b => b.isActive == true).Select(b => b.Device_id).Contains(a.Id)), "Id", "numser");
                ViewBag.User_id = new SelectList(db.Users.Where(d => d.isActive == true && d.isAdmin != true && d.IsDeleted != true), "Id", "numuser");
                return View();
            }

            else
            {
                return RedirectToAction("login", "users");
            }
        }


        // POST: Status/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Status status)
        {
            // Check if the user is an admin
            if (hisadmin == true)
            {
                if (ModelState.IsValid)
                {
                    db.Status.Add(status);
                    status.isActive = true;
                    status.DateStart = DateTime.Now;
                    db.SaveChanges();
                    return RedirectToAction("Index", TempData["SuccessMessage"] = true);
                }

                ViewBag.Device_id = new SelectList(db.Devices.Where(a => a.isActive == true && !db.Status.Where(b => b.isActive == true).Select(b => b.Device_id).Contains(a.Id)), "Id", "numser");
                ViewBag.User_id = new SelectList(db.Users.Where(d => d.isActive == true && d.isAdmin == false && d.IsDeleted != true), "Id", "numuser");
                return View(status);
            }

            else
            {
                return RedirectToAction("login", "users");
            }
        }


        public ActionResult Unassing(int id)
        {
            // Check if the user is an admin
            if (hisadmin == true)
            {
                var findid = db.Status.Find(id);
                findid.isActive = false;
                findid.EndDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index", TempData["SuccessMessage"] = true);

            }

            else
            {
                return RedirectToAction("login", "users");
            }

        }


        public ActionResult myAssign()
        {
            // Check if the user is login
            if (logged == true)
            {
                var status = db.Status.Where(x => x.isActive == true && x.User_id == myid).Include(s => s.Devices).Include(s => s.Users);
                return View(status.ToList());
            }

            else
            {
                return RedirectToAction("users" ,"login");
            }
        }

        public ActionResult myoldassign()
        {
            // Check if the user is login
            if (logged == true)
            {
                var status = db.Status.Where(x => x.isActive == false && x.User_id == myid).Include(s => s.Devices).Include(s => s.Users);
                return View(status.ToList());
            }

            else
            {
                return RedirectToAction("login", "users");
            }
        }

    }
}
