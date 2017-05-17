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
using Newtonsoft.Json;
namespace TheBucketListMobile7

//All bucket items
{
    [Activity(Icon = "@drawable/icon", Theme = "@style/Theme.Bucketlist", Label = "The Bucket List")]
    public class MainActivity : Activity
    {
        public List<BucketItem> items;
        public LinearLayout mainLayout, layout;
        public Korisnik user;
        public ImageView pic;
        private string kategorija;
        private byte[] slika;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            string url = Intent.GetStringExtra("url");
            login(url);

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            ActionBar.Tab tab = ActionBar.NewTab();
            tab.SetText(Resources.GetString(Resource.String.All));

            //PRVI TAB*************************************************************************
            tab.TabSelected += (sender, args) =>
            {

                items = new List<BucketItem>();
                var relative = new RelativeLayout(this)
                {
                    LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
                };
                this.SetContentView(relative);

                //Kreiranje osnovnog scroll view-a
                var scrollView = new ScrollView(this)
                {
                    LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
                };
                relative.AddView(scrollView);

                //Kreiranje glavnog layouta (koji je dio scroll view-a)
                mainLayout = new LinearLayout(this)
                {
                    Orientation = Orientation.Vertical,
                    LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
                };
                mainLayout.SetBackgroundColor(Color.WhiteSmoke);

                scrollView.AddView(mainLayout);

                // Dohvat podataka
                conn("http://ec2-34-249-76-120.eu-west-1.compute.amazonaws.com/api/items");
            };
            ActionBar.AddTab(tab);






            //DRUGI TAB*************************************************************************
            tab = ActionBar.NewTab();
            tab.SetText(Resources.GetString(Resource.String.Profile));
            tab.TabSelected += (sender, args) =>
            {
                items = new List<BucketItem>();
                var relative = new RelativeLayout(this)
                {
                    LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
                };
                this.SetContentView(relative);

                //Kreiranje osnovnog scroll view-a
                var scrollView = new ScrollView(this)
                {
                    LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
                };
                relative.AddView(scrollView);

                //Kreiranje glavnog layouta (koji je dio scroll view-a)
                mainLayout = new LinearLayout(this)
                {
                    Orientation = Orientation.Vertical,
                    LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
                };
                mainLayout.SetBackgroundColor(Color.WhiteSmoke);

                scrollView.AddView(mainLayout);

                TextView imekorisnika = new TextView(this);
                LinearLayout.LayoutParams LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 120);
                imekorisnika.LayoutParameters = LayoutParameters;
                imekorisnika.Text = user.imeprezime;
                imekorisnika.TextSize = 25;
                imekorisnika.SetPadding(60, 10, 10, 10);
                imekorisnika.SetTextColor(Color.White);
                imekorisnika.SetBackgroundColor(Color.DarkGray);
                mainLayout.AddView(imekorisnika);
    

                // Dohvat podataka
                conn("http://ec2-34-249-76-120.eu-west-1.compute.amazonaws.com/api/items/" + user.id);
            };
            ActionBar.AddTab(tab);







            //TREĆI TAB*************************************************************************
            tab = ActionBar.NewTab();
            tab.SetText(Resources.GetString(Resource.String.Add));
            tab.TabSelected += (sender, args) =>
            {
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
                naziv.Hint = "Naziv";
                mainLayout.AddView(naziv);

                pic = new ImageView(this);
                LinearLayout.LayoutParams LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                //LayoutParameters.SetMargins(30, 30, 30, 30);
                //pic.LayoutParameters = LayoutParameters;
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

                mainLayout.AddView(spinner);

                EditText opis = new EditText(this);
                opis.Hint = "Opis";
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                opis.InputType = Android.Text.InputTypes.TextVariationLongMessage;
                opis.LayoutParameters = LayoutParameters;
                mainLayout.AddView(opis);

                Button spremi = new Button(this);
                spremi.SetPadding(20, 30, 20, 60);
                spremi.Text = "Spremi";
                mainLayout.AddView(spremi);

                //spremi.Click += (sender1, e) =>
                //{
                //    BucketItemModel bModel = new BucketItemModel();
                //    bModel.Ime = naziv.Text;
                //    bModel.KategorijaNaziv = kategorija;
                //    bModel.Opis = opis.Text;
                //    bModel.Ostvareno = false;
                //    bModel.Slika = slika;
                //};

            };
            ActionBar.AddTab(tab);
        }


        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            kategorija = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
        }

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

        public async void conn(string url)
        {
            JsonValue response = await RefreshDataAsync(url);
            ParseAndDisplay(response);
        }

        //Getting the data from passed URL
        public async Task<JsonValue> RefreshDataAsync(string url)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                // Get a stream representation of the HTTP web response:
                using (System.IO.Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                    // Return the JSON document:
                    return jsonDoc;
                }
            }
        }

        //public async Task SaveTodoItemAsync(BucketItemModel item, int id)
        //{
        //    // RestUrl = http://developer.xamarin.com:8081/api/todoitems{0}
        //    var uri = new Uri(string.Format("http://ec2-34-249-76-120.eu-west-1.compute.amazonaws.com/api/create/", id));

        //    var json = JsonConvert.SerializeObject(item);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = null;
        //    if (isNewItem)
        //    {
        //        response = await client.PostAsync(uri, content);
        //    }


        //    if (response.IsSuccessStatusCode)
        //    {

        //    }

        //}


        private void ParseAndDisplay(JsonValue json)
        {
            //Parsiranje iz JSON-a u model BucketItem te nakon toga u listu BucketItema
            foreach (JsonValue vrijednost in json)
            {
                BucketItem item = new BucketItem();
                item.Id = vrijednost["Id"];
                item.Ime = vrijednost["Ime"];
                if (vrijednost["Slika"] == null) item.Slika = null;
                else item.Slika = Base64.Decode(vrijednost["Slika"], Base64Flags.Default);
                item.Opis = vrijednost["Opis"];
                item.KategorijaNaziv = vrijednost["KategorijaNaziv"];
                item.Ostvareno = vrijednost["Ostvareno"];
                items.Add(item);
            }

            //Dodavanje na layout
            foreach (BucketItem item in items)
            {
                LinearLayout.LayoutParams LayoutParameters;

                //Kreiranje novog Layouta
                layout = new LinearLayout(this);
                layout.Orientation = Orientation.Vertical;
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                LayoutParameters.SetMargins(0, 30, 0, 30);
                layout.LayoutParameters = LayoutParameters;
                
                layout.SetBackgroundColor(Color.White);
                layout.SetPadding(40, 40, 40, 40);

                //Kreiranje imena
                var imeLabel = new TextView(this);
                imeLabel.SetTextSize(ComplexUnitType.Px, 80);
                imeLabel.Text = item.Ime;
                imeLabel.SetTextColor(Color.LightSlateGray);
                layout.AddView(imeLabel);

                //Kreiranje kategorije
                var kategorijaLabel = new TextView(this);
                kategorijaLabel.Text = "Kategorija: " + item.KategorijaNaziv;
                kategorijaLabel.SetTextColor(Color.LightGray);
                if (item.Slika == null) kategorijaLabel.SetPadding(30, 0, 0, 30);
                else kategorijaLabel.SetPadding(30, 0, 0, 0);
                layout.AddView(kategorijaLabel);

                //Kreiranje slike
                var slikaView = new ImageView(this);
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                slikaView.LayoutParameters = LayoutParameters;

                if (item.Slika != null)
                {
                    DisplayMetrics mets = new DisplayMetrics();
                    WindowManager.DefaultDisplay.GetMetrics(mets);
                    var bmp = BitmapFactory.DecodeByteArray(item.Slika, 0, item.Slika.Length);
                    double viewWidthToBitmapWidthRatio = (double)mets.WidthPixels / (double)bmp.Width;
                    slikaView.LayoutParameters.Height = (int)(bmp.Height * viewWidthToBitmapWidthRatio);
                    slikaView.SetImageBitmap(bmp);
                    slikaView.SetScaleType(ImageView.ScaleType.FitCenter);
                }
                layout.AddView(slikaView);

                /*
                if (item.Slika == null)
                {
                    //Crta
                    var crta = new LinearLayout(this);
                    LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 10);
                    crta.LayoutParameters = LayoutParameters;
                    crta.SetBackgroundColor(Color.LightGray);
                    layout.AddView(crta);
                    
                }
                */

                //Kreiranje opisa
                var opisLabel = new TextView(this);
                opisLabel.Text = item.Opis;
                opisLabel.SetPadding(0, 20, 0, 20);
                opisLabel.SetTextColor(Color.LightSlateGray);
                layout.AddView(opisLabel);

                //Kreiranje ostvarenosti
                var ostvarenoSlika = new ImageView(this);
                if (item.Ostvareno == true) ostvarenoSlika.SetImageResource(Resource.Drawable.ostvarenoTRUE);
                else ostvarenoSlika.SetImageResource(Resource.Drawable.ostvarenoFALSE);
                LayoutParameters = new LinearLayout.LayoutParams(100, 100);
                ostvarenoSlika.LayoutParameters = LayoutParameters;

                var ostvarenoLabel = new TextView(this);
                ostvarenoLabel.SetTextColor(Color.LightGray);
                if (item.Ostvareno == true) ostvarenoLabel.Text = "Ti i još 32 ljudi je ostvarilo ovo";
                else ostvarenoLabel.Text = "32 ljudi je ostvarilo ovo";
                ostvarenoLabel.SetPadding(10, 25, 0, 0);

                var grupa = new LinearLayout(this)
                {
                    Orientation = Orientation.Horizontal,
                    LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 100)
                };

                grupa.AddView(ostvarenoSlika);
                grupa.AddView(ostvarenoLabel);
                layout.AddView(grupa);

                if (item.Ostvareno == false)
                {
                    //Kreiranje gumba za dodavanje
                    var dodajButton = new Button(this);
                    dodajButton.Text = "+ Dodaj";
                    dodajButton.SetTextColor(Color.White);
                    dodajButton.SetTextSize(ComplexUnitType.Px, 40);
                    dodajButton.Click += (sender, e) =>
                    { Toast.MakeText(this, "Dodano na bucket listu!", ToastLength.Short).Show(); };
                    layout.AddView(dodajButton);
                }

                //Dodavanje kreiranog layouta na glavni layout
                mainLayout.AddView(layout);
            }
        }

        private async void login(string url)
        {
            JsonValue response = await RefreshDataAsync(url);
            UserInfo(response);
        }

        private void UserInfo (JsonValue json)
        {
            user = new TheBucketListMobile7.Korisnik();
            user.ime = json["Ime"];
            user.imeprezime = json["FullName"];
            user.prezime = json["Prezime"];
            //if (json["Slika"] == "null") user.slika = 
            user.slika = Base64.Decode(json["Slika"], Base64Flags.Default);
            user.username = json["Username"];
            user.password = json["Lozinka"];
            user.motto = json["Moto"];
            user.opis = json["Opis"];
            user.id = json["Id"];
        }
    }
}
