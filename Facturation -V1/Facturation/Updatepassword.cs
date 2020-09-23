using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
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
    [Activity(Label = "Modifier mot de passe", Theme = "@style/AppTheme.NoActionBar")]
    public class Updatepassword : AppCompatActivity
    {
        AdView ads1, ads2;
        EditText mpd, nvmdp;
        EditText conmdp;
        Button btnok;
        string Passwordcheck(string password,string email)
        {
            string tard ="";

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@pass", password));
            contenu.Parameters.Add(new SqliteParameter("@email", email));

            contenu.CommandText = "SELECT Mdp from ClientAdmin WHERE Mdp=@pass and  Emailclient=@email";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {
                if (r[0].ToString() != null)
                {


                    tard =r[0].ToString();

                }



            }


            connection.Close();
            return tard;

        }
        void UpdatePassword(string password, string role, string email)
        {

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@pass", password));
            contenu.Parameters.Add(new SqliteParameter("@Role", role));
            contenu.Parameters.Add(new SqliteParameter("@Email", email));

            contenu.CommandText = "UPDATE ClientAdmin SET Mdp=@pass  WHERE role=@Role and Emailclient=@Email";

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

                SetContentView(Resource.Layout.UpdateMdp);

                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);


                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                SupportActionBar.Title = "Modifier Mot de passe";

                ads1 = FindViewById<AdView>(Resource.Id.adView1mdpUpdate);
                ads2 = FindViewById<AdView>(Resource.Id.adView2mdpUpdate);

                var adRequest1 = new AdRequest.Builder().Build();
                ads1.LoadAd(adRequest1);
                var adRequest2 = new AdRequest.Builder().Build();
                ads2.LoadAd(adRequest2);


                nvmdp = FindViewById<EditText>(Resource.Id.editTextpassactUpdate);
                mpd = FindViewById<EditText>(Resource.Id.textInputEditTextmotdepasseUpdate);
                conmdp = FindViewById<EditText>(Resource.Id.textInputEditTextconmotdepasseUpdate);

                btnok = FindViewById<Button>(Resource.Id.buttonokUpdate);




                btnok.Click += delegate
                {
                    

                        //if (Passwordcheck(mpd.Text, sharedPreferences.GetString("Emailad", "")) == mpd.Text)
                        //{

                            UpdatePassword(mpd.Text, "Admin", sharedPreferences.GetString("Emailad", ""));
                            Intent intent = new Intent(this, typeof(Itemsapp));
                    intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                    StartActivity(intent);

                        //}
                        //else
                        //{
                            //AlertDialog.Builder builder = new AlertDialog.Builder(this);

                            //builder.SetTitle("avertissement").SetMessage("mot de passe  incorrecte").SetPositiveButton("OK", (sender, args) => { builder.Dispose(); }).Show();


                        
                    
                };


            }
            catch (Exception ex)
            {

                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();

            }
        }
    }
}