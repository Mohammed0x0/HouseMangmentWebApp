using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HouseMangment.Entity;

namespace HouseMangment.Controllers
{
    public class DevicesController : Controller
    {
        private WhereHouseEntities db = new WhereHouseEntities();
        public bool hisadmin = UsersController.hisadmin;
        // GET: Devices
        public ActionResult Index()
        {
            if (hisadmin == true)
            {
                return View(db.Devices.Where(x => x.isActive == true).ToList());
            }
            else
            {
                               return RedirectToAction("login", "users");
            }
        }

        // GET: Devices/Details/5
        public ActionResult Details(int? id)
        {
                if (hisadmin == true)
                {
                    if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Devices devices = db.Devices.Find(id);
            if (devices == null || devices.isActive == false)
            {
                return HttpNotFound();
            }
            return View(devices);
                }
                else
                {
                                   return RedirectToAction("login", "users");
                }
            }

        // GET: Devices/Create
        public ActionResult Create()
        {
                    if (hisadmin == true)
                    {
                        return View();
                    }
                    else
                    {
                                       return RedirectToAction("login", "users");
                    }
                }

        // POST: Devices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Devices devices)
        {
                        if (hisadmin == true)
                        {
                            if (ModelState.IsValid)
            {
                db.Devices.Add(devices);
                devices.isActive = true;
                db.SaveChanges();
                return RedirectToAction("Index" , TempData["SuccessMessage"] = "Good");
            }

            return View(devices);
                        }
                        else
                        {
                                           return RedirectToAction("login", "users");
                        }
                    }

        // GET: Devices/Edit/5
        public ActionResult Edit(int? id)
        {
                            if (hisadmin == true)
                            {
                                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Devices devices = db.Devices.Find(id);
            if (devices == null || devices.isActive == false)
            {
                return HttpNotFound();
            }
            return View(devices);
                            }
                            else
                            {
                                               return RedirectToAction("login", "users");
                            }
                        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Devices devices)
        {
                                if (hisadmin == true)
                                {
                                    if (ModelState.IsValid)
            {
                var upd = db.Devices.Find(devices.Id);
                    upd.SerialNumber = devices.SerialNumber;
                    upd.Name = devices.Name;
                db.SaveChanges();
                return RedirectToAction("Index", TempData["SuccessMessage"] = true);
            }
            return View(devices);
                                }
                                else
                                {
                                                   return RedirectToAction("login", "users");
                                }
                            }

        // GET: Devices/Delete/5
        public ActionResult Delete(int? id)
        {
                                    if (hisadmin == true)
                                    {
                                        if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Devices devices = db.Devices.Find(id);
            if (devices == null)
            {
                return HttpNotFound();
            }
            
            var checkUnassign = db.Status.Where(x => x.Device_id == id && x.isActive == true).Any();
            if (checkUnassign)
            {
                TempData["inassign"] = true;

            }

            return View(devices);
                                    }
                                    else
                                    {
             return RedirectToAction("login", "users");
                                    }
                                }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
                                        if (hisadmin == true)
                                        {

                                            var findid = db.Devices.Find(id);
            findid.isActive = false;
            db.SaveChanges();
            return RedirectToAction("Index");
                                        }
                                        else
                                        {
              return RedirectToAction("login", "users");
                                        }
                                    }


    }
}
