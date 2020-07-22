using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryMSMVC.Models;

namespace LibraryMSMVC.Controllers
{
    public class TblUsersController : Controller
    {
        private UserEntity userDb = new UserEntity();

        // GET: tblUsers
        public ActionResult Index()
        {
            return View(userDb.tblUsers.ToList());
        }

        // GET: tblUsers Json
        public ActionResult GetAll()
        {
            var userlist = userDb.tblUsers.ToList();
            return Json(new { data = userlist }, JsonRequestBehavior.AllowGet);
        }

        // GET: tblUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = userDb.tblUsers.Find(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }
            return View(tblUser);
        }

        // GET: tblUsers/Create
        public ActionResult Create()
        {  
            return View();
        }

        // POST: tblUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,UserName,UserGender,UserDep,UserAdmNo,UserEmail,UserPass")] tblUser tblUser)
        {
            Session.Remove("emailExists");
            if (ModelState.IsValid)
            {
                
                if (userDb.tblUsers.Where(u=>u.UserEmail==tblUser.UserEmail).Count()>0)
                {
                    Session["emailExists"] = "The Email address is already exists.";
                    return View(tblUser);
                }
                else
                {
                    Session["operationMsg"] = "User added successfully";
                    userDb.tblUsers.Add(tblUser);
                    userDb.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(tblUser);
        }

        // Remove the session datas which are used for alerts
        // OperationAlert
        public ActionResult OperationAlert()
        {
            Session.Remove("operationMsg");
            return RedirectToAction("Index");
        }

        // GET: tblUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = userDb.tblUsers.Find(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }
            return View(tblUser);
        }

        // POST: tblUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,UserGender,UserDep,UserAdmNo,UserEmail,UserPass")] tblUser tblUser)
        {
            if (ModelState.IsValid)
            {
                Session["operationMsg"] = "User updated successfully";
                userDb.Entry(tblUser).State = EntityState.Modified;
                userDb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblUser);
        }

        // GET: tblUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = userDb.tblUsers.Find(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }
            return View(tblUser);
        }

        // POST: tblUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblUser tblUser = userDb.tblUsers.Find(id);
            userDb.tblUsers.Remove(tblUser);
            userDb.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                userDb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
