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
    public class TblBooksController : Controller
    {
        private BookEntity bookDb = new BookEntity();

        // GET: tblBooks
        public ActionResult Index()
        {
            return View(bookDb.tblBooks.ToList());
        }
        // GET: tblBooks Json
        public ActionResult GetAll()
        {
            var booklist = bookDb.tblBooks.ToList();
            return Json(new { data = booklist }, JsonRequestBehavior.AllowGet);
        }

        // GET: tblBooks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBook tblBook = bookDb.tblBooks.Find(id);
            if (tblBook == null)
            {
                return HttpNotFound();
            }
            return View(tblBook);
        }

        // GET: tblBooks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tblBooks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookId,BookTitle,BookCategory,BookAuthor,BookCopies,BookPub,BookPubName,BookISBN,Copyright,DateAdded,Status")] tblBook tblBook)
        {
            if (ModelState.IsValid)
            {
                Session["operationMsg"] = "Book added successfully";
                bookDb.tblBooks.Add(tblBook);
                bookDb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblBook);
        }

        // Remove the session datas which are used for alerts
        // OperationAlert
        public ActionResult OperationAlert()
        {
            Session.Remove("operationMsg");
            return RedirectToAction("Index");

        }

        // GET: tblBooks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBook tblBook = bookDb.tblBooks.Find(id);
            if (tblBook == null)
            {
                return HttpNotFound();
            }
            return View(tblBook);
        }

        // POST: tblBooks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookId,BookTitle,BookCategory,BookAuthor,BookCopies,BookPub,BookPubName,BookISBN,Copyright,DateAdded,Status")] tblBook tblBook)
        {
            if (ModelState.IsValid)
            {
                Session["operationMsg"] = "Book updated successfully";
                bookDb.Entry(tblBook).State = EntityState.Modified;
                bookDb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblBook);
        }

        // GET: tblBooks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBook tblBook = bookDb.tblBooks.Find(id);
            if (tblBook == null)
            {
                return HttpNotFound();
            }
            return View(tblBook);
        }

        // POST: tblBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblBook tblBook = bookDb.tblBooks.Find(id);
            bookDb.tblBooks.Remove(tblBook);
            bookDb.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bookDb.Dispose();
            }
            base.Dispose(disposing);
        }
        

    }
}
