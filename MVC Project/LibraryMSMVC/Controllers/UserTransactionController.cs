using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryMSMVC.Models;

namespace LibraryMSMVC.Controllers
{
    public class UserTransactionController : Controller
    {
        static int userId;      // Used to store user id

        TransEntity transDb = new TransEntity();
        BookEntity bookDb = new BookEntity();
        private UserEntity userDb = new UserEntity();

        // Returns user requested view, here user can cancel request.
        public ActionResult Requested(int? userId)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser user = userDb.tblUsers.Find(userId);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserTransactionController.userId = (int)userId;
            var requestList = transDb.tblTransactions.Where(s => s.TranStatus == "Requested" && s.UserId == userId);
            if (requestList.Count()==0)
            {
                Session["requestMessage"]= "Your Requested list is empty, Go to Borrow section for request a book.";
            }
            else
            {
                Session.Remove("requestMessage");
            }
            return View(requestList.ToList());
        }

        // Cancel book request, redirected to requested
        public ActionResult DeleteRequest(int? tranId)
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
                return RedirectToAction("Requested", "UserTransaction", new { userId = userId });
           /* }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
        }

        // Returns user rejected view, here user can rerequest and cancel book request.
        public ActionResult Rejected(int? userId)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser user = userDb.tblUsers.Find(userId);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserTransactionController.userId = (int)userId;
            var rejectedList = transDb.tblTransactions.Where(s => s.TranStatus == "Rejected" && s.UserId == userId);
            if (rejectedList.Count() == 0)
            {
                Session["rejectMessage"] = "Your Rejected list is empty, Wait for the admin to take action.";
            }
            else
            {
                Session.Remove("rejectMessage");
            }
            return View(rejectedList.ToList());
        }

        // Rerequest book request, redirected to rejected
        public ActionResult RerequestRejected(int? tranId)
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
                transaction.TranStatus = "Requested";
                transaction.TranDate = DateTime.Now.ToShortDateString();
                tblBook book = bookDb.tblBooks.FirstOrDefault(b => b.BookId == transaction.BookId);
                book.BookCopies = book.BookCopies - 1;
                bookDb.SaveChanges();
                transDb.SaveChanges();
                return RedirectToAction("Rejected", "UserTransaction", new { userId = userId });
            /*}
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
        }

        // Cancel book request, redirected to rejected
        public ActionResult CancelRejected(int? tranId)
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
                return RedirectToAction("Rejected", "UserTransaction", new { userId = userId });
           /* }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
        }

        // Returns user received view, here user can read and return the book, redirected to received
        public ActionResult Received(int? userId)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser user = userDb.tblUsers.Find(userId);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserTransactionController.userId = (int)userId;
            var receivedList = transDb.tblTransactions.Where(s => s.TranStatus == "Accepted" && s.UserId == userId);
            if (receivedList.Count() == 0)
            {
                Session["receiveMessage"] = "Your Received list is empty, Wait for the admin to take action.";
            }
            else
            {
                Session.Remove("receiveMessage");
            }
            return View(receivedList.ToList());
        }

        // Return book
        public ActionResult ReturnReceived(int? tranId)
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
                transaction.TranDate = DateTime.Now.ToShortDateString();
                transaction.TranStatus = "Returned";
                transDb.SaveChanges();
                return RedirectToAction("Received", "UserTransaction", new { userId = userId });
           /* }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
        }
    }
}