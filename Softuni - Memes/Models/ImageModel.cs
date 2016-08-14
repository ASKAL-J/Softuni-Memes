using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Softuni___Memes.Models
{
    [Table("Images")]
    public class ImageModel
    {
        [Key]
        public int Id { get; set; }

        [Column("image", TypeName = "image")]
        public byte[] Image { get; set; }

        public string AuthorId { get; set; }
    }
}