using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using TheBucketList.NHibernate.Entities;

namespace TheBucketList.NHibernate
{
    public static class FluentNHibernateHelper
    {
        public static ISession OpenSession()
        {
            string connectionString = "Data Source=localhost;" +
                "User id=bucketlist;" +
                "Password=Tijan1993;";
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString)
                .ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Korisnik>().AddFromAssemblyOf<BucketItem>()
                .AddFromAssemblyOf<Kategorija>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                .Create(false, false))
                .BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}