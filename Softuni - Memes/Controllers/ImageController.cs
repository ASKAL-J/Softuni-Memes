using Softuni___Memes.Extensions;
using Softuni___Memes.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Mvc;
using X.PagedList;

namespace Softuni___Memes.Controllers
{
    public class ImageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const int MaxImagesPerPage = 9;

        // GET: Image
        public ActionResult Index(int? page)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            var score = Request.Params["rate"];
            var imageId = Request.Params["ImageId"];
            if (score != null)
            {
                Rating rating = new Rating();
                rating.Score = double.Parse(score);
                rating.ImageId = int.Parse(imageId);

                string userId = rating.UserId;
                this.AddScore(double.Parse(score), int.Parse(imageId), userId, rating);

                db.SaveChanges();
            }

            //Paging the images

            //number of images
            var images = db.ImageModels
                .OrderByDescending(img => img.DateCreated).ToList();
            //If no page specified in query, we set the default to 1
            var pageNumber = page ?? 1;

            //May need a different formula, but it's working fine for now
            if (pageNumber > (images.Count / MaxImagesPerPage) + 1)
            {
                this.AddNotification("This page does not exist.", NotificationType.WARNING);
                return RedirectToAction("Index");
            }

            var onePageOfImages = images.ToPagedList(pageNumber, MaxImagesPerPage);

            ViewBag.OnePageOfImages = onePageOfImages;

            return View();
        }

        /// <summary>
        /// Takes the top 5 rated images from the Database
        /// and orders them in descending order
        /// </summary>
        /// <returns>The needed view</returns>
        public ActionResult TopFive()
        {
            List<ImageModel> topRatedImages =
                this.db.ImageModels
                .OrderByDescending(img => img.AverageScore)
                .Take(5)
                .ToList();

            return View(topRatedImages);
        }

        /// <summary>
        /// Returns the image with the requested id
        /// </summary>
        /// <param name="id">The id of the image to return</param>
        /// <returns>Transformed byte array to img file</returns>
        public ActionResult GetImage(int id)
        {
            var image = db.ImageModels.Single(p => p.Id == id).Image;

            return File(image, "image/png");
        }

        /// <summary>
        /// Downoads the image with the given id
        /// </summary>
        /// <param name="id">The id of the image to download</param>
        /// <returns>Transformed byte array to img file
        /// and the location of the download</returns>
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

            var comments = db.Comments.Where(i => i.ImageId == id).ToList();
            ViewBag.Comments = comments;

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
                ImageModel imageModel = new ImageModel
                {
                    Image = Convert.FromBase64String(image),
                };

                db.ImageModels.Add(imageModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

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
            var ratings = db.Ratings.Where(r => r.ImageId == id);
            var comments = db.Comments.Where(comment => comment.ImageId == id);
            db.Comments.RemoveRange(comments);
            db.ImageModels.Remove(imageModel);
            db.Ratings.RemoveRange(ratings);
            db.SaveChanges();
            this.AddNotification("Image deleted.", NotificationType.INFO);

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

        /// <summary>
        /// Adds the score to the image model
        /// and created a rating with the given score
        /// or overwrites an existing one
        /// </summary>
        /// <param name="score">The score of the rating</param>
        /// <param name="imageId">The id of the image</param>
        /// <param name="userId">The user id of the rating</param>
        /// <param name="rating">The rating to add in the database</param>
        private void AddScore(double score, int imageId, string userId, Rating rating)
        {
            ImageModel image = this.db.ImageModels.Single(img => img.Id == imageId);
            Rating existingRating = db.Ratings
                .FirstOrDefault(r => r.ImageId == imageId && r.UserId == userId);
            int numberOfRatingsForModel = db.Ratings.Count(r => r.ImageId == imageId);

            if (existingRating == null)
            {
                image.OverallScore += score;
                numberOfRatingsForModel++;
                db.Ratings.Add(rating);
                db.SaveChanges();
            }
            else
            {
                image.OverallScore -= existingRating.Score;
                existingRating.Score = score;
                image.OverallScore += score;
            }

            image.AverageScore = image.OverallScore / numberOfRatingsForModel;
            db.SaveChanges();
            this.AddNotification("Image successfully rated.", NotificationType.SUCCESS);
        }
    }
}