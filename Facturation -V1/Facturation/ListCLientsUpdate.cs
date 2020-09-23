using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace Facturation.Class
{
    [Activity(Label = "List CLients", Theme = "@style/AppTheme.NoActionBar")]
    public class ListCLientsUpdate : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
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


        List<Clients> clients = new List<Clients>();
        private List<Clients> Clientsinfos()
        {

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select idClient,TypeClinet , NomClinet from Client";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                clients.Add(new Class.Clients(int.Parse(r[0].ToString()), r[1].ToString(), r[2].ToString()));

            }

            return clients;

        }

        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        { byte[] ta = Intent.GetByteArrayExtra("ImagePath");
            string ob = Intent.GetStringExtra("ob");
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();
            var ListView = sender as ListView;
            var t = Clientsinfos()[e.Position];
            int c = e.Position + 1;
            Intent intent = new Intent(this, typeof(Updatefacture));
            editorfacture.PutString("titre", t.NomClinet);
            editorfacture.PutString("Typeclient", t.TypeClinet);
            editorfacture.PutInt("IDclient", t.Idclient);
            editorfacture.PutString("ob", ob);
            // editorfacture.PutInt("Item", c);
            editorfacture.Apply();
           
            intent.PutExtra("ImagePath", ta);
          
            StartActivity(intent);
            Android.Widget.Toast.MakeText(this, c.ToString(), Android.Widget.ToastLength.Short).Show();
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorDevis = sharedPreferences.Edit();
                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.ListClient);
                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

                SetSupportActionBar(toolbar);

                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                toolbar.Title = "Liste Clients";

                //this.ActionBar.NavigationMode= ActionBarNavigationMode.Tabs;
                //var tab = this.ActionBar.NewTab();
                //tab.SetText("tabText");
                //tab.SetIcon(Resource.Drawable.abc_ic_ab_back_material);



                //var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
                //toolbar.SetTitleTextColor(Android.Graphics.Color.White);
                //toolbar.SetBackgroundColor(Android.Graphics.Color.RoyalBlue);


                DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
                Android.Support.V7.App.ActionBarDrawerToggle toggle = new Android.Support.V7.App.ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);

                drawer.AddDrawerListener(toggle);
                toggle.SyncState();

                NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
                navigationView.SetNavigationItemSelectedListener(this);
                var hederview = navigationView.GetHeaderView(0);

                var imagelogo = hederview.FindViewById<ImageView>(Resource.Id.imageViewnavheaderlogo);

                var EmailHeader = hederview.FindViewById<TextView>(Resource.Id.EmailHeder);

                var NomEntreprise = hederview.FindViewById<TextView>(Resource.Id.NomHeader);

                ads1 = FindViewById<AdView>(Resource.Id.adView1Listclient);
                ads2 = FindViewById<AdView>(Resource.Id.adView2Listclient);

                var adRequest1 = new AdRequest.Builder().Build();
                ads1.LoadAd(adRequest1);
                var adRequest2 = new AdRequest.Builder().Build();
                ads2.LoadAd(adRequest2);


                imagelogo.SetImageBitmap(BitmapFactory.DecodeByteArray(selectionInfoEntreprise(sharedPreferences.GetString("Emailadmin", ""))[0].Logo, 0, selectionInfoEntreprise(sharedPreferences.GetString("Emailadmin", ""))[0].Logo.Length));

                EmailHeader.Text = selectionInfoEntreprise(sharedPreferences.GetString("Emailadmin", ""))[0].Email;

                NomEntreprise.Text = "Bonjour " + selectionInfoEntreprise(sharedPreferences.GetString("Emailadmin", ""))[0].NomClinet;


                var Ajouter = FindViewById<FloatingActionButton>(Resource.Id.floatingActionButton);

                var listClients = FindViewById<ListView>(Resource.Id.listViewVpsHebergement);




                listClients.Adapter = new HomeScreenAdapter(this, Clientsinfos());

                listClients.ItemClick += OnListItemClick;

                Ajouter.Click += delegate
                {


                    Intent intent = new Intent(this, typeof(Clientss));

                    StartActivity(intent);


                };

            }catch(Exception ex)
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

                intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                StartActivity(intent);

            }

            if (id == Resource.Id.Clients)
            {
                Intent intent = new Intent(this, typeof(ListCLientsStandart));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();

                StartActivity(intent);

            }
            else if (id == Resource.Id.Facture)
            {
                Intent intent = new Intent(this, typeof(Listacture));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();

                StartActivity(intent);

            }
            else if (id == Resource.Id.Devis)
            {
                Intent intent = new Intent(this, typeof(ListDevis));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();
                StartActivity(intent);
            }
            else if (id == Resource.Id.Produits)
            {
                Intent intent = new Intent(this, typeof(ListProduitsStandart));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();
                StartActivity(intent);
            }
            else if (id == Resource.Id.nav_share)
            {
                Etatconx();
                Intent intent = new Intent(this, typeof(PageHome));
                StartActivity(intent);
            }
            else if (id == Resource.Id.nav_parametre)
            {
                Intent intent = new Intent(this, typeof(parametre));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();
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