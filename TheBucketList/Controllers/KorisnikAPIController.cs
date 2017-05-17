using System;
using System.Collections.Generic;
using System.Drawing;
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
    public class KorisnikAPIController : ApiController
    {
        private HttpResponseMessage response;
        // GET: api/KorisnikAPI
        [Route("api/svi")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/KorisnikAPI/5
        [Route("api/korisnik")]
        public HttpResponseMessage Get(string username, string password)
        {
            Korisnik korisnik = new Korisnik();
            Korisnik prazan = new Korisnik();

            using (var session = FluentNHibernateHelper.OpenSession())
            {
                try
                {
                    korisnik = ((List<Korisnik>)session.QueryOver<Korisnik>()
                        .Where(k => k.Username == username).List()).ElementAt(0);

                }
                catch (Exception e)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, prazan);
                    return response;
                }
            }

            if (korisnik.Lozinka == password)
            {
                var novi = new KorisnikModel();
                novi.Id = korisnik.Id;
                novi.Ime = korisnik.Ime;
                novi.Lozinka = korisnik.Lozinka;
                novi.Moto = korisnik.Moto;
                novi.Opis = korisnik.Opis;
                novi.Prezime = korisnik.Prezime;
                novi.Slika = korisnik.Slika;
                novi.Username = korisnik.Username;

                response = Request.CreateResponse(HttpStatusCode.OK, novi);
                return response;
            }

            else
            {
                response = Request.CreateResponse(HttpStatusCode.OK, prazan);
                return response;
            }
        }

        [HttpPost]
        // POST: api/KorisnikAPI
        [Route("api/korisnik/register")]
        public HttpResponseMessage Post([FromBody]KorisnikModel Korisnik)
        {
            if (ModelState.IsValid)
            {
                Korisnik korisnik = new Korisnik();
                Image Slika;
                using (var session = FluentNHibernateHelper.OpenSession())
                {
                    korisnik.Ime = Korisnik.Ime; korisnik.Prezime = Korisnik.Prezime;
                    korisnik.Username = Korisnik.Username; korisnik.Moto = Korisnik.Moto;
                    korisnik.Opis = Korisnik.Opis; korisnik.Lozinka = Korisnik.Lozinka;
                    if (Korisnik.Slika == null)
                    {
                        //Slika = Image.FromFile(@"~/Content/Images/boyDefault.png");
                        
                        //using (var ms = new MemoryStream())
                        //{
                        //    Slika.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        //    korisnik.Slika = ms.ToArray();
                        //}
                    }
                    else korisnik.Slika = Korisnik.Slika;

                    session.SaveOrUpdate(korisnik);
                }

                var novi = new KorisnikModel();
                novi.Id = korisnik.Id;
                novi.Ime = korisnik.Ime;
                novi.Lozinka = korisnik.Lozinka;
                novi.Moto = korisnik.Moto;
                novi.Opis = korisnik.Opis;
                novi.Prezime = korisnik.Prezime;
                novi.Slika = korisnik.Slika;
                novi.Username = korisnik.Username;

                response = Request.CreateResponse(HttpStatusCode.OK, novi);
                return response;
            }
            Korisnik prazan = new Korisnik();
            response = Request.CreateResponse(HttpStatusCode.OK, prazan);
            return response;

        }

        // PUT: api/KorisnikAPI/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/KorisnikAPI/5
        public void Delete(int id)
        {
        }
    }
}
