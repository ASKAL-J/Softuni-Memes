using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Softuni___Memes.Models
{
    public class Rating
    {
        public Rating()
        {
            this.UserId = HttpContext.Current.User.Identity.GetUserId();
        }

        [Key]
        public int Id { get; set; }

        public int Score { get; set; }

        public int ImageId { get; set; }

        public string UserId { get; set; }
    }
}