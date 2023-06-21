using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using System.Web.UI.WebControls;
using HouseMangment.Entity;
using Rotativa;


namespace HouseMangment.Controllers
{

    public class UsersController : Controller
    {

        
        public static bool hisadmin;
        public static bool logged;
        public static int myid;
        public static string username;

        private WhereHouseEntities db = new WhereHouseEntities();

        // GET: Users
        public ActionResult Index()
        {
// Check if the user is an admin
if (hisadmin == true) 
            { 

            var user = db.Users.Where(x => x.isActive == true && x.isAdmin != true && x.IsDeleted != true).ToList();
            int countnoactive = db.Users.Where(x => x.isActive != true && x.isAdmin != true && x.IsDeleted != true).Count();
            if (countnoactive > 0) { ViewBag.coun = countnoactive; }
            return View(user);
            }
            else
            {
             return RedirectToAction("login");
            }
        }

        public ActionResult InactiveUsers()
        {
            // Check if the user is an admin
            if (hisadmin == true)
            {

                var users = db.Users.Where(x => x.isActive != true && x.isAdmin != true && x.IsDeleted != true).ToList();
                return View(users);
            }
            else
            {
                return RedirectToAction("login");
            }

            }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            // Check if the user is an admin
            if (hisadmin == true)
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
            else
            {
                return RedirectToAction("login");
    }
}



        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            // Check if the user is an admin
            if (hisadmin == true)
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
            else
            {
                return RedirectToAction("login");
            }
        }


        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Users users)
        {
            // Check if the user is an admin
            if (hisadmin == true)
                {
                    if (ModelState.IsValid)
            {
                    var upd = db.Users.Find(users.Id);
                    upd.name = users.name;
                    upd.phone = users.phone;
                    upd.username = users.username;
                    upd.Password = users.Password;
                    var compare = db.Users.Where(m => m.username == users.username).Where(u => u.Id != users.Id).Count();
                    while (compare > 0)
                    {
                        return RedirectToAction("edit", TempData["username"] = true);

                    }
                    db.SaveChanges();


                    return RedirectToAction("index", TempData["SuccessMessage"] = true);
                }
            return View(users);
            }
            else
            {
                return RedirectToAction("login");
            }
        }


        public ActionResult Delete(int? id)
        {
            // Check if the user is an admin
            if (hisadmin == true)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Users user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                var checkUnassign = db.Status.Where(x => x.User_id == id && x.isActive == true).Count();
                if (checkUnassign > 0)
                {
                    TempData["countdev"] = checkUnassign;
                    TempData["inassign"] = true;
                    return RedirectToAction("index");
                }
                else
                {

                    TempData["name"] = user.name;
                    TempData["id"] = user.Id;
                    return RedirectToAction("index");
                }
            }
            else
            {
                return RedirectToAction("login", "users");
            }
        }


        public ActionResult Deletet(int id)
        {
            // Check if the user is an admin
            if (hisadmin == true)
            {

                var findid = db.Users.Find(id);
                findid.IsDeleted = true;
                db.SaveChanges();
                return RedirectToAction("Index", TempData["deleted"] = true);
            }
            else
            {
                return RedirectToAction("login", "users");
            }
        }



        public ActionResult Login()
        {
            if (logged != true)
            {
                return View();
            }
            else
            {
                // Check if the user is an admin
                if (hisadmin != true)
                {
                    return RedirectToAction("myassign", "status");

                }
                else
                {

                    return RedirectToAction("index");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users user)
        {
          if (logged != true)
           {
            var tryLogin = db.Users.Where(u => u.username == user.username && u.Password == user.Password && u.isActive ==true && u.IsDeleted !=true).FirstOrDefault();
            if (tryLogin != null)
            {
                    // Check if the user is an admin
                    if (tryLogin.isAdmin == true)
                {

                    hisadmin = true;
                    logged = true;
                    myid = tryLogin.Id;
                    username = tryLogin.username;

                        return RedirectToAction("index");

                } else
                {
                                    myid = tryLogin.Id;
                                    logged = true;
                        username = tryLogin.username;

                        return RedirectToAction("myassign", "status");

                }
            }
            else
            {
                ViewBag.LoginError = "The username or password is wrong , or the user is not activated";
            }

            return View();
            }
            else
            {
                // Check if the user is an admin
                if (hisadmin != true)
                {
                    return RedirectToAction("myassign", "status");

                }
                else
                {

                    return RedirectToAction("index");
                }
            }
        }

        public ActionResult Register()
        {
           if (logged != true)
            {
             return View();
            }
            else
            {
             return RedirectToAction("index");
            }
        }
        [HttpPost]
        public ActionResult Register(Users user)
        {
            
                var compare = db.Users.Where(m => m.username == user.username).Count();
                while (compare > 0)
                {
                    ViewBag.Message = "Username is already registered";
                    return View();
                }
                var add = db.Users.Add(user);
                add.isActive = false;
                add.isAdmin = false;

                db.SaveChanges();
                TempData["good"] = "registered successfully, please wait for the user to be activated by the administrator";
                return RedirectToAction("login", TempData["good"]);


        }
        [HttpGet]
        public ActionResult logout()
        {
            if (logged == true)
            {
                hisadmin = false;
                logged = false;
                return RedirectToAction("Login", "Users");
            }
            else
            {
                return RedirectToAction("index");
            }


        }
        public ActionResult Active(int id)
        {
            // Check if the user is an admin
            if (hisadmin == true)
            {

                var findid = db.Users.Find(id);
                findid.isActive = true;
                db.SaveChanges();
                return RedirectToAction("inactiveusers", TempData["SuccessMessage"] = true);
            }
            else
            {
                return RedirectToAction("login", "users");
            }
        }


        public ActionResult profile()
        {

            // Check if the user is login
            if (logged == true)
            {
                
                Users users = db.Users.Find(myid);
                if (users == null)
                {
                    return HttpNotFound();
                }
                return View(users);
            }
            else
            {
                return RedirectToAction("login");
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult profile(Users users)
        {
            // Check if the user is login
            if (logged == true)
            {
                if (ModelState.IsValid)
                {
                    var upd = db.Users.Find(myid);

                    var compare = db.Users.Where(m => m.username == users.username).Where(u => u.Id != myid).Count();
                    while (compare > 0)
                    {
                        return RedirectToAction("profile", TempData["username"] = true);

                    }
                    upd.name = users.name;
                    upd.phone = users.phone;
                    upd.username = users.username;


                    db.SaveChanges();

                    return RedirectToAction("Index", TempData["SuccessMessage"] = true);
                }
                return View();

            }
            else
            {
                return RedirectToAction("login");
            }
        }
        public ActionResult ChangePassword()
        {

            // Check if the user is login
            if (logged == true)
            {

                return View();
            }
            else
            {
                return RedirectToAction("login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(Users users)
        {
            // Check if the user is login
            if (logged == true)
            {
                var upd = db.Users.Find(myid);

                if (users.Password != users.rePassword)
                {
                    return RedirectToAction("changepassword", TempData["nocomp"] = true);

                }

                var compare = db.Users.Where(m => m.Password == users.CrPassword).Where(u => u.Id == myid).Any();
                if (compare == true)
                {
                    upd.Password = users.Password;
                    db.SaveChanges();
                    return RedirectToAction("index", TempData["SuccessMessage"] = true);

                }
                return RedirectToAction("changepassword", TempData["nopass"] = true);

            }
            else
            {
                return RedirectToAction("login");
            }
        }



        public ActionResult Report()
        {
            if (logged == true)
            {
                var users = db.Users.Where(x => x.isActive == true && x.isAdmin != true && x.IsDeleted != true);
                return new ViewAsPdf("Report", users);
            }
            else
            {
                return RedirectToAction("login");
            }
        }
    }


}
