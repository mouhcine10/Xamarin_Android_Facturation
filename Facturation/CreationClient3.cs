using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Facturation
{
    [Activity(Label = "Logo", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class CreationClient3 : AppCompatActivity
    {
        ImageView imageLogo;
        Button tbngetlogo;
        Button suivantmot;
        AdView ad1, ad2;
        Android.Net.Uri uri;
        public static readonly int PickImageId = 100;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            try
            {

                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorDevis = sharedPreferences.Edit();


                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.CreationClient3);

                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

                SetSupportActionBar(toolbar);
                //SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                //SupportActionBar.SetHomeButtonEnabled(true);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                toolbar.Title = "Logo";
                // Create your application here

                imageLogo = FindViewById<ImageView>(Resource.Id.imageViewlogoclient);
                tbngetlogo = FindViewById<Button>(Resource.Id.buttonsuivantlogo);
                suivantmot = FindViewById<Button>(Resource.Id.buttonsuivant);
                ad1 = FindViewById<AdView>(Resource.Id.adView1Logo);
                ad2 = FindViewById<AdView>(Resource.Id.adView2logo);
                var adRequest1 = new AdRequest.Builder().Build();
                var adRequest2 = new AdRequest.Builder().Build();

                ad1.LoadAd(adRequest1);
                ad2.LoadAd(adRequest2);
                var btnp = FindViewById<Button>(Resource.Id.buttonPrecedant);

                btnp.Click += delegate {

                    Intent intent = new Intent(this, typeof(CreationClient2));
                    editorDevis.PutString("Nom", sharedPreferences.GetString("Nom", ""));
                    editorDevis.PutString("Email", sharedPreferences.GetString("Email", ""));
                    editorDevis.PutString("Tel", sharedPreferences.GetString("Tel", ""));
                    editorDevis.PutString("Adresse", sharedPreferences.GetString("Adresse", ""));
                    editorDevis.PutString("CodePostal", sharedPreferences.GetString("CodePostal", ""));
                    editorDevis.PutString("Ville", sharedPreferences.GetString("Ville", ""));
                    editorDevis.PutString("Pays", sharedPreferences.GetString("Pays", ""));

                    editorDevis.PutString("Nombanque", sharedPreferences.GetString("Nombanque", ""));
                    editorDevis.PutString("Bic", sharedPreferences.GetString("Bic", ""));
                    editorDevis.PutString("IBAN", sharedPreferences.GetString("IBAN", ""));
                    editorDevis.PutString("Devise", sharedPreferences.GetString("Devise", ""));

                    editorDevis.Apply();
                    StartActivity(intent);
                
                
                };

                tbngetlogo.Click += delegate
                {

                    Intent = new Intent();
                    Intent.SetType("image/*");
                    Intent.SetAction(Intent.ActionGetContent);
                    StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);



                };

                suivantmot.Click += delegate
                {

                    editorDevis.PutString("Nom", sharedPreferences.GetString("Nom", ""));
                    editorDevis.PutString("Email", sharedPreferences.GetString("Email", ""));
                    editorDevis.PutString("Tel", sharedPreferences.GetString("Tel", ""));
                    editorDevis.PutString("Adresse", sharedPreferences.GetString("Adresse", ""));
                    editorDevis.PutString("CodePostal", sharedPreferences.GetString("CodePostal", ""));
                    editorDevis.PutString("Ville", sharedPreferences.GetString("Ville", ""));
                    editorDevis.PutString("Pays", sharedPreferences.GetString("Pays", ""));

                    editorDevis.PutString("Nombanque",sharedPreferences.GetString("Nombanque", ""));
                    editorDevis.PutString("Bic",sharedPreferences.GetString("Bic", ""));
                    editorDevis.PutString("IBAN",sharedPreferences.GetString("IBAN", ""));
                    editorDevis.PutString("Devise", sharedPreferences.GetString("Devise", ""));

                    editorDevis.Apply();

                    if (uri != null)
                    {
                        Stream stream = ContentResolver.OpenInputStream(uri);
                        byte[] byteArray;
                        var memoryStream = new MemoryStream();
                        stream.CopyTo(memoryStream);
                        byteArray = memoryStream.ToArray();

                        Intent intent = new Intent(this, typeof(CreationClient4));

                         

                        intent.PutExtra("ImageLogo", byteArray);

                        StartActivity(intent);
                    }
                    else
                    {

                        Toast.MakeText(this, "Image", ToastLength.Long).Show();

                    }
                };



            }
            catch (Exception ex)
            {

                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }

        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
            {
                uri = data.Data;
                imageLogo.SetImageURI(uri);



            }
        }
    }

}




