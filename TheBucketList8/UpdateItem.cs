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
using Android;
using System.Net;
using System.IO;
using System.Json;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using System.Net.Http;
using Newtonsoft.Json;

namespace TheBucketList8
{
    [Activity(Label = "UpdateItem", Theme = "@style/Theme.Bucketlist")]
    public class UpdateItem : Activity
    {
        public LinearLayout mainLayout, layout;
        public ImageView pic;
        private string kategorija;
        private byte[] slika;
        public HttpClient client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            BucketItem model = new BucketItem();

            model.Id = Intent.GetIntExtra("itemId", 0);
            model.Ime = Intent.GetStringExtra("ime");
            model.KategorijaNaziv = Intent.GetStringExtra("kategorija");
            model.Opis = Intent.GetStringExtra("opis");
            model.Slika = Intent.GetByteArrayExtra("slika");
            model.koliko = Intent.GetIntExtra("koliko", 0);
            int userId = Intent.GetIntExtra("userId", 0);

            //Kreiranje osnovnog scroll view-a
            var scrollView = new ScrollView(this)
            {
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
            };
            SetContentView(scrollView);

            //Kreiranje glavnog layouta (koji je dio scroll view-a)
            mainLayout = new LinearLayout(this)
            {
                Orientation = Orientation.Vertical,
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
            };
            mainLayout.SetBackgroundColor(Color.White);
            mainLayout.SetPadding(30, 30, 30, 30);
            scrollView.AddView(mainLayout);

            EditText naziv = new EditText(this);
            naziv.Text = model.Ime;
            naziv.Hint = "Naziv";
            mainLayout.AddView(naziv);

            pic = new ImageView(this);
            LinearLayout.LayoutParams LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            pic.LayoutParameters = LayoutParameters;

            if (model.Slika != null)
            {
                DisplayMetrics mets = new DisplayMetrics();
                WindowManager.DefaultDisplay.GetMetrics(mets);
                var bmp = BitmapFactory.DecodeByteArray(model.Slika, 0, model.Slika.Length);
                double viewWidthToBitmapWidthRatio = (double)mets.WidthPixels / (double)bmp.Width;
                pic.LayoutParameters.Height = (int)(bmp.Height * viewWidthToBitmapWidthRatio);
                pic.SetImageBitmap(bmp);
                pic.SetScaleType(ImageView.ScaleType.FitCenter);
            }

            mainLayout.AddView(pic);

            Button picbutt = new Button(this);
            picbutt.Text = "Odaberi sliku";
            picbutt.SetPadding(20, 30, 20, 30);
            mainLayout.AddView(picbutt);

            picbutt.Click += delegate {
                var imageIntent = new Intent();
                imageIntent.SetType("image/*");
                imageIntent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(
                    Intent.CreateChooser(imageIntent, "Select photo"), 0);
            };

            Spinner spinner = new Spinner(this);
            string[] kategorije = { "Travel", "Sport", "Education", "Fun", "Extreme", "Career", "Hobby", "Family", "Charity" };

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.kategorija_array, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            spinner.SetSelection(adapter.GetPosition(model.KategorijaNaziv));

            mainLayout.AddView(spinner);

            EditText opis = new EditText(this);
            opis.Hint = "Opis";
            LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            opis.InputType = Android.Text.InputTypes.TextVariationLongMessage;
            opis.LayoutParameters = LayoutParameters;
            opis.Text = model.Opis;
            mainLayout.AddView(opis);

            Button spremi = new Button(this);
            spremi.SetPadding(20, 30, 20, 60);
            spremi.Text = "Spremi";
            mainLayout.AddView(spremi);

            spremi.Click += (sender1, e) =>
            {
                model.Ime = naziv.Text;
                model.KategorijaNaziv = kategorija;
                model.Opis = opis.Text;
                model.Ostvareno = false;
                if (slika != null) { model.Slika = slika; }

                client = new HttpClient();
                conn(model, userId);
            };
        }

        public async void conn(BucketItem item, int id)
        {
            await SaveTodoItemAsync(item, id);
        }

        //Post data to passed URL
        public async Task SaveTodoItemAsync(BucketItem item, int id)
        {
            string uri;
            uri = "http://bucketlist.ddns.net/api/items/update/" + id;

            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await client.PutAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                Toast.MakeText(this, "Uspješno izmjenjeno!", ToastLength.Short).Show();
            }
            else Toast.MakeText(this, "Greška", ToastLength.Short).Show();
        }



        //[Spinner helper] selecting value from dropdown list
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            kategorija = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
        }

        //[Image upload] fetching image from mobile repository and parsing it into byte[]
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                pic.SetImageURI(data.Data);

                Android.Graphics.Bitmap mBitmap = null;
                mBitmap = Android.Provider.MediaStore.Images.Media.GetBitmap(this.ContentResolver, data.Data);
                using (var stream = new MemoryStream())
                {
                    mBitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                    slika = stream.ToArray();
                }
            }
        }
    }
}