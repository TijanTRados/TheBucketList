using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheBucketList.Models
{
    public class BuketListModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Item Name")]
        public string Ime { get; set; }

        [Display(Name = "Completed")]
        public bool Ostvareno { get; set; }

        [Display(Name = "Picture")]
        [Required]
        public HttpPostedFileBase Slika { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Opis { get; set; }
        public List<SelectListItem> kategorijaItems { get; set; }

        [Display(Name = "Category")]
        public string KategorijaId { get; set; }

        public BuketListModel()
        {
            kategorijaItems = new List<SelectListItem>();
        }
    }

    public class BuketListModelWithoutPicture
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Item Name")]
        public string Ime { get; set; }

        [Display(Name = "Completed")]
        public bool Ostvareno { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Opis { get; set; }
        public List<SelectListItem> kategorijaItems { get; set; }

        [Display(Name = "Category")]
        public string KategorijaId { get; set; }

        public BuketListModelWithoutPicture()
        {
            kategorijaItems = new List<SelectListItem>();
        }
    }
}