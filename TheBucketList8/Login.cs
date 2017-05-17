using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
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

namespace TheBucketList8
{
    [Activity(Label = "The Bucket List", MainLauncher = true)]
    public class Login : Activity
    {

        public EditText username, password;
        public Button login, register;
        public string url;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.login);

            username = FindViewById<EditText>(Resource.Id.editText1);
            password = FindViewById<EditText>(Resource.Id.editText2);
            login = FindViewById<Button>(Resource.Id.button1);
            register = FindViewById<Button>(Resource.Id.button2);

            login.Click += (sender, e) =>
            {
                if (username.Text == "") Toast.MakeText(this, "Upiši username", ToastLength.Short).Show();
                else conn();
            };

            register.Click += (sender, e) =>
            {
                Intent activityReg = new Intent(this, typeof(Register));
                StartActivity(activityReg);
            };

            // Create your application here
        }

        public async void conn()
        {
            url = "http://bucketlist.ddns.net/api/korisnik?username=" + username.Text + "&password=" + password.Text;
            JsonValue response = await RefreshDataAsync(url);
            Toast.MakeText(this, "SpajanjeDRUGO", ToastLength.Short).Show();
            StartMain(response);
        }

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

        private void StartMain(JsonValue json)
        {

            if((json["Ime"]) == "null")
            {
                Toast.MakeText(this, "Neispravni login podaci ili nepostojeæi korisnik?", ToastLength.Short).Show();
                return;
            }
            else
            {
                Intent activity2 = new Intent(this, typeof(MainActivity));
                activity2.PutExtra("url", url);
                StartActivity(activity2);
            }
        }
    }
}