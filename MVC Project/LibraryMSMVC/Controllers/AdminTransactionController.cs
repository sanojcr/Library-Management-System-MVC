using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryMSMVC.Models;

namespace LibraryMSMVC.Controllers
{

    public class AdminTransactionController : Controller
    {
        private BookEntity bookDb = new BookEntity();
        private TransEntity transDb = new TransEntity();

        // Returns admin request view, here admin can accept and reject the book requests
        public ActionResult Requests()
        {
            return View(transDb.tblTransactions.ToList());
        }
        // Returns all book requests in json format.
        public ActionResult GetAllRequests()
        {
            var transactionList = transDb.tblTransactions.Where(r => r.TranStatus == "Requested").ToList();
            return Json(new { data = transactionList }, JsonRequestBehavior.AllowGet);
        }
        // Accepts the book request.
        public ActionResult AcceptRequest(int? tranId)
        {
           /* try
            {*/
                if (tranId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tblTransaction transaction = transDb.tblTransactions.FirstOrDefault(t => t.TranId == tranId);
                if (transaction == null)
                {
                    return HttpNotFound();
                }
                transaction.TranStatus = "Accepted";
                transaction.TranDate = DateTime.Now.ToShortDateString();
                transDb.SaveChanges();
                return View("Requests");
            /*}
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/

        }
        // Reject the book request. 
        public ActionResult RejectRequest(int? tranId)
        {
            /*try
            {*/
                if (tranId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tblTransaction transaction = transDb.tblTransactions.FirstOrDefault(t => t.TranId == tranId);
                if (transaction == null)
                {
                    return HttpNotFound();
                }
                transaction.TranStatus = "Rejected";
                transaction.TranDate = DateTime.Now.ToShortDateString();
                tblBook book = bookDb.tblBooks.FirstOrDefault(b => b.BookId == transaction.BookId);
                book.BookCopies = book.BookCopies + 1;
                bookDb.SaveChanges();
                transDb.SaveChanges();
                return View("Requests");
            /*}
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
        }
        // Returns admin accepted view, here admin can view the accepted books.
        public ActionResult Accepted()
        {
            return View(transDb.tblTransactions.ToList());
        }
        // Returns all accepted books in json format.
        public ActionResult GetAllAccepted()
        {
            var transactionList = transDb.tblTransactions.Where(r => r.TranStatus == "Accepted").ToList();
            return Json(new { data = transactionList }, JsonRequestBehavior.AllowGet);
        }
        // Returns admin return view, here admin can accept book return requests.
        public ActionResult Return()
        {
            return View(transDb.tblTransactions.ToList());
        }
        // Returns all return books in json format.
        public ActionResult GetAllReturn()
        {
            var transactionList = transDb.tblTransactions.Where(r => r.TranStatus == "Returned").ToList();
            return Json(new { data = transactionList }, JsonRequestBehavior.AllowGet);
        }
        // Accepts the book return request.
        public ActionResult AcceptReturn(int? tranId)
        {

            /*try
            {*/
                if (tranId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tblTransaction transaction = transDb.tblTransactions.FirstOrDefault(t => t.TranId == tranId);
                if (transaction == null)
                {
                    return HttpNotFound();
                }
                tblBook book = bookDb.tblBooks.FirstOrDefault(b => b.BookId == transaction.BookId);
                book.BookCopies = book.BookCopies + 1;
                bookDb.SaveChanges();
                transDb.tblTransactions.Remove(transaction);
                transDb.SaveChanges();
                return View("Return");
            /*}
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
        }
        // Returns admin home view.
        public ActionResult AdminHome()
        {
            return View();
        }
        // Returns admin about view.
        public ActionResult AdminAbout()
        {
            return View();
        }
        // Returns admin contact view.
        public ActionResult AdminContact()
        {
            return View();
        }
    }
}