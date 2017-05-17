using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;

namespace TheBucketList.NHibernate.Entities
{
    public class Kategorija
    {
        public virtual int Id { get; protected set; }
        public virtual string Naziv { get; set; }
        public virtual IList<BucketItem> BucketItems { get; set; }
    }
}