using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheBucketList.Models
{
    public class ImageViewModel
    {
        [Required]
        public HttpPostedFileBase File { get; set; }
    }
}