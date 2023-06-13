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
    public class UsersController : Controller
    {
        private WhereHouseEntities db = new WhereHouseEntities();

        // GET: Users
        public ActionResult Index()
        {
            var user = db.Users.Where(x => x.isActive == true && x.isAdmin == false && x.IsDeleted != true).ToList();
            int countnoactive = db.Users.Where(x => x.isActive == false && x.isAdmin == false && x.IsDeleted != true).Count();
            if (countnoactive > 0) { ViewBag.coun = countnoactive; }
            return View(user);
        }

        public ActionResult InactiveUsers()
        {
            var users = db.Users.Where(x => x.isActive == false && x.isAdmin == false && x.IsDeleted != true).ToList();
            return View(users);


        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,phone,username,isAdmin,isActive,Password")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(users);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

         public ActionResult Login()
        {
            if (Session["UserName"] == null)
            {
                return View();
            }
            else
            {
             return RedirectToAction("Index");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users user)
        {
            var tryLogin = db.Users.Where(u => u.username == user.username && u.Password == user.Password && u.isActive ==true && u.IsDeleted !=true).FirstOrDefault();
            if (tryLogin != null)
            {

                if (user.isAdmin == true)
                {
                    Session["Admin"] = true;
                    return RedirectToAction("users");

                } else
                {
                    return RedirectToAction("users","myassign");

                }
            }
            else
            {
                ViewBag.Login = "تأكد من كلمة المرور واسم المستخدم";
            }
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Users user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "تأكد من البيانات المدخلة";
            }
            
                var compare = db.Users.Where(m => m.username == user.username).Count();
                while (compare > 0)
                {
                    ViewBag.Message = "هذا المستخدم موجود مسبقاً";
                    return View();
                }
                var add = db.Users.Add(user);
                add.isActive = false;
                add.isAdmin = false;

                db.SaveChanges();
                ViewBag.Message = "تم تسجيلك بنجاح";
                return RedirectToAction("Login", "User");
            
        }
    



}
}
