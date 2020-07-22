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
    public class BorrowController : Controller
    {
        static int userId;          // Used to store user id.
        static string userName;     // Used to store user name.

        private UserEntity userDb = new UserEntity();
        private BookEntity bookDb = new BookEntity();
        private TransEntity transDb = new TransEntity();
       

        // Returns user books borrow view, here user can request for a book.
        public ActionResult Index(int? userId, string userName)
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

            BorrowController.userId = (int)userId;
            BorrowController.userName = userName;
            return View(bookDb.tblBooks.ToList());
        }

        // Returns user home view.
        public ActionResult UserHome()
        {
            return View();
        }

        // Returns user about view.
        public ActionResult UserAbout()
        {
            return View();
        }

        // Returns user contact view.
        public ActionResult UserContact()
        {
            return View();
        }

        // Navbar menus.
        // Redirected to index view of borrow controller with user id and username.
        public ActionResult MenuBorrow()
        {
            return RedirectToAction("Index", "Borrow", new { userId = userId, userName = userName });
        }

        // Redirected to Requested view of user transaction controller with user id.
        public ActionResult MenuRequested()
        {
            return RedirectToAction("Requested", "UserTransaction", new { userId = userId });
        }

        // Redirected to Received view of user transaction controller with user id.
        public ActionResult MenuReceived()
        {
            Session.Remove("receivedBadge");
            return RedirectToAction("Received", "UserTransaction", new { userId = userId });
        }

        // Redirected to Rejected view of user transaction controller with user id.
        public ActionResult MenuRejected()
        {
            Session.Remove("rejectedBadge");
            return RedirectToAction("Rejected", "UserTransaction", new { userId = userId });
        }

        // Borrow the book, redirect to index view.
        public ActionResult Borrow(int? bookId)
        {
            /*try
            {*/
                if (transDb.tblTransactions.Where(t => t.UserId == userId).Count() < 6)
                {
                    if (bookId != null)
                    {
                        tblBook book = bookDb.tblBooks.FirstOrDefault(b => b.BookId == bookId);
                        if (book == null)
                        {
                            return HttpNotFound();
                        }
                        if (book.BookCopies > 0)
                        {
                            book.BookCopies = book.BookCopies - 1;
                            tblTransaction trans = new tblTransaction()
                            {
                                BookId = book.BookId,
                                BookTitle = book.BookTitle,
                                BookISBN = book.BookISBN,
                                TranDate = DateTime.Now.ToShortDateString(),
                                TranStatus = "Requested",
                                UserId = userId,
                                UserName = userName,
                            };
                            bookDb.SaveChanges();
                            transDb.tblTransactions.Add(trans);
                            transDb.SaveChanges();
                            Session["requestMsg"] = "Requested successfully";
                        }
                        else
                        {
                            Session["requestMsg"] = "Sorry you cant take, Book copy is zero";
                        }
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    Session["requestMsg"] = "Sorry you cant take more than six books";
                }
                return RedirectToAction("Index", "Borrow", new { userId = userId, userName = userName });
            /*}
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
        }

        // Remove the session datas which are used for alerts
        // ReqAlert
        public ActionResult RequestAlert()
        {
            Session.Remove("requestMsg");
            return RedirectToAction("Index", "Borrow", new { userId = userId, userName = userName });
        }
        
    }
}