using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using Android.Preferences;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Facturation.Class;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace Facturation
{
    [Activity(Label = "Liste Produits/Services", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class ListProduits : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        AdView ads1, ads2;
        private List<Clients> selectionInfoEntreprise(string email)
        {
            List<Clients> clientsta = new List<Clients>();

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@email", email));
            contenu.CommandText = "SELECT  NomClinet, Emailclient ,Logo FROM ClientAdmin WHERE Emailclient =@email";



            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                if (r != null)
                {

                    clientsta.Add(new Clients(r[0].ToString(), r[1].ToString(), (byte[])r[2]));
                }
            }


            return clientsta;



        }



        ListView tara;
        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();
            var ListView = sender as ListView;
            var t = selectionlistProduitService()[e.Position];
            int c = e.Position + 1;
            Intent intent = new Intent(this, typeof(DetailFacture));
            editorfacture.PutInt("IDPRODUIT", t.Idproduit);
            editorfacture.PutString("NomArticle", t.Nomproduit);
            editorfacture.PutString("Type", t.Nomproduit);
            editorfacture.PutFloat("Prix", t.Prix);
            editorfacture.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));

            editorfacture.PutInt("Item", c);
            editorfacture.Apply();

            intent.PutExtra("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));

            StartActivity(intent);
           // Android.Widget.Toast.MakeText(this, c.ToString(), Android.Widget.ToastLength.Short).Show();
        }
        private List<ProduitClass> selectionlistProduitService()
        {
            List<ProduitClass> produit = new List<ProduitClass>();

            string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select idProduit,typePro,Nomproduit,Prix  from Produit";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                produit.Add(new ProduitClass(int.Parse(r[0].ToString()), r[1].ToString(), r[2].ToString(), float.Parse(r[3].ToString())));


            }

            connection.Close();
            return produit;



        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorDevis = sharedPreferences.Edit();
                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.ListProduits);


                SetContentView(Resource.Layout.ListProduits);
                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

                SetSupportActionBar(toolbar);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                toolbar.Title = "Liste Produits";


                DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

                Android.Support.V7.App.ActionBarDrawerToggle toggle = new Android.Support.V7.App.ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);

                tara = FindViewById<ListView>(Resource.Id.listViewVpsHebergement);



                var Ajouter = FindViewById<FloatingActionButton>(Resource.Id.floatingActionButton);

                tara.Adapter = new HomeScreenAdapterProduit(this, selectionlistProduitService());

                tara.ItemClick += OnListItemClick;

                Ajouter.Click += delegate
                {
                    editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                    editorDevis.Apply();

                    Intent intent = new Intent(this, typeof(Produit));
                    intent.PutExtra("Emailadmin",sharedPreferences.GetString("Emailadmin", ""));
                    StartActivity(intent);


                };

                drawer.AddDrawerListener(toggle);

                toggle.SyncState();

                NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
                navigationView.SetNavigationItemSelectedListener(this);
                var hederview = navigationView.GetHeaderView(0);

                var imagelogo = hederview.FindViewById<ImageView>(Resource.Id.imageViewnavheaderlogo);

                var EmailHeader = hederview.FindViewById<TextView>(Resource.Id.EmailHeder);

                var NomEntreprise = hederview.FindViewById<TextView>(Resource.Id.NomHeader);

                AdView ads1, ads2;

                ads1 = FindViewById<AdView>(Resource.Id.adView4ListProduit);
                ads2 = FindViewById<AdView>(Resource.Id.adView2ListProduit);

                var adRequest1 = new AdRequest.Builder().Build();
                ads1.LoadAd(adRequest1);
                var adRequest2 = new AdRequest.Builder().Build();
                ads2.LoadAd(adRequest2);



                imagelogo.SetImageBitmap(Android.Graphics.BitmapFactory.DecodeByteArray(selectionInfoEntreprise(sharedPreferences.GetString("Emailadmin", ""))[0].Logo, 0, selectionInfoEntreprise(sharedPreferences.GetString("Emailadmin", ""))[0].Logo.Length));

                EmailHeader.Text = selectionInfoEntreprise(sharedPreferences.GetString("Emailadmin", ""))[0].Email;

                NomEntreprise.Text = "Bonjour " + selectionInfoEntreprise(sharedPreferences.GetString("Emailadmin", ""))[0].NomClinet;

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }
        }
        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorDevis = sharedPreferences.Edit();

            int id = item.ItemId;
            if (id == Resource.Id.Home)
            {
                Intent intent = new Intent(this, typeof(Itemsapp));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();

                string m = Intent.GetStringExtra("Emailadmin");

                intent.PutExtra("Emailadmin", m);


                StartActivity(intent);

            }
            if (id == Resource.Id.Clients)
            {
                Intent intent = new Intent(this, typeof(ListCLientsStandart));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();

                string m = Intent.GetStringExtra("Emailadmin");

                intent.PutExtra("Emailadmin", m);

                StartActivity(intent);

            }
            else if (id == Resource.Id.Facture)
            {
                Intent intent = new Intent(this, typeof(Listacture));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();
                string m = Intent.GetStringExtra("Emailadmin");

                intent.PutExtra("Emailadmin", m);

                StartActivity(intent);

            }
            else if (id == Resource.Id.Devis)
            {
                Intent intent = new Intent(this, typeof(ListDevis));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();

                string m = Intent.GetStringExtra("Emailadmin");

                intent.PutExtra("Emailadmin", m);

                StartActivity(intent);
            }
            else if (id == Resource.Id.Produits)
            {
                Intent intent = new Intent(this, typeof(ListProduitsStandart));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();

                string m = Intent.GetStringExtra("Emailadmin");

                intent.PutExtra("Emailadmin", m);

                StartActivity(intent);
            }
            else if (id == Resource.Id.nav_share)
            {
                Etatconx();
                Intent intent = new Intent(this, typeof(PageHome));
                string m = Intent.GetStringExtra("Emailadmin");

                intent.PutExtra("Emailadmin", m);


                StartActivity(intent);

            }
            else if (id == Resource.Id.nav_parametre)
            {
                Intent intent = new Intent(this, typeof(parametre));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();

                string m = Intent.GetStringExtra("Emailadmin");

                intent.PutExtra("Emailadmin", m);

                StartActivity(intent);
            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;


        }
        private void Etatconx()
        {
            string email = "";

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();

            // contenu.Parameters.Add(new SqliteParameter("@email", Email));
            contenu.CommandText = "Update Clientadmin  set Conexteat='N' where Role='Admin' ";


            contenu.ExecuteNonQuery();

        }
    }
}