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
using static TheBucketListMobile.AllBucketItems;
using System.Threading.Tasks;
using System.Net.Http;

namespace TheBucketListMobile
{
    [Activity(Label = "AllBucketItems", MainLauncher = true)]
    public class AllBucketItems : Activity
    {

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
                        HttpResponseMessage x = await RefreshDataAsync();
                    // Create your application here
                    }

                    //Task<List<BucketItemModel>>
                    public async Task<HttpResponseMessage> RefreshDataAsync()
                    {
                        HttpClient client = new HttpClient();
                        client.MaxResponseContentBufferSize = 256000;

                        string RestUrl = "Http://161.53.78.35/api/items";
                            var uri = new Uri(string.Format(RestUrl, string.Empty));

                        var response = await client.GetAsync(uri);
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            //var Items = DeserializeObject<List<BucketItemModel>>(content);
                        }

                        return response;
                    }
                }
            }
