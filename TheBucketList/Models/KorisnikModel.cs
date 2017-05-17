using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheBucketList.Models
{
    public class KorisnikModel
    {
        public int Id { get; set; }
        public string Username { get; set; }

        [Display(Name = "Password")]
        public string Lozinka { get; set; }

        [Display(Name = "First Name")]
        public string Ime { get; set; }

        [Display(Name = "Last Name")]
        public string Prezime { get; set; }
        public byte[] Slika { get; set; }

        [Display(Name = "Motto")]
        public string Moto { get; set; }

        [Display(Name = "About me")]
        public string Opis { get; set; }

        public string FullName
        {
            get
            {
                return Ime + " " + Prezime;
            }
        }
    }
}