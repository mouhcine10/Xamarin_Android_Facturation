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
    [Activity(Label = "Informations Bancaire", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class CreationClient2 : AppCompatActivity
    {
        EditText nombanque;
        EditText Bic;
        EditText IBAN;
        AdView ad1, ad2;
        Button buttonsuivantbanque;
        Spinner devisespinner;

        public void UPBanque(int idclient, string role, string Nom, string bic, string Iban)
        {
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();


            contenu.Parameters.Add(new SqliteParameter("@idclient", idclient));
            contenu.CommandText = "Select idFacture from  Facture where idFacture=@idfacture";

            var r = contenu.ExecuteReader();
        }
        string toast = "";
        public void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            toast = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorDevis = sharedPreferences.Edit();

                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.CreationClient2);
                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

                SetSupportActionBar(toolbar);
                //SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                //SupportActionBar.SetHomeButtonEnabled(true);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                toolbar.Title = "Information Bancaire";

                nombanque = FindViewById<EditText>(Resource.Id.textInputEditTextNombanque);
                Bic = FindViewById<EditText>(Resource.Id.textInputEditTextBIC);
                IBAN = FindViewById<EditText>(Resource.Id.textInputEditTextIBAN);

                ad1 = FindViewById<AdView>(Resource.Id.adView1banqe);
                ad2 = FindViewById<AdView>(Resource.Id.adView2banqe);
                var adRequest1 = new AdRequest.Builder().Build();
                var adRequest2 = new AdRequest.Builder().Build();

                ad1.LoadAd(adRequest1);
                ad2.LoadAd(adRequest2);

                devisespinner = FindViewById<Spinner>(Resource.Id.spinnerDevis);

                var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.Devise, Android.Resource.Layout.SimpleSpinnerItem);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

                devisespinner.Adapter = adapter;
                devisespinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
                buttonsuivantbanque = FindViewById<Button>(Resource.Id.buttonsuivantbbanque);

                var btnp = FindViewById<Button>(Resource.Id.buttonPrecedant);


                nombanque.Text = sharedPreferences.GetString("Nombanque", "");
                Bic.Text = sharedPreferences.GetString("Bic", "");
                IBAN.Text = sharedPreferences.GetString("IBAN", "");

                int d = adapter.GetPosition(sharedPreferences.GetString("Devis", ""));

                devisespinner.SetSelection(d);
                btnp.Click += delegate
                {
                    nombanque.Text = "";
                    Bic.Text = "";
                    IBAN.Text = " ";
                    Intent intent = new Intent(this, typeof(CreationClient1));
                    editorDevis.PutString("Nom", sharedPreferences.GetString("Nom", ""));
                    editorDevis.PutString("Email", sharedPreferences.GetString("Email", ""));
                    editorDevis.PutString("Tel", sharedPreferences.GetString("Tel", ""));
                    editorDevis.PutString("Adresse", sharedPreferences.GetString("Adresse", ""));
                    editorDevis.PutString("CodePostal", sharedPreferences.GetString("CodePostal", ""));
                    editorDevis.PutString("Ville", sharedPreferences.GetString("Ville", ""));
                    editorDevis.PutString("Pays", sharedPreferences.GetString("Pays", ""));

                    editorDevis.PutString("Nombanque", nombanque.Text);
                    editorDevis.PutString("Bic", Bic.Text);
                    editorDevis.PutString("IBAN", IBAN.Text);
                    editorDevis.PutString("Devise", toast);
                    editorDevis.Apply();
                    StartActivity(intent);


                };

                buttonsuivantbanque.Click += delegate
                {
                    nombanque.Text = "";
                    Bic.Text = "";
                    IBAN.Text = " ";

                    editorDevis.PutString("Nom", sharedPreferences.GetString("Nom", ""));
                    editorDevis.PutString("Email", sharedPreferences.GetString("Email", ""));
                    editorDevis.PutString("Tel", sharedPreferences.GetString("Tel", ""));
                    editorDevis.PutString("Adresse", sharedPreferences.GetString("Adresse", ""));
                    editorDevis.PutString("CodePostal", sharedPreferences.GetString("CodePostal", ""));
                    editorDevis.PutString("Ville", sharedPreferences.GetString("Ville", ""));
                    editorDevis.PutString("Pays", sharedPreferences.GetString("Pays", ""));

                    editorDevis.PutString("Nombanque", nombanque.Text);
                    editorDevis.PutString("Bic", Bic.Text);
                    editorDevis.PutString("IBAN", IBAN.Text);
                    editorDevis.PutString("Devise", toast);
                    editorDevis.Apply();

                    Intent intent = new Intent(this, typeof(CreationClient3));

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