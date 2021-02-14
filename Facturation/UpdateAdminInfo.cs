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
using Facturation.Class;
using Mono.Data.Sqlite;

namespace Facturation
{
    [Activity(Label = "UpdateAdminInfo", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class UpdateAdminInfo : AppCompatActivity
    {
        AdView ads1, ads2;
        EditText nom;
        EditText Email;
        EditText Adresse;
        EditText tel;
        EditText CodePostal;
        EditText Ville;
        EditText Pays;
        Button btnsuivant;

        string nomtext, emailtext, adressetext, teltext, codepsotaltext, villetext, paystext;
        List<Clients> clients = new List<Clients>();
        private List<Clients> affAdmin(string inemail)
        {
            string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@email", inemail));
            contenu.CommandText = "Select Emailclient,NomClinet,Telephone,adresse,CodePostal,ville,pays from ClientAdmin where EmailClient=@email";


            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                clients.Add(new Clients(r[0].ToString(), r[1].ToString(), r[2].ToString(), r[3].ToString(), r[4].ToString(), r[5].ToString(), r[6].ToString()));
            }

            connection.Close();

            return clients;
        }

        private void updateAdmin(string inemail, string emailL, string nom, string tel, string adresse, string codepostal, string ville, string pays)
        {
            string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@emailad", inemail));
            contenu.Parameters.Add(new SqliteParameter("@emaill", emailL));
            contenu.Parameters.Add(new SqliteParameter("@nom", nom));
            contenu.Parameters.Add(new SqliteParameter("@tel", tel));
            contenu.Parameters.Add(new SqliteParameter("@adresse", adresse));
            contenu.Parameters.Add(new SqliteParameter("@codepostal", codepostal));
            contenu.Parameters.Add(new SqliteParameter("@ville", ville));
            contenu.Parameters.Add(new SqliteParameter("@pays", pays));
            contenu.CommandText = "UPDATE ClientAdmin set NomClinet=@nom,Telephone=@tel,Emailclient=@emaill,adresse=@adresse,ville=@ville,pays=@pays where Emailclient=@emailad ";

            contenu.ExecuteNonQuery();


            connection.Close();



        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.UpdateInfoAdmin);


            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);


            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
            toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
            SupportActionBar.Title = "Modifier vos Informations";

            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();
            nom = FindViewById<EditText>(Resource.Id.textInputEditTextNomupdate);
            Email = FindViewById<EditText>(Resource.Id.textInputEditTextEmailupdate);
            Email.Enabled = false;
            tel = FindViewById<EditText>(Resource.Id.editTextTelePhoneupdate);
            Adresse = FindViewById<EditText>(Resource.Id.textInputEditTextAdresseupdate);
            CodePostal = FindViewById<EditText>(Resource.Id.textInputEditTextCodePostalupdate);
            Ville = FindViewById<EditText>(Resource.Id.textInputEditTextVilleupdate);
            Pays = FindViewById<EditText>(Resource.Id.textInputEditTextPaysupdate);
            btnsuivant = FindViewById<Button>(Resource.Id.buttonsuivantupdate);
            ads1 = FindViewById<AdView>(Resource.Id.adView1Updatecl);
            ads2 = FindViewById<AdView>(Resource.Id.adView2Updatecl);
            
            var ads3 = FindViewById<AdView>(Resource.Id.adView12updatecl);

            var adRequest1 = new AdRequest.Builder().Build();
            ads1.LoadAd(adRequest1);
            var adRequest2 = new AdRequest.Builder().Build();
            ads2.LoadAd(adRequest2);

            var adRequest3 = new AdRequest.Builder().Build();

            ads3.LoadAd(adRequest3);


            nom.Text = affAdmin(sharedPreferences.GetString("Emailadmin", ""))[0].NomClinet;
            Email.Text = affAdmin(sharedPreferences.GetString("Emailadmin", ""))[0].Email;
            
            tel.Text = affAdmin(sharedPreferences.GetString("Emailadmin", ""))[0].Telephone;
            Adresse.Text = affAdmin(sharedPreferences.GetString("Emailadmin", ""))[0].Adresse;
            CodePostal.Text = affAdmin(sharedPreferences.GetString("Emailadmin", ""))[0].Codepostal;
            Ville.Text = affAdmin(sharedPreferences.GetString("Emailadmin", ""))[0].Ville;
            Pays.Text = affAdmin(sharedPreferences.GetString("Emailadmin", ""))[0].Pays;

            btnsuivant.Click += delegate
            {



                updateAdmin(Email.Text, Email.Text, nom.Text, tel.Text, Adresse.Text, CodePostal.Text, Ville.Text, Pays.Text);


                Intent intent = new Intent(this, typeof(Itemsapp));
                editorfacture.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin",""));
                intent.PutExtra("Emailadmin",Intent.GetStringExtra("Emailadmin"));

                StartActivity(intent);


            };


            // Create your application here
        }
    }
}