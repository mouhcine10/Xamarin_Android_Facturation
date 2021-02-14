using System;
using System.Collections.Generic;
using Android.Graphics;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Xamarin.Controls;
using Android.Gms.Ads;
using Android.Content.PM;

namespace Facturation
{
    [Activity(Label = "Signature", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class LayoutsignatureDevis : AppCompatActivity
    {

        AdView ads1, ads2;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {

                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();
                base.OnCreate(savedInstanceState);


                SetContentView(Resource.Layout.layoutSignature);

                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
                SetSupportActionBar(toolbar);
                //SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                //SupportActionBar.SetHomeButtonEnabled(true);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));

                //Get a reference to the FingerPaintCanvasView from the Main.axml file
                //  fingerPaintCanvasView = FindViewById<FingerPaintCanvasView>(Resource.Id.fingerPaintCanvasView1);

                Button signature = FindViewById<Button>(Resource.Id.buttonrenregis);


                ads1 = FindViewById<AdView>(Resource.Id.adView1Signature);
                ads2 = FindViewById<AdView>(Resource.Id.adView2Signature);

                var adRequest1 = new AdRequest.Builder().Build();
                ads1.LoadAd(adRequest1);
                var adRequest2 = new AdRequest.Builder().Build();
                ads2.LoadAd(adRequest2);



                var signatureView = FindViewById<SignaturePadView>(Resource.Id.signaturePadView5);

                signatureView.BackgroundColor = Android.Graphics.Color.White;


                //  var img = FindViewById<ImageView>(Resource.Id.imageViewsign);


                signatureView.GetImageStreamAsync(SignatureImageFormat.Png);






                signature.Click += delegate
                {
                    Bitmap image = signatureView.GetImage();

                    //var tra = signatureView.GetImageStreamAsync(SignatureImageFormat.Png);
                    //int id = Intent.GetIntExtra("IDfacture", 0);
                    //string filename = "SignatureFacture" + id.ToString() + "";
                    //FileStream stream=null;
                    //Java.IO.File dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "SignatureImage");
                    //string path = System.IO.Path.Combine(dir.AbsolutePath, filename);
                    //bool Existe = System.IO.File.Exists(path);
                    //if (!dir.Exists())
                    //{
                    //    dir.Mkdir();

                    //    if (Existe)
                    //    {

                    MemoryStream bos = new MemoryStream();
                    image.Compress(Bitmap.CompressFormat.Png, 100, bos);
                    byte[] bitmapdata = bos.ToArray();
                    Intent intent = new Intent(this, typeof(Deviss));


                    //InsertionImage(bitmapdata, Intent.GetIntExtra("IDfacture",0));
                    intent.PutExtra("ImagePath", bitmapdata);

                    // img.SetImageBitmap(image);

                    StartActivity(intent);


                };
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }
        }

    }
}