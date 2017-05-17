using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheBucketList.NHibernate.Entities
{
    public class BucketItem
    {
        public virtual int Id { get; protected set; }
        public virtual string Ime { get; set; }
        public virtual bool Ostvareno { get; set; }
        public virtual byte[] Slika { get; set; }
        public virtual string Opis { get; set; }
        public virtual Korisnik Korisnik { get; set; }
        public virtual Kategorija Kategorija { get; set; }
    }
}