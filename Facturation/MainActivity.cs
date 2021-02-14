using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Widget;
using System;

namespace Facturation
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation/*, MainLauncher = true*/)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {

                base.OnCreate(savedInstanceState);
                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                // Set our view from the "main" layout resource
                //  SetContentView(Resource.Layout.Fact);
                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                SetSupportActionBar(toolbar);

                //var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
                //toolbar.SetTitleTextColor(Android.Graphics.Color.GreenYellow);
                //toolbar.SetBackgroundColor(Android.Graphics.Color.White);
                //SetActionBar(toolbar);
                //ActionBar.Title = "Liste de Facture";


                var butonAjouterFact = FindViewById<FloatingActionButton>(Resource.Id.AjouterFact);
                // var btnaPiece = FindViewById<Button>(Resource.Id.buttonPiece);

                //butonAjouterFact.Click += delegate
                //{


                //    Intent intent = new Intent(this, typeof(F));

                //    StartActivity(intent);

                //};

            }catch(Exception ex)
            {

                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}