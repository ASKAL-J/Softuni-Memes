using Softuni___Memes.Extensions;
using Softuni___Memes.Models;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Softuni___Memes.Controllers
{
    public class ImageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Image
        public ActionResult Index()
        {
            var score = Request.Params["rate"];
            var imageId = Request.Params["ImageId"];
            if (score != null)
            {
                Rating rating = new Rating();
                rating.Score = int.Parse(score);
                rating.ImageId = int.Parse(imageId);
                db.Ratings.Add(rating);
                db.SaveChanges();
            }

            return View(db.ImageModels.ToList());
        }


        public ActionResult GetImage(int id)
        {
            var image = db.ImageModels.Single(p => p.Id == id).Image;

            return File(image, "image/png");
        }

        public ActionResult DownloadImage(int id)
        {
            var image = db.ImageModels.Single(p => p.Id == id).Image;
            return File(
         image, System.Net.Mime.MediaTypeNames.Application.Octet, id.ToString() + ".png");
        }

        // GET: Image/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageModel imageModel = db.ImageModels.Find(id);

            var comments = db.Comments.ToList();

            ViewBag.Comments = comments.Where(i => i.ImageId == id).ToList();

            if (imageModel == null)
            {
                return HttpNotFound();
            }
            return View(imageModel);
        }

        // GET: Image/Create
        public ActionResult Create()
        {
            var image = Request.Params["base64"];

            if (image != null)
            {
                ImageModel imageModel = new ImageModel();
                imageModel.Image = Convert.FromBase64String(image);
                db.ImageModels.Add(imageModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // POST: Image/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*  [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Create([Bind(Include = "Id,Image")] ImageModel imageModel)
          {
              if (ModelState.IsValid)
              {
                  db.ImageModels.Add(imageModel);
                  db.SaveChanges();
                  this.AddNotification("Image created.", NotificationType.SUCCESS);
                  return RedirectToAction("Index");
              }

              return View(imageModel);
          }*/

        // GET: Image/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageModel imageModel = db.ImageModels.Find(id);
            if (imageModel == null)
            {
                return HttpNotFound();
            }
            return View(imageModel);
        }

        // POST: Image/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ImageModel imageModel = db.ImageModels.Find(id);
            Rating rating = db.Ratings.Single(r =>r.ImageId == id);
            db.ImageModels.Remove(imageModel);
            db.Ratings.Remove(rating);
            db.SaveChanges();
            this.AddNotification("Image created.", NotificationType.SUCCESS);
            return RedirectToAction("Index");
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