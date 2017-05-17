using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using TheBucketList.Models;
using TheBucketList.NHibernate;
using TheBucketList.NHibernate.Entities;

namespace TheBucketList.Controllers
{
    public class ItemsAPIController : ApiController
    {
        private HttpResponseMessage response;
        // GET: api/ItemsAPI
        [HttpGet]
        [Route("api/items")]
        public HttpResponseMessage Get()
        {
            var bucketItemsModel = new BucketItemsModel();
            using (var session = FluentNHibernateHelper.OpenSession())
            {
                List<BucketItem> bucketItems = (List<BucketItem>)session.QueryOver<BucketItem>().List();

                foreach (var item in bucketItems)
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
                response = Request.CreateResponse(HttpStatusCode.OK, bucketItemsModel.BucketItems);
                return response;
            }
        }

        [HttpGet]
        // GET: api/ItemsAPI/5
        [Route("api/items/{id}")]
        public HttpResponseMessage Get(int id)
        {
            var bucketItemsModel = new BucketItemsModel();
            using (var session = FluentNHibernateHelper.OpenSession())
            {
                List<BucketItem> bucketItems = (List<BucketItem>)session.QueryOver<BucketItem>()
                    .Where(k => k.Korisnik.Id == id).List();

                foreach (var item in bucketItems)
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
                response = Request.CreateResponse(HttpStatusCode.OK, bucketItemsModel.BucketItems);
                return response;
            }
        }

        [HttpPut]
        [Route("api/items/update/{id}")]
        public HttpResponseMessage Update([FromBody]BucketItemModel model, int id)
        {
            try
            {
                BucketItem bucketItem = new BucketItem();

                using (var session = FluentNHibernateHelper.OpenSession())
                {
                    bucketItem = ((List<BucketItem>)session.QueryOver<BucketItem>()
                             .Where(k => k.Id == model.Id).List()).ElementAt(0);

                    bucketItem.Ime = model.Ime;
                    bucketItem.Opis = model.Opis;
                    if (model.Slika == null) bucketItem.Slika = null;
                    else bucketItem.Slika = model.Slika;

                    List<Kategorija> kategorija = (List<Kategorija>)session.QueryOver<Kategorija>()
                        .Where(k => k.Naziv == model.KategorijaNaziv).List();
                    bucketItem.Kategorija = kategorija[0];

                    session.Update(bucketItem);
                    session.Flush();
                }
                response = Get(id);
                return response;
            }
            catch
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "Ne");
                return response;
            }
        }

        [HttpPost]
        // POST: api/ItemsAPI
        [Route("api/items/create/{id}")]
        public HttpResponseMessage Create([FromBody]BucketItemModel model, int id)
        {
            try
            {
                BucketItem bucketItem = new BucketItem();
                bucketItem.Ime = model.Ime;
                bucketItem.Ostvareno = false;
                if (model.Slika == null) bucketItem.Slika = null;
                else bucketItem.Slika = model.Slika;
                bucketItem.Opis = model.Opis;

                Korisnik kor = new Korisnik();

                using (var session = FluentNHibernateHelper.OpenSession())
                {
                    kor = ((List<Korisnik>)session.QueryOver<Korisnik>()
                            .Where(k => k.Id == id).List()).ElementAt(0);

                    bucketItem.Korisnik = kor;

                    List<Kategorija> kategorija = (List<Kategorija>)session.QueryOver<Kategorija>()
                        .Where(k => k.Naziv == model.KategorijaNaziv).List();
                    bucketItem.Kategorija = kategorija[0];
                    session.SaveOrUpdate(bucketItem);
                }

                response = Get(id);
                return response;
            }
            catch
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "Ne");
                return response;
            }
        }

        [HttpGet]
        [Route("api/items/ostvareno/{id}")]
        public HttpResponseMessage changeOstvareno(int bucketItemId, int id)
        {

            BucketItem bucketItem = new BucketItem();

            using (var session = FluentNHibernateHelper.OpenSession())
            {
                bucketItem = ((List<BucketItem>)session.QueryOver<BucketItem>()
                        .Where(k => k.Id == bucketItemId).List()).ElementAt(0);
                if (bucketItem.Ostvareno == true)
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
            response = Get(id);
            return response;
        }


        // PUT: api/ItemsAPI/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ItemsAPI/5
        public void Delete(int id)
        {
        }
    }
}
