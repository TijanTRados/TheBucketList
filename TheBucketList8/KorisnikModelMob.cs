using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TheBucketList8
{
    class KorisnikModelMob
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }

        public string Prezime { get; set; }
        public byte[] Slika { get; set; }

        public string Moto { get; set; }

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