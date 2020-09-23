using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;
using System;
using System.IO;

namespace Facturation
{
    [Activity(Label = "Produit/Service", Theme = "@style/AppTheme.NoActionBar")]
    public class ProduitDevis : AppCompatActivity
    {
        public string toast;

        public void InsetionProduit(string nom, float prix, string type)
        {
            string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Insert into Produit(Nomproduit,Prix,typePro) Values('" + nom + "'," + prix + ",'" + type + "')";
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

                var spiner = FindViewById<Spinner>(Resource.Id.spinnerchoix);




                var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.Produit, Android.Resource.Layout.SimpleSpinnerItem);

                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

                spiner.Adapter = adapter;
                spiner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

                //def les contrelleur 
              //  var codebarre = FindViewById<EditText>(Resource.Id.editTextcodebarre);

                var produitNom = FindViewById<EditText>(Resource.Id.textInputEditTextnomproduit);

                var prix = FindViewById<EditText>(Resource.Id.editTextPrix);





                var btnenregitProduit = FindViewById<Button>(Resource.Id.buttonproduit);


                btnenregitProduit.Click += delegate
                {


                    InsetionProduit(produitNom.Text, float.Parse(prix.Text), toast);

                    Intent intent = new Intent(this, typeof(ListProduitsDevis));

                    StartActivity(intent);


                };


            }
            catch (Exception ex)
            {

                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}