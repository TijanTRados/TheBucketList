using Android.App;
using Android.Widget;
using Android.OS;
using TheBucketList.Models;
using TheBucketList.Controllers;
using TheBucketList.NHibernate;


namespace TheBucketListMobile
{
    [Activity(Label = "TheBucketListMobile", Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        EditText txtEmail;
        EditText txtPassword;
        Button btnLogin;

        protected override void OnCreate(Bundle bundle)
        {

            EditText txtEmail = (EditText)FindViewById(Resource.Id.editText1);
            EditText txtPassword = (EditText)FindViewById(Resource.Id.editText2);
            Button btnLogin = (Button)FindViewById(Resource.Id.button1);
            base.OnCreate(bundle);           
            btnLogin.Click += delegate
            {
                btn1Click();
            };
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

        }

        private void btn1Click()
        {
            if (txtEmail.Text!=null && txtEmail.Text != "" && txtPassword.Text != null && txtPassword.Text != "")
            {
                AccountController accControler = new AccountController();
                LoginViewModel loginModel = new LoginViewModel();
                loginModel.Email = txtEmail.Text;
                loginModel.Password = txtPassword.Text;
                //BucketItemsModel bItemsModel = accControler.LoginMobile(loginModel);
            }
            else
            {

            }
        }
    }
}

