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

namespace TheBucketListMobile7
{
    public class Korisnik
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public string imeprezime { get; set; }
        public string opis { get; set; }
        public string motto { get; set; }
        public byte[] slika { get; set; }

        public List<BucketItem> bucket = new List<BucketItem>();
    }
}