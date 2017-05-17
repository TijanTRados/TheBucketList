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

//MAIN ACTIVITY - ALL TABS ***********************************************************************************
{
    [Activity(Icon = "@drawable/icon", Theme = "@style/Theme.Bucketlist", Label = "The Bucket List")]
    public class MainActivity : Activity
    {
        //PUBLIC VARIABLES .......................................

        public List<BucketItem> allitems, myitems;
        public LinearLayout mainLayout, layout;
        public Korisnik user;
        public ImageView pic;
        private string kategorija;
        private byte[] slika;
        public HttpClient client;

        //........................................................

//ON CREATE() ***********************************************************************************************

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            string url = Intent.GetStringExtra("url");
            login(url);


//PRVI TAB - HOMEPAGE ***************************************************************************************

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            ActionBar.Tab tab = ActionBar.NewTab();
            tab.SetText(Resources.GetString(Resource.String.All));
            tab.TabSelected += (sender, args) =>
            {
                allitems = new List<BucketItem>();
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
                conn("http://bucketlist.ddns.net/api/items", 0);
            };
            ActionBar.AddTab(tab);


//DRUGI TAB - PROFILE ***************************************************************************************

            tab = ActionBar.NewTab();
            tab.SetText(Resources.GetString(Resource.String.Profile));
            tab.TabSelected += (sender, args) =>
            {
                myitems = new List<BucketItem>();
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

                LinearLayout podaci = new LinearLayout(this)
                {
                    LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent),
                    Orientation = Orientation.Vertical
                };

                podaci.SetBackgroundColor(Color.DarkSeaGreen);
                podaci.SetPadding(20, 20, 20, 20);

                var slikakorisnik = new ImageView(this);
                var bmp1 = BitmapFactory.DecodeByteArray(user.slika, 0, user.slika.Length);
                var bmp = getRoundedShape(bmp1);
                slikakorisnik.SetImageBitmap(bmp);
                LinearLayout.LayoutParams LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 402);
                slikakorisnik.LayoutParameters = LayoutParameters;
                //slikakorisnik.SetScaleType(ImageView.ScaleType.FitCenter);
                podaci.AddView(slikakorisnik);

                TextView imekorisnika = new TextView(this);
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                imekorisnika.LayoutParameters = LayoutParameters;
                imekorisnika.Text = user.imeprezime;
                imekorisnika.TextSize = 25;
                imekorisnika.SetTextColor(Color.White);
                imekorisnika.Gravity = GravityFlags.Center;
                podaci.AddView(imekorisnika);

                mainLayout.AddView(podaci);

                LinearLayout podaci2 = new LinearLayout(this);
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                podaci2.Orientation = Orientation.Vertical;
                LayoutParameters.SetMargins(0, 0, 0, 20);
                podaci2.SetPadding(40, 40, 40, 40);
                podaci2.SetBackgroundColor(Color.WhiteSmoke);
                podaci2.LayoutParameters = LayoutParameters;

                TextView motolabel = new TextView(this);
                motolabel.Text = "Moto";
                motolabel.SetTextColor(Color.LightSlateGray);
                motolabel.SetTypeface(Typeface.SansSerif, TypefaceStyle.Italic);
                motolabel.LayoutParameters = LayoutParameters;
                motolabel.Gravity = GravityFlags.Center;
                podaci2.AddView(motolabel);

                TextView moto = new TextView(this);
                moto.Text = user.motto;
                moto.SetTextColor(Color.LightSlateGray);
                moto.LayoutParameters = LayoutParameters;
                moto.Gravity = GravityFlags.Center;
                podaci2.AddView(moto);

                TextView opislabel = new TextView(this);
                opislabel.Text = "Opis";
                opislabel.SetTextColor(Color.LightSlateGray);
                opislabel.SetTypeface(Typeface.SansSerif, TypefaceStyle.Italic);
                opislabel.LayoutParameters = LayoutParameters;
                opislabel.Gravity = GravityFlags.Center;
                podaci2.AddView(opislabel);

                TextView opis = new TextView(this);
                opis.Text = user.opis;
                opis.SetTextColor(Color.LightSlateGray);
                opis.LayoutParameters = LayoutParameters;
                opis.Gravity = GravityFlags.Center;
                podaci2.AddView(opis);
                
                mainLayout.AddView(podaci2);
    

                // Dohvat podataka
                conn("http://bucketlist.ddns.net/api/items/" + user.id, 1);
            };
            ActionBar.AddTab(tab);


//TREĆI TAB - CREATE/EDIT**************************************************************************************************

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
                mainLayout.AddView(pic);
                slika = null;

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

                spremi.Click += (sender1, e) =>
                {
                    BucketItem bModel = new BucketItem();
                    bModel.Ime = naziv.Text;
                    bModel.KategorijaNaziv = kategorija;
                    bModel.Opis = opis.Text;
                    bModel.Ostvareno = false;
                    bModel.Slika = slika;

                    client = new HttpClient();
                    conn2(bModel, user.id);
                };
            };
            ActionBar.AddTab(tab);
        } // --> KRAJ ONCREATE() METODE


//SPINNER / IMAGEUPLOADER CONFIG ****************************************************************************

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

        //[ImageView convert to rounded ImageView]
        public Bitmap getRoundedShape(Bitmap scaleBitmapImage)
        {
            int targetWidth = 400;
            int targetHeight = 400;
            Bitmap targetBitmap = Bitmap.CreateBitmap(targetWidth,
                targetHeight, Bitmap.Config.Argb8888);

            Canvas canvas = new Canvas(targetBitmap);
            Android.Graphics.Path path = new Android.Graphics.Path();
            path.AddCircle(((float)targetWidth - 1) / 2,
                ((float)targetHeight - 1) / 2,
                (Math.Min(((float)targetWidth),
                    ((float)targetHeight)) / 2),
                Android.Graphics.Path.Direction.Ccw);

            canvas.ClipPath(path);
            Bitmap sourceBitmap = scaleBitmapImage;
            canvas.DrawBitmap(sourceBitmap,
                new Rect(0, 0, sourceBitmap.Width,
                    sourceBitmap.Height),
                new Rect(0, 0, targetWidth, targetHeight), null);
            return targetBitmap;
        }

        //CONNECTIONS************************************************************************************************

        //Basic connection to URL (allormine value=0 -> FETCH ALL value=1 ->FETCH BY USERID)
        public async void conn(string url, int allormine)
        {
            JsonValue response = await RefreshDataAsync(url);
            Parse(response, allormine);
            Display(allormine);
        }

        //Init user's bucket
        public async void Init(string url)
        {
            JsonValue response = await RefreshDataAsync(url);
            Parse(response, 1);
        }

        //Connection for saving data to database
        public async void conn2(BucketItem item, int id)
        {
            await SaveTodoItemAsync(item, id);
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

        //Post data to passed URL
        public async Task SaveTodoItemAsync(BucketItem item, int id)
        {
            string uri;
            uri = "http://bucketlist.ddns.net/api/items/create/" + id; 

            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            
            response = await client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                Toast.MakeText(this, "Uspješno kreirano!", ToastLength.Short).Show();
            }
            else Toast.MakeText(this, "Greška", ToastLength.Short).Show();

        }

 //PARSE AND DISPLAY METHODS**********************************************************************************

        //PARSE ...................................................................................
        private void Parse(JsonValue json, int allormine)
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
                item.koliko = 1;

                switch (allormine)
                {
                    case 0:
                        {
                            bool postoji = false;
                            foreach (BucketItem redak in allitems)
                            {
                                if (item.Ime == redak.Ime)
                                {
                                    redak.koliko++;
                                    postoji = true;
                                }
                            }
                            if (!postoji) allitems.Add(item);
                            break;
                        }
                    case 1:
                        {
                            myitems.Add(item);
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        //DISPLAY .................................................................................
        private void Display(int allormine) {

            List<BucketItem> show = new List<BucketItem>();
            switch (allormine)
            {
                case 0:
                    show = allitems;
                    break;
                case 1:
                    show = myitems;
                    break;
                default: break;
            }

            //Slučaj novog korisnika
            if (show.Count == 0)
            {
                layout = new LinearLayout(this);
                layout.Orientation = Orientation.Vertical;
                LinearLayout.LayoutParams LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                LayoutParameters.SetMargins(0, 30, 0, 30);
                layout.LayoutParameters = LayoutParameters;
                layout.SetBackgroundColor(Color.White);
                layout.SetPadding(40, 40, 40, 40);

                var obavijest = new TextView(this);
                obavijest.TextSize = 20;
                obavijest.Text = "Dobrodošao! :)";
                obavijest.SetTextColor(Color.LightSlateGray);
                obavijest.Gravity = GravityFlags.Center;
                obavijest.SetPadding(0, 0, 0, 30);
                layout.AddView(obavijest);

                var obavijest2 = new TextView(this);
                obavijest2.Text = "Nemaš još ništa na svojoj listi? Istraži ALL ili kreiraj novu ludu stvar koju želiš napraviti u svom životu pritiskom na CREATE :)";
                obavijest2.SetTextColor(Color.LightSlateGray);
                obavijest2.SetPadding(40, 0, 40, 40);
                obavijest2.Gravity = GravityFlags.Center;
                layout.AddView(obavijest2);

                mainLayout.AddView(layout);
            }

            //Dodavanje na layout
            show.Reverse();
            foreach (BucketItem item in show)
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

                //Kreiranje opisa
                var opisLabel = new TextView(this);
                opisLabel.Text = item.Opis;
                opisLabel.SetPadding(0, 20, 0, 20);
                opisLabel.SetTextColor(Color.LightSlateGray);
                layout.AddView(opisLabel);

                if (allormine == 0)
                {
                    //Kreiranje ostvarenosti
                    var ostvarenoSlika = new ImageView(this);
                    bool imamNaSvojojListi = myitems.Any(e => e.Ime == item.Ime);
                    if (imamNaSvojojListi) ostvarenoSlika.SetImageResource(Resource.Drawable.ostvarenoTRUE);
                    else ostvarenoSlika.SetImageResource(Resource.Drawable.ostvarenoFALSE);
                    LayoutParameters = new LinearLayout.LayoutParams(100, 100);
                    ostvarenoSlika.LayoutParameters = LayoutParameters;

                    var ostvarenoLabel = new TextView(this);
                    ostvarenoLabel.SetTextColor(Color.LightGray);

                    int broj = item.koliko - 1;
                    if (imamNaSvojojListi) ostvarenoLabel.Text = "Ti i još " + broj + " ljudi je ostvaruje ovo";
                    else ostvarenoLabel.Text = item.koliko + " ljudi ostvaruje ovo";
                    ostvarenoLabel.SetPadding(10, 25, 0, 0);

                    var grupa = new LinearLayout(this)
                    {
                        Orientation = Orientation.Horizontal,
                        LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 100)
                    };

                    grupa.AddView(ostvarenoSlika);
                    grupa.AddView(ostvarenoLabel);
                    layout.AddView(grupa);

                    if (!imamNaSvojojListi)
                    {
                        //Kreiranje gumba za dodavanje
                        var dodajButton = new Button(this);
                        dodajButton.Text = "+ Dodaj";
                        dodajButton.SetTextColor(Color.White);
                        dodajButton.SetTextSize(ComplexUnitType.Px, 40);
                        dodajButton.Click += (sender, e) =>
                        {
                            BucketItem bModel = new BucketItem();
                            bModel = item;
                            bModel.Ostvareno = false;
                            dodajButton.Text = "Dodano na listu";
                            dodajButton.Enabled = false;

                            client = new HttpClient();
                            conn2(bModel, user.id);
                            Toast.MakeText(this, "Dodano na bucket listu! Prilagodi na svom profilu." + item.Ime, ToastLength.Short).Show();
                        };
                        layout.AddView(dodajButton);
                    }
                }

                if (allormine == 1)
                {
                    var grupa1 = new LinearLayout(this)
                    {
                        Orientation = Orientation.Horizontal,
                        LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 100)
                    };

                    var ostvarenoSlika = new ImageView(this);
                    if (item.Ostvareno) ostvarenoSlika.SetImageResource(Resource.Drawable.ostvarenoTRUE);
                    else ostvarenoSlika.SetImageResource(Resource.Drawable.ostvarenoFALSE);
                    LayoutParameters = new LinearLayout.LayoutParams(100, 100);
                    ostvarenoSlika.LayoutParameters = LayoutParameters;
                    grupa1.AddView(ostvarenoSlika);

                    var ostvarenoLabel = new TextView(this);
                    ostvarenoLabel.SetTextColor(Color.LightGray);
                    int broj = item.koliko - 1;
                    if (item.Ostvareno) ostvarenoLabel.Text = "Ostvario si ovo";
                    else ostvarenoLabel.Text = "Nisi još ostvario";
                    ostvarenoLabel.SetPadding(10, 25, 0, 0);
                    grupa1.AddView(ostvarenoLabel);

                    layout.AddView(grupa1);

                    var grupa2 = new LinearLayout(this)
                    {
                        Orientation = Orientation.Horizontal,
                        //LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, )
                    };

                    var ostvarenobutton = new Button(this);
                    if (item.Ostvareno) ostvarenobutton.Text = "Vrati na neostvareno";
                    else ostvarenobutton.Text = "Ostvari";
                    ostvarenobutton.SetTextColor(Color.White);
                    ostvarenobutton.Click += (sender, e) =>
                    {
                        if (item.Ostvareno)
                        {
                            ostvarenoLabel.Text = "Nisi još ostvario";
                            ostvarenoSlika.SetImageResource(Resource.Drawable.ostvarenoFALSE);
                            ostvarenobutton.Text = "Ostvari";
                            item.Ostvareno = !item.Ostvareno;
                        }
                        else
                        {
                            ostvarenoLabel.Text = "Ostvario si ovo";
                            ostvarenoSlika.SetImageResource(Resource.Drawable.ostvarenoTRUE);
                            ostvarenobutton.Text = "Vrati na neostvareno";
                            item.Ostvareno = !item.Ostvareno;
                        }

                        conn("http://bucketlist.ddns.net/api/items/ostvareno/" + user.id + "?bucketItemId="+item.Id, 1);
                    };
                    grupa2.AddView(ostvarenobutton);

                    var uredibutton = new Button(this);
                    uredibutton.Text = "Uredi";
                    uredibutton.SetTextColor(Color.White);
                    uredibutton.Click += (sender, e) =>
                    {
                        Intent activity2 = new Intent(this, typeof(UpdateItem));
                        activity2.PutExtra("ime", item.Ime);
                        activity2.PutExtra("itemId", item.Id);
                        activity2.PutExtra("kategorija", item.KategorijaNaziv);
                        activity2.PutExtra("opis", item.Opis);
                        activity2.PutExtra("slika", item.Slika);
                        activity2.PutExtra("userId", user.id);
                        activity2.PutExtra("koliko", item.koliko);
                        StartActivity(activity2);
                    };
                    grupa2.AddView(uredibutton);
                    

                    layout.AddView(grupa2);
                }


                //Dodavanje kreiranog layouta na glavni layout
                mainLayout.AddView(layout);
            }
        }


//LOGIN*****************************************************************************************************

        //Login fecth the URL from login.cs activity
        private async void login(string url)
        {
            JsonValue response = await RefreshDataAsync(url);
            UserInfo(response);
            myitems = new List<BucketItem>();
            Init("http://bucketlist.ddns.net/api/items/" + user.id);
            
        }

        //Fetching user information
        private void UserInfo (JsonValue json)
        {
            user = new TheBucketList8.Korisnik();
            user.ime = json["Ime"];
            user.imeprezime = json["FullName"];
            user.prezime = json["Prezime"];
            user.slika = Base64.Decode(json["Slika"], Base64Flags.Default);
            user.username = json["Username"];
            user.password = json["Lozinka"];
            user.motto = json["Moto"];
            user.opis = json["Opis"];
            user.id = json["Id"];
        }


    }
}
