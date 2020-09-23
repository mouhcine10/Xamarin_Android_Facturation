using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Preferences;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Facturation.Class;
using iTextSharp.text;
using Java.IO;
using Mono.Data.Sqlite;
using System;
using System.IO;
using Xamarin.Controls;

namespace Facturation
{
    [Activity(Label = "Signature", Theme = "@style/AppTheme.NoActionBar"/*,MainLauncher =true*/)]
    public class LayoutSegnature : AppCompatActivity
    {
        FingerPaintCanvasView fingerPaintCanvasView;

        //private void InsertionImage(byte[] image,int IDfacture)
        //{

        //    string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "andtadgFactureDevis.db3");
        //    var connection = new SqliteConnection("Data Source=" + dbpath);
        //    connection.Open();
        //    var contenu = connection.CreateCommand();
        //    contenu.Parameters.Add(new SqliteParameter("@param1", image));
        //    contenu.Parameters.Add(new SqliteParameter("@param2", IDfacture));
        //    contenu.CommandText = "Insert into Imagefact(Imagevvide,IdFacture) values(@param1,@param2)";

        //    contenu.ExecuteNonQuery();

        //    connection.Close();

        //}

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



                var signatureView = FindViewById<SignaturePadView>(Resource.Id.signaturePadView5);


                ads2 = FindViewById<AdView>(Resource.Id.adView2Signature);
                ads1 = FindViewById<AdView>(Resource.Id.adView1Signature);

                var adRequest1 = new AdRequest.Builder().Build();
                ads1.LoadAd(adRequest1);
                var adRequest2 = new AdRequest.Builder().Build();
                ads2.LoadAd(adRequest2);


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
                     Intent intent = new Intent(this, typeof(Facturee));


                 //InsertionImage(bitmapdata, Intent.GetIntExtra("IDfacture",0));
                 intent.PutExtra("ImagePath", bitmapdata);

                 // img.SetImageBitmap(image);

                 StartActivity(intent);


                 };
            }catch(Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }
        }

    }




}

