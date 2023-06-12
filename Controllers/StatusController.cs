using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using HouseMangment.Entity;

namespace HouseMangment.Controllers
{
    public class StatusController : Controller
    {
        private WhereHouseEntities db = new WhereHouseEntities();

        // GET: Status
        public ActionResult Index()
        {
            var status = db.Status.Where(x => x.isActive == true).Include(s => s.Devices).Include(s => s.Users);
            return View(status.ToList());
        }

        public ActionResult HestoryAssign()
        {
            var status = db.Status.Where(x => x.isActive == false).Include(s => s.Devices).Include(s => s.Users);
            return View(status.ToList());
        }
        
        // GET: Status/Create
        public ActionResult Create()
        {

           // var query = db.Devices
           //.Join(db.Status, d => d.Id, s => s.Device_id, (d, s) => new { dis = s.Device_id, di = d.Id, d.Name, d.SerialNumber })
           //.Where(di != dis);

           // var res = db.Devices.Join(db.Status,
           //      s => s.Id,
           //      c => c.Device_id,
           // (s, c) => new { s, c })
           //.Where(sc => sc.c.Device_id != c.id && sc.c.isActive == false)
           //.Select(sc => sc.s);



           // var statusd = db.Devices.Where(d => d.isActive != false).Join(da => da.Id, db.Status, d => d.Device_id).Where(da => da.Id == d => d.Device_id);
           // ViewBag.Device_id = new SelectList(db.Devices.Where(d => d.isActive != false).Join(
           // db.Status s => s.Id,



            ViewBag.Device_id = new SelectList(db.Devices.Where(d => d.isActive != false), "Id", "Name");
            ViewBag.User_id = new SelectList(db.Users.Where(d => d.isActive != false && d.isAdmin != true), "Id", "name");
            return View();
        }

        // POST: Status/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Device_id,User_id,DateStart,EndDate,isActive")] Status status)
        {
            if (ModelState.IsValid)
            {
                db.Status.Add(status);
                status.isActive = true;
                status.DateStart = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index" , TempData["SuccessMessage"] = true);
            }

            ViewBag.Device_id = new SelectList(db.Devices, "Id", "Name", status.Device_id);
            ViewBag.User_id = new SelectList(db.Users, "Id", "name", status.User_id);
            return View(status);
        }


        public ActionResult Unassing(int id)
        {
            
            var findid = db.Status.Find(id);
            findid.isActive = false;
            findid.EndDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index", TempData["SuccessMessage"] = true);
        }

    }
}
