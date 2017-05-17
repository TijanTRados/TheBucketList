using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheBucketList.NHibernate;
using TheBucketList.NHibernate.Entities;

namespace TheBucketList.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                Session["LogIn"] = false;
            }
            return View();
        }

        public ActionResult About()
        {
            Korisnik prviKorisnik;
            using (var session = FluentNHibernateHelper.OpenSession())
            { 
                List<Korisnik> korisnik = (List<Korisnik>)session.QueryOver<Korisnik>()
                    .Where(k => k.Id == 4).List();
                prviKorisnik = korisnik.ElementAt(0);
                BucketItem bucketItem = prviKorisnik.BucketItems[0];
            }
            
            ViewBag.Message = "Your application username: " + prviKorisnik.Username;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}