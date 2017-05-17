using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheBucketList.Models;
using TheBucketList.NHibernate;
using TheBucketList.NHibernate.Entities;

namespace TheBucketList.Controllers
{
    public class KorisnikController : Controller
    {
        // GET: Korisnik
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult usernameSubmit(string newUsername)
        {
            Korisnik korisnik = (Korisnik)Session["User"];
            korisnik.Username = newUsername;
            Session["User"] = korisnik;

            using (var session = FluentNHibernateHelper.OpenSession())
            {
                session.Update(korisnik);
                session.Flush();
            }
            return RedirectToAction("Details");
        }

        public ActionResult changeOstvareno(string ostvareno, int bucketItemId)
        {

            BucketItem bucketItem = new BucketItem();

            using (var session = FluentNHibernateHelper.OpenSession())
            {
                bucketItem = ((List<BucketItem>)session.QueryOver<BucketItem>()
                        .Where(k => k.Id == bucketItemId).List()).ElementAt(0);
                    if (ostvareno == "nijeOstvareno")
                    {
                        bucketItem.Ostvareno = false;
                    }
                    else
                    {
                        bucketItem.Ostvareno = true;
                    }
                session.Update(bucketItem);
                session.Flush();
            }
            return RedirectToAction("BucketItems");
        }

        public ActionResult ChangeBucketItem(int itemId)
        {

            BucketItem bucketItem = new BucketItem();
            BuketListModelWithoutPicture buketListModel = new BuketListModelWithoutPicture();

            using (var session = FluentNHibernateHelper.OpenSession())
            {
               bucketItem = ((List<BucketItem>)session.QueryOver<BucketItem>()
                        .Where(k => k.Id == itemId).List()).ElementAt(0);

                buketListModel.Id = bucketItem.Id;
                buketListModel.Ime = bucketItem.Ime;
                buketListModel.KategorijaId = bucketItem.Kategorija.Id.ToString();
                buketListModel.Opis = bucketItem.Opis;
                buketListModel.Ostvareno = bucketItem.Ostvareno;
                List<Kategorija> kategorije = (List<Kategorija>)session
                    .QueryOver<Kategorija>()
                    .List();
                foreach (var kategorija in kategorije)
                {
                    SelectListItem selectListItem = new SelectListItem();
                    selectListItem.Text = kategorija.Naziv;
                    selectListItem.Value = kategorija.Id.ToString();
                    buketListModel.kategorijaItems.Add(selectListItem);
                }

            }
            return View(buketListModel);
        }

        [HttpPost]
        public ActionResult ChangeBucketItem(BuketListModelWithoutPicture bucketModel)
        {

            BucketItem bucketItem = new BucketItem();

            using (var session = FluentNHibernateHelper.OpenSession())
            {
                bucketItem = ((List<BucketItem>)session.QueryOver<BucketItem>()
                         .Where(k => k.Id == bucketModel.Id).List()).ElementAt(0);
                bucketItem.Ime = bucketModel.Ime;
                bucketItem.Opis = bucketModel.Opis;
                bucketItem.Ostvareno = bucketModel.Ostvareno;
                List<Kategorija> kategorija = (List<Kategorija>)session.QueryOver<Kategorija>()
                    .Where(k => k.Id == int.Parse(bucketModel.KategorijaId)).List();
                bucketItem.Kategorija = kategorija[0];
                session.Update(bucketItem);
                session.Flush();
            }
            return RedirectToAction("BucketItems");
        }

        public ActionResult lozinkaSubmit(string newLozinka)
        {
            Korisnik korisnik = (Korisnik)Session["User"];
            korisnik.Lozinka = newLozinka;
            Session["User"] = korisnik;

            using (var session = FluentNHibernateHelper.OpenSession())
            {
                session.Update(korisnik);
                session.Flush();
            }
            return RedirectToAction("Details");
        }

        public ActionResult imeSubmit(string newIme)
        {
            Korisnik korisnik = (Korisnik)Session["User"];
            korisnik.Ime = newIme;
            Session["User"] = korisnik;

            using (var session = FluentNHibernateHelper.OpenSession())
            {
                session.Update(korisnik);
                session.Flush();
            }
            return RedirectToAction("Details");
        }

        public ActionResult prezimeSubmit(string newPrezime)
        {
            Korisnik korisnik = (Korisnik)Session["User"];
            korisnik.Prezime = newPrezime;
            Session["User"] = korisnik;

            using (var session = FluentNHibernateHelper.OpenSession())
            {
                session.Update(korisnik);
                session.Flush();
            }
            return RedirectToAction("Details");
        }

        public ActionResult motoSubmit(string newMoto)
        {
            Korisnik korisnik = (Korisnik)Session["User"];
            korisnik.Moto = newMoto;
            Session["User"] = korisnik;

            using (var session = FluentNHibernateHelper.OpenSession())
            {
                session.Update(korisnik);
                session.Flush();
            }
            return RedirectToAction("Details");
        }

        public ActionResult opisSubmit(string newOpis)
        {
            Korisnik korisnik = (Korisnik)Session["User"];
            korisnik.Opis = newOpis;
            Session["User"] = korisnik;

            using (var session = FluentNHibernateHelper.OpenSession())
            {
                session.Update(korisnik);
                session.Flush();
            }
            return RedirectToAction("Details");
        }

        // Here I removed Id parameter
        // GET: Korisnik/Details
        public ActionResult Details()
        {
            Korisnik prviKorisnik = (Korisnik)Session["User"];
            KorisnikModel korisnikModel = new KorisnikModel();
                
            korisnikModel.Id = prviKorisnik.Id;
            korisnikModel.Ime = prviKorisnik.Ime;
            korisnikModel.Prezime = prviKorisnik.Prezime;
            korisnikModel.Lozinka = prviKorisnik.Lozinka;
            korisnikModel.Moto = prviKorisnik.Moto;
            korisnikModel.Opis = prviKorisnik.Opis;
            korisnikModel.Slika = prviKorisnik.Slika;
            korisnikModel.Username = prviKorisnik.Username;
          
            return View(korisnikModel);
        }

        // GET: Korisnik/Create
        public ActionResult Create()
        {
            BuketListModel bucketListModel = new BuketListModel();
            using (var session = FluentNHibernateHelper.OpenSession())
            {
                List<Kategorija> kategorije = (List<Kategorija>)session
                    .QueryOver<Kategorija>()
                    .List();
                foreach (var kategorija in kategorije)
                {
                    SelectListItem selectListItem = new SelectListItem();
                    selectListItem.Text = kategorija.Naziv;
                    selectListItem.Value = kategorija.Id.ToString();
                    bucketListModel.kategorijaItems.Add(selectListItem);
                }
            }
            return View(bucketListModel);
        }

        // POST: Korisnik/Create
        [HttpPost]
        public ActionResult Create(BuketListModel model)
        {
            try
            {
                BucketItem bucketItem = new BucketItem();
                bucketItem.Ime = model.Ime;
                bucketItem.Ostvareno = model.Ostvareno;

                MemoryStream target = new MemoryStream();
                model.Slika.InputStream.CopyTo(target);
                byte[] data = target.ToArray();
                bucketItem.Slika = data;

                bucketItem.Opis = model.Opis;
                bucketItem.Korisnik = (Korisnik)Session["User"];

                using (var session = FluentNHibernateHelper.OpenSession())
                {
                    List<Kategorija> kategorija = (List<Kategorija>)session.QueryOver<Kategorija>()
                        .Where(k => k.Id == int.Parse(model.KategorijaId)).List();
                    bucketItem.Kategorija = kategorija[0];
                    session.SaveOrUpdate(bucketItem);
                }

                    return RedirectToAction("BucketItems", "Korisnik");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult BucketItems()
        {
            var bucketItemsModel = new BucketItemsModel();
            using (var session = FluentNHibernateHelper.OpenSession())
            {
                List<BucketItem> bucketItems = (List<BucketItem>)session.QueryOver<BucketItem>()
                    .Where(k => k.Korisnik.Id == ((Korisnik)Session["User"]).Id).List();
                
                foreach(var item in bucketItems)
                {
                    var itemModel = new BucketItemModel();
                    itemModel.Id = item.Id;
                    itemModel.Ime = item.Ime;
                    itemModel.Ostvareno = item.Ostvareno;
                    itemModel.Slika = item.Slika;
                    itemModel.Opis = item.Opis;
                    itemModel.KategorijaNaziv = item.Kategorija.Naziv;
                    bucketItemsModel.BucketItems.Add(itemModel);
                }
            }

            return View(bucketItemsModel);
        }

        public ActionResult BucketItemsMobile()
        {
            var bucketItemsModel = new BucketItemsModel();
            using (var session = FluentNHibernateHelper.OpenSession())
            {
                List<BucketItem> bucketItems = (List<BucketItem>)session.QueryOver<BucketItem>().List();

                return Json(bucketItems);
            }
        }

        [HttpPost]
        public ActionResult ChangeImage(ImageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // the user didn't upload any file =>
                // render the same view again in order to display the error message
                return RedirectToAction("Details", "Korisnik");
            }
            Korisnik korisnik = (Korisnik)Session["User"];

            MemoryStream target = new MemoryStream();
            model.File.InputStream.CopyTo(target);
            byte[] data = target.ToArray();

            korisnik.Slika = data;

            using (var session = FluentNHibernateHelper.OpenSession())
            {
                session.Update(korisnik);
                session.Flush();
            }

            return RedirectToAction("Details", "Korisnik");
            
        }

        // GET: Korisnik/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Korisnik/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Korisnik/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Korisnik/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
