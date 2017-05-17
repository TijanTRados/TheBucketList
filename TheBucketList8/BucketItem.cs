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
    public class BucketItem
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public bool Ostvareno { get; set; }
        public byte[] Slika { get; set; }
        public string Opis { get; set; }
        public string KategorijaNaziv { get; set; }

        public int koliko { get; set; }
    }
}