using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using Android.Graphics;

namespace TheBucketList8
{
    [Activity(Label = "Register")]
    public class Register : Activity
    {

        private EditText username, password, password2, name, surname, moto, about;
        private Button piccbutt, regg;
        private ImageView img;
        private byte[] slika;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.register);
            username = FindViewById<EditText>(Resource.Id.txtUsernameReg);
            password = FindViewById<EditText>(Resource.Id.txtZaporkaReg);
            password2 = FindViewById<EditText>(Resource.Id.txtPonoviReg);
            name = FindViewById<EditText>(Resource.Id.txtImeReg);
            surname = FindViewById<EditText>(Resource.Id.txtPrezimeReg);
            moto = FindViewById<EditText>(Resource.Id.txtMotoReg);
            about = FindViewById<EditText>(Resource.Id.txtOMeniReg);
            piccbutt = FindViewById<Button>(Resource.Id.btnSlika);
            regg = FindViewById<Button>(Resource.Id.btnReg);
            img = FindViewById<ImageView>(Resource.Id.imgReg);
            piccbutt.Click += delegate
            {
                var imageIntent = new Intent();
                imageIntent.SetType("image/*");
                imageIntent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(
                    Intent.CreateChooser(imageIntent, "Select photo"), 0);
            };

            regg.Click += delegate
            {
                if (username.Text.Equals(String.Empty) || password.Text.Equals(String.Empty) || password2.Text.Equals(String.Empty) || name.Text.Equals(String.Empty) || surname.Text.Equals(String.Empty))
                {
                    Toast.MakeText(this, "Prvih 5 polja su obavezna!", ToastLength.Short).Show();
                }
                else if (password.Text != password2.Text)
                {
                    Toast.MakeText(this, "Lozinka i ponovljena lozinka nisu iste!", ToastLength.Short).Show();
                }
                else
                {
                    KorisnikModelMob korisnik = new KorisnikModelMob();
                    korisnik.Username = username.Text;
                    korisnik.Ime = name.Text;
                    korisnik.Prezime = surname.Text;
                    korisnik.Lozinka = password.Text;
                    korisnik.Moto = moto.Text;
                    korisnik.Opis = about.Text;
                    if(slika != null && slika.Length >5)
                    {
                        korisnik.Slika = slika;
                    }
                    connection(korisnik);
                    Intent activity = new Intent(this, typeof(Login));
                    StartActivity(activity);

                }
            };
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                img.SetImageURI(data.Data);

                Android.Graphics.Bitmap mBitmap = null;
                mBitmap = Android.Provider.MediaStore.Images.Media.GetBitmap(this.ContentResolver, data.Data);
                using (var stream = new MemoryStream())
                {
                    mBitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                    slika = stream.ToArray();
                }
            }
        }



        private async void connection(KorisnikModelMob korisnik)
        {
            await SaveTodoItemAsync(korisnik);
        }

        private async Task SaveTodoItemAsync(KorisnikModelMob korisnik)
        {
            HttpClient client = new HttpClient();
            string uri = "http://bucketlist.ddns.net/api/korisnik/register";

            var json = JsonConvert.SerializeObject(korisnik);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                Toast.MakeText(this, "Uspješno registriran novi korisnik!", ToastLength.Short).Show();
            }
            else Toast.MakeText(this, "Greška", ToastLength.Short).Show();

        }
    }
}