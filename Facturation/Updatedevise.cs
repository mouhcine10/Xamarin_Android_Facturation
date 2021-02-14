using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Hardware.Camera2;
using Android.OS;   
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;

namespace Facturation
{
    [Activity(Label = "Updatedevise", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class Updatedevise : AppCompatActivity
    {
        string toast = " ";
        public void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            toast = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorDevis = sharedPreferences.Edit();

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.UpdateDevise);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);


            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
            toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
            SupportActionBar.Title = "Modifier Devise";

            var TextDevisactuel = FindViewById<TextView>(Resource.Id.textViewUpdateDevisactuel);

            TextDevisactuel.Text = Devise(sharedPreferences.GetString("Emailadmin", ""));

            var spinnerDeviseupdate = FindViewById<Spinner>(Resource.Id.spinnerUpdateDevise);

            var btnvalider = FindViewById<Button>(Resource.Id.buttonUpdateDevise);


            btnvalider.Click += delegate {


                UpdateDevise(Email(sharedPreferences.GetString("Emailadmin", "")),toast);

                Intent intent = new Intent(this, typeof(Itemsapp));
                intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                StartActivity(intent);

            };


            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.Devise, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerDeviseupdate.Adapter = adapter;
            spinnerDeviseupdate.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);



            // Create your application here
        }
        private string Devise(string email)
        {
            string dev = "";
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@email", email));

            contenu.CommandText = "SELECT Devise from Banck INNER JOIN ClientAdmin  on Banck.idclientAdmin = ClientAdmin.idclientAdmin WHERE Emailclient=@email";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                dev = r[0].ToString();


            }

            connection.Close();

            return dev;

        }
        private int Email(string email)
        {
            int dev = 0;
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@email", email));

            contenu.CommandText = "SELECT Banck.idclientAdmin from Banck INNER JOIN ClientAdmin on Banck.idclientAdmin = ClientAdmin.idclientAdmin WHERE ClientAdmin.Emailclient=@email";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                dev = int.Parse(r[0].ToString());


            }

            connection.Close();

            return dev;

        }
        private void UpdateDevise(int idclient, string toast)
        {
            
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idclient",idclient));
            contenu.Parameters.Add(new SqliteParameter("@Devise",toast));

            contenu.CommandText = "UPDATE Banck set Devise=@Devise WHERE idclientAdmin=@idclient";

            contenu.ExecuteNonQuery();

            

            connection.Close();

        }



    }
}