using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TheBucketList.NHibernate.Entities;

namespace TheBucketList.Models
{
    public class BucketItemModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Item Name")]
        public string Ime { get; set; }

        [Display(Name = "Completed")]
        public bool Ostvareno { get; set; }


        [Display(Name = "Picture")]
        public byte[] Slika { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Opis { get; set; }


        [Required]
        public Kategorija Kategorija { get; set; }

        public string KategorijaNaziv { get; set; }
    }
}