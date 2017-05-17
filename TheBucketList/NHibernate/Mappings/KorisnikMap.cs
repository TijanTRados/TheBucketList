using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheBucketList.NHibernate.Entities;
using FluentNHibernate.Mapping;

namespace TheBucketList.NHibernate.Mappings
{
    public class KorisnikMap : ClassMap<Korisnik>
    {
        public KorisnikMap()
        {
            Id(x => x.Id);
            Map(x => x.Username);
            Map(x => x.Lozinka);
            Map(x => x.Ime);
            Map(x => x.Prezime);
            Map(x => x.Slika)
                .CustomSqlType("varbinary(max)")
                .Length(int.MaxValue); 
            Map(x => x.Moto);
            Map(x => x.Opis);
            HasMany(x => x.BucketItems)
                .KeyColumn("userId");
        }
    }
}