using System;
using Softuni___Memes.Extensions;
using Softuni___Memes.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Softuni___Memes.Controllers
{
    [ValidateInput(false)]
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            return View(db.Comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create(int id)
        {
            Comment comment = new Comment
            {
                ImageId = id,
            };

            return View(comment);
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Body,ImageId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ApplicationUser user = userManager.FindById(this.User.Identity.GetUserId());

                comment.AuthorFirstName = user.FirstName;
                comment.AuthorLastName = user.LastName;

                db.Comments.Add(comment);
                db.SaveChanges();
                this.AddNotification("You have successfully commented.", NotificationType.SUCCESS);
                return RedirectToAction("Details", "Image", new { id = comment.ImageId });
            }

            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (this.User.Identity.GetUserName() != comment.UserName)
            {
                this.AddNotification("You are not authorized to edit this comment", NotificationType.ERROR);
                return RedirectToAction("Details", "Image", new { id = comment.ImageId });
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Body,ImageId,UserId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                var cm = this.db.Comments.Find(comment.Id);
                cm.Body = comment.Body;
                db.Entry(cm).State = EntityState.Modified;
                db.SaveChanges();
                this.AddNotification("Comment successfully updated.", NotificationType.SUCCESS);
                return RedirectToAction("Details", "Image", new { id = cm.ImageId });
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (this.User.Identity.GetUserName() != comment.UserName)
            {
                this.AddNotification("You are not authorized to delete this comment", NotificationType.ERROR);
                return RedirectToAction("Details", "Image", new { id = comment.ImageId });
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            this.AddNotification("Comment successfully deleted.", NotificationType.INFO);
            return RedirectToAction("Details", "Image", new { id = comment.ImageId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}