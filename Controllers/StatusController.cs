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

namespace HouseMangment.Controllers

{
    public class StatusController : Controller

    {
        public bool hisadmin = UsersController.hisadmin;
        private WhereHouseEntities db = new WhereHouseEntities();
        public ActionResult Index()
        {
            if (hisadmin == true)
            {
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

            // var query = db.Devices
            //.Join(db.Status, d => d.Id, s => s.Device_id, (d, s) => new { dis = s.Device_id, di = d.Id, d.Name, d.SerialNumber })
            //.Where(di != dis);
            //var query1 = db.Devices;
            //var query2 = db.Status.ToList();

            //query1.Join(query1.is);

            //var query3 =
            //            Join query1 in db.Status.ToList()
            //            on Devices.Id != status..;

            //var q =
            //     from b in db.Devices
            //     join bm in db.Status on b.Id equals bm.Device_id
            //     where bm.isActive == true
            //     select b;

            // var res = db.Devices.Join(db.Status,
            //      s => s.Id,
            //      c => c.Device_id,
            // (s, c) => new { s, c })
            //.Where(sc => sc.c.Device_id != s.id && sc.c.isActive == false)
            //.Select(sc => sc.s);



            //var statusd = db.Devices.Where(d => d.isActive != false).Join(da => da.Id, db.Status, d => d.Device_id).Where(da => da.Id == d => d.Device_id);
            //ViewBag.Device_id = new SelectList(db.Devices.Where(d => d.isActive != false).Join(
            //db.Status s => s.Id,


            //var filteredEmployees = db.Devices.Where(a => a.isActive == true && !db.Status.Where(b => b.isActive == true).Select(b => b.Device_id).Contains(a.Id)).ToList();


            if (hisadmin == true)
            {
                ViewBag.Device_id = new SelectList(db.Devices.Where(a => a.isActive == true && !db.Status.Where(b => b.isActive == true).Select(b => b.Device_id).Contains(a.Id)), "Id", "numser");
                ViewBag.User_id = new SelectList(db.Users.Where(d => d.isActive == true && d.isAdmin == false && d.IsDeleted != true), "Id", "numuser");
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
            if (UsersController.logged == true)
            {
                var status = db.Status.Where(x => x.isActive == true && x.User_id == UsersController.myid).Include(s => s.Devices).Include(s => s.Users);
                return View(status.ToList());
            }

            else
            {
                return RedirectToAction("users" ,"login");
            }
        }

        public ActionResult myoldassign()
        {
            if (UsersController.logged == true)
            {
                var status = db.Status.Where(x => x.isActive == false && x.User_id == UsersController.myid).Include(s => s.Devices).Include(s => s.Users);
                return View(status.ToList());
            }

            else
            {
                return RedirectToAction("login", "users");
            }
        }


    }
}
