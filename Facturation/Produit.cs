using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using Android.Preferences;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;
using System;
using System.IO;

namespace Facturation
{
    [Activity(Label = "Produit/Service", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]


    public class Produit : AppCompatActivity
    {
        public string toast;

        AdView ads1, ads2, ads3;
        EditText produitNom;

        EditText prix;


        EditText tav;


        private void deleteproduit(int idproduit)
        {
            string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@Idproduit", idproduit));
            contenu.CommandText = "Delete from Produit where idProduit=@Idproduit";
            contenu.ExecuteNonQuery();
            connection.Close();


        }
        private void updateproduit(string nom, float prix, string type, float tva, int id)
        {
            string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@nom", nom));
            contenu.Parameters.Add(new SqliteParameter("@prix", prix));
            contenu.Parameters.Add(new SqliteParameter("@type", type));
            contenu.Parameters.Add(new SqliteParameter("@tva", tva));
            contenu.Parameters.Add(new SqliteParameter("@id", id));
            contenu.CommandText = "Update Produit set Nomproduit=@nom ,Prix=@prix ,typePro=@type,tva=@tva where  idProduit=@id";
            contenu.ExecuteNonQuery();
            connection.Close();


        }
        public void InsetionProduit(string nom, float prix, string type, float tva)
        {
            string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Insert into Produit(Nomproduit,Prix,typePro,tva) Values('" + nom + "'," + prix + ",'" + type + "'," + tva + ")";
            contenu.ExecuteNonQuery();
            connection.Close();


        }



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
                ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();
                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.Produits);

                //var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
                //toolbar.SetTitleTextColor(Android.Graphics.Color.White);
                //toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(40, 195, 195));
                //SetActionBar(toolbar);
                //ActionBar.Title = "Produit/Service";
                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

                SetSupportActionBar(toolbar);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                var spiner = FindViewById<Spinner>(Resource.Id.spinnerchoix);




                var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.Produit, Android.Resource.Layout.SimpleSpinnerItem);

                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

                spiner.Adapter = adapter;
                spiner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

                //def les contrelleur 
                //  var codebarre = FindViewById<EditText>(Resource.Id.editTextcodebarre);

                produitNom = FindViewById<EditText>(Resource.Id.textInputEditTextnomproduit);

                prix = FindViewById<EditText>(Resource.Id.editTextPrix);


                tav = FindViewById<EditText>(Resource.Id.editTextTVAProduit);



                produitNom.Text = sharedPreferences.GetString("NomProduit", "");

                prix.Text = sharedPreferences.GetFloat("PRIX", 0).ToString();

                tav.Text = sharedPreferences.GetFloat("TVA", 0).ToString();

                ads1 = FindViewById<AdView>(Resource.Id.adView1prod);
                ads2 = FindViewById<AdView>(Resource.Id.adView2prod);
                ads3 = FindViewById<AdView>(Resource.Id.adView3prod);

                var adRequest1 = new AdRequest.Builder().Build();
                ads1.LoadAd(adRequest1);
                var adRequest2 = new AdRequest.Builder().Build();
                ads2.LoadAd(adRequest2);

                var adrequest3 = new AdRequest.Builder().Build();
                ads3.LoadAd(adrequest3);

                var btnenregitProduit = FindViewById<Button>(Resource.Id.buttonproduit);


                btnenregitProduit.Click += delegate
                {


                    InsetionProduit(produitNom.Text, float.Parse(prix.Text), toast, float.Parse(tav.Text));

                    prix.Text = "";
                    produitNom.Text = "";
                    tav.Text = "";

                    Intent intent = new Intent(this, typeof(ListProduitsStandart));
                    editorfacture.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                    editorfacture.Apply();

                    string m = Intent.GetStringExtra("Emailadmin");
                    intent.PutExtra("Emailadmin", m);

                    StartActivity(intent);



                };


            }
            catch (Exception ex)
            {

                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }

        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menupr, menu);
            return true;
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();
            if (item.ItemId == Android.Resource.Id.Home)
            {
                editorfacture.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                Intent intent = new Intent(this, typeof(ListProduitsStandart));

                intent.PutExtra("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));

                StartActivity(intent);

            }

            if (item.ItemId == Resource.Id.Modifier)
            {
                updateproduit(produitNom.Text, float.Parse(prix.Text), toast, float.Parse(tav.Text), sharedPreferences.GetInt("IDPRODUIT", 0));
                Intent intent = new Intent(this, typeof(ListProduitsStandart));
                editorfacture.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorfacture.Apply();
                string m = Intent.GetStringExtra("Emailadmin");
                intent.PutExtra("Emailadmin", m);

                StartActivity(intent);

            }
            if (item.ItemId == Resource.Id.Supprimer)
            {

                deleteproduit(sharedPreferences.GetInt("IDPRODUIT", 0));
                editorfacture.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorfacture.Apply();

                Intent intent = new Intent(this, typeof(ListProduitsStandart));
                string m = Intent.GetStringExtra("Emailadmin");
                intent.PutExtra("Emailadmin", m);
                StartActivity(intent);
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}