using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheBucketList.NHibernate.Entities;

namespace TheBucketList.NHibernate.Mappings
{
    public class KategorijaMap : ClassMap<Kategorija>
    {
        public KategorijaMap()
        {
            Id(x => x.Id);
            Map(x => x.Naziv);
            HasMany(x => x.BucketItems)
                .KeyColumn("kategorijaId");
        }
    }
}