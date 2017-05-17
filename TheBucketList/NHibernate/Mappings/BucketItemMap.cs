using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheBucketList.NHibernate.Entities;
using FluentNHibernate.Mapping;

namespace TheBucketList.NHibernate.Mappings
{
    public class BucketItemMap : ClassMap<BucketItem>
    {
        public BucketItemMap()
        {
            Id(x => x.Id);
            Map(x => x.Ime);
            Map(x => x.Ostvareno);
            Map(x => x.Slika)
                .CustomSqlType("varbinary(max)")
                .Length(int.MaxValue);
            Map(x => x.Opis);
            References(x => x.Korisnik)
                .Column("userId");
            References(x => x.Kategorija)
                .Column("kategorijaId");
        }
    }
}