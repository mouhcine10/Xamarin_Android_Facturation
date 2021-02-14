using System;
using System.Collections.Generic;
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
using Mono.Data.Sqlite;

namespace Facturation
{
    [Activity(Label = "UpdateInfoBanque", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class UpdateInfoBanque : AppCompatActivity
    {
        EditText nombanque;
        EditText Bic;
        EditText IBAN;
        AdView ads1, ads2 ,ads3;
        Button buttonsuivantbanque;

        private void UpdateInfobanque(int id, string bic, string iban, string nomb)
        {


            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idbanque", id));
            contenu.Parameters.Add(new SqliteParameter("@bic", bic));
            contenu.Parameters.Add(new SqliteParameter("@iban", iban));
            contenu.Parameters.Add(new SqliteParameter("@Nom", nomb));

            // contenu.Parameters.Add(new SqliteParameter("@role",role));
            contenu.CommandText = "UPDATE  Banck SET bic=@bic, IBAN=@iban,Nombanc=@Nom WHERE  Banck.IDInfobanck=@idbanque  ";

            contenu.ExecuteNonQuery();

            connection.Close();



        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorDevis = sharedPreferences.Edit();
                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.UpdateInfobanque);


                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);


                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                SupportActionBar.Title = "Modifier Vos Info bancaire";
                nombanque = FindViewById<EditText>(Resource.Id.textInputEditTextNombanqueupdate);
                Bic = FindViewById<EditText>(Resource.Id.textInputEditTextBICupdate);
                IBAN = FindViewById<EditText>(Resource.Id.textInputEditTextIBANupdate);
                buttonsuivantbanque = FindViewById<Button>(Resource.Id.buttonsuivantbbanqueupdate);
                ads1 = FindViewById<AdView>(Resource.Id.adView1banqe);
                ads2 = FindViewById<AdView>(Resource.Id.adView2banqe);
                ads3= FindViewById<AdView>(Resource.Id.adView3baque);

                var adRequest1 = new AdRequest.Builder().Build();
                ads1.LoadAd(adRequest1);
                var adRequest2 = new AdRequest.Builder().Build();
                ads2.LoadAd(adRequest2);
                var adRequest3 = new AdRequest.Builder().Build();
                ads3.LoadAd(adRequest3);


                nombanque.Text = sharedPreferences.GetString("NomBanque", "");
                Bic.Text = sharedPreferences.GetString("Bic", "");
                IBAN.Text = sharedPreferences.GetString("Iban", "");
                sharedPreferences.GetInt("ID", 0);
                buttonsuivantbanque.Click += delegate {



                    UpdateInfobanque(sharedPreferences.GetInt("ID", 0), Bic.Text, IBAN.Text,nombanque.Text) ;

                    Intent intent = new Intent(this, typeof(Itemsapp));
                    intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                    StartActivity(intent);

                };

                }
            catch (Exception ex)
            {

                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }
            // Create your application here
        }
    }
}