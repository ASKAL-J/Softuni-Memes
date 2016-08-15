using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Softuni___Memes.Models
{
    public class Comment
    {
        public Comment()
        {
            this.UserId = HttpContext.Current.User.Identity.GetUserId();
            this.Date = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        public string Body { get; set; }

        public DateTime Date { get; set; }

        public int ImageId { get; set; }

        public string UserId { get; set; }
    }
}