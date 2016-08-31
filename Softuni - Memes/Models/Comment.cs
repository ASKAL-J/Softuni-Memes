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
            this.UserName = HttpContext.Current.User.Identity.GetUserName();
            DateTime date = DateTime.Now;
            this.Date = date.ToLocalTime();
        }

        [Key]
        public int Id { get; set; }

        public string Body { get; set; }

        public DateTime Date { get; set; }

        public int ImageId { get; set; }

        public string UserName { get; set; }

        public string AuthorFirstName { get; set; }

        public string AuthorLastName { get; set; }
    }
}