using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheBucketList.NHibernate.Entities
{
    public class Korisnik
    {
        public virtual int Id { get; protected set; }
        public virtual string Username { get; set; }
        public virtual string Lozinka { get; set; }
        public virtual string Ime { get; set; }
        public virtual string Prezime { get; set; }
        public virtual byte[] Slika { get; set; }
        public virtual string Moto { get; set; }
        public virtual string Opis { get; set; }
        public virtual IList<BucketItem> BucketItems { get; set; }

    }
}