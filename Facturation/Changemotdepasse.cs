using System;
using System.Collections.Generic;
using System.IO;
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
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;

namespace Facturation
{
    [Activity(Label = "Changer mot de passe", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class Changemotdepasse : Activity
    {

        private void UpdatePassword(string Role,  string mpdn)
        {
            string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@Role", Role));
            contenu.Parameters.Add(new SqliteParameter("@Idclient", Role));
            contenu.Parameters.Add(new SqliteParameter("@Mdpn", mpdn));
            contenu.CommandText = "Update Client set Mdp=@Mdpn where role=@Role";

            contenu.ExecuteNonQuery();

            connection.Clone();





        }


        AdView ad1, ad2;
        EditText mpd;
        EditText conmdp;
        Button btnok;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorDevis = sharedPreferences.Edit();

                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.CreationClinet4);

                ad1 = FindViewById<AdView>(Resource.Id.adView1mdp);
                ad2 = FindViewById<AdView>(Resource.Id.adView2mdp);

                mpd = FindViewById<EditText>(Resource.Id.textInputEditTextmotdepasse);
                conmdp = FindViewById<EditText>(Resource.Id.textInputEditTextconmotdepasse);

                btnok = FindViewById<Button>(Resource.Id.buttonok);

                btnok.Click += delegate
                {

                    if (mpd.Text == conmdp.Text)
                    {

                        Intent intent = new Intent(this, typeof(Itemsapp));

                        UpdatePassword("Admin",mpd.Text);

                        StartActivity(intent);
                    }
                    else
                    {

                        AlertDialog.Builder builder = new AlertDialog.Builder(this);

                        builder.SetTitle("Avertissement").SetMessage("Mot de passe incorrecte").SetPositiveButton("OK", (sender,args) => { builder.Dispose(); });

                        builder.Show();

                    }
                };
            }
            catch (Exception ex)
            {

                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }
        }
    }
}