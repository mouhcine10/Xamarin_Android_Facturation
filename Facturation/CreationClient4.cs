using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Facturation.Class;
using Mono.Data.Sqlite;

namespace Facturation
{
    [Activity(Label = "Mot de passe", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class CreationClient4 : AppCompatActivity
    {
        AdView ad1, ad2;
        EditText mpd;
        EditText conmdp;
        Button btnok;

        byte[] db = null;

        List<Clients> Clients = new List<Clients>();
        int IDclient(string role, string email)
        {
            int id = 0;

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@Role", role));
            contenu.Parameters.Add(new SqliteParameter("@email", email));
            contenu.CommandText = "Select idClientadmin from ClientAdmin where Role=@Role and  Emailclient=@email";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                id = int.Parse(r[0].ToString());

            }


            return id;

        }

        private void Inscription(string typeclient, string Nomclient, int tel, string Email, string adresse, string Codepostal, string ville, string pays, byte[] logo, string mdp, string role)
        {

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@Typeclient", typeclient));
            contenu.Parameters.Add(new SqliteParameter("@Nomclient", Nomclient));
            contenu.Parameters.Add(new SqliteParameter("@tel", tel));
            contenu.Parameters.Add(new SqliteParameter("@email", Email));
            contenu.Parameters.Add(new SqliteParameter("@adresse", adresse));
            contenu.Parameters.Add(new SqliteParameter("@codepostal", Codepostal));
            contenu.Parameters.Add(new SqliteParameter("@ville", ville));
            contenu.Parameters.Add(new SqliteParameter("@pays", pays));
            contenu.Parameters.Add(new SqliteParameter("@logo", logo));
            contenu.Parameters.Add(new SqliteParameter("@mdp", mdp));
            contenu.Parameters.Add(new SqliteParameter("@role", role));
            contenu.CommandText = "INSERT into ClientAdmin(TypeClinet,NomClinet,Telephone,Emailclient,adresse,CodePostal,ville,pays,Logo,Mdp,role)values(@Typeclient,@Nomclient,@tel,@email,@adresse,@codepostal,@ville,@pays,@logo,@mdp,@role)";

            contenu.ExecuteNonQuery();

            connection.Close();

        }

        private void Infobanck(string nombanc, string Bic, string Iban, string devise, int idclient)
        {

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@nomclient", nombanc));
            contenu.Parameters.Add(new SqliteParameter("@Bic", Bic));
            contenu.Parameters.Add(new SqliteParameter("@Iban", Iban));
            contenu.Parameters.Add(new SqliteParameter("@idclient", idclient));
            contenu.Parameters.Add(new SqliteParameter("@Devise", devise));
            contenu.CommandText = "INSERT into Banck(Nombanc,BIC,IBAN,Devise,idclientAdmin)VALUES(@nomclient,@Bic,@Iban,@Devise,@idclient)";

            contenu.ExecuteNonQuery();

            connection.Close();

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorDevis = sharedPreferences.Edit();

                db = Intent.GetByteArrayExtra("ImageLogo");









                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.CreationClinet4);

                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

                SetSupportActionBar(toolbar);
                //SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                //SupportActionBar.SetHomeButtonEnabled(true);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                toolbar.Title = "Mot de passe";

                ad1 = FindViewById<AdView>(Resource.Id.adView1mdp);
                ad2 = FindViewById<AdView>(Resource.Id.adView2mdp);

                var adRequest1 = new AdRequest.Builder().Build();
                var adRequest2 = new AdRequest.Builder().Build();

                ad1.LoadAd(adRequest1);
                ad2.LoadAd(adRequest2);

                mpd = FindViewById<EditText>(Resource.Id.textInputEditTextmotdepasse);
                conmdp = FindViewById<EditText>(Resource.Id.textInputEditTextconmotdepasse);

                btnok = FindViewById<Button>(Resource.Id.buttonok);

                var btnp = FindViewById<Button>(Resource.Id.buttonPrecedant);

                btnp.Click += delegate
                {
                    Intent intent = new Intent(this, typeof(CreationClient3));
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

                btnok.Click += delegate
                {

                    Inscription("Admin", sharedPreferences.GetString("Nom", ""), int.Parse(sharedPreferences.GetString("Tel", "")), sharedPreferences.GetString("Email", ""), sharedPreferences.GetString("Adresse", ""), sharedPreferences.GetString("CodePostal", ""), sharedPreferences.GetString("Ville", ""), sharedPreferences.GetString("Pays", ""), db, mpd.Text, "Admin");


                    Infobanck(sharedPreferences.GetString("Nombanque", ""), sharedPreferences.GetString("Bic", ""), sharedPreferences.GetString("IBAN", ""), sharedPreferences.GetString("Devise", ""), IDclient("Admin", sharedPreferences.GetString("Email", "")));

                    Intent intent = new Intent(this, typeof(PageHome));

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