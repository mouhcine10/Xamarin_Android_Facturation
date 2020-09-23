using Android.App;
using Android.Content;
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
using Java.Sql;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;

namespace Facturation
{
    [Activity(Label = "Liste Devis", Theme = "@style/AppTheme.NoActionBar")]
    public class ListDevisHome : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        static int i = 0;
        int IDD;

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


        public int selectionCountDevis()
        {

            string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select Max(idDevis), count(*),Max(rowid) from  Devis";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {
                if (int.Parse(r[1].ToString()) == 0)
                {

                    i = int.Parse(r[1].ToString());

                }
                else
                {

                    i = int.Parse(r[0].ToString());
                }

            }

            connection.Close();
            return i;

        }
        private List<Devis> selectionlistDevis()
        {
            List<Devis> devis = new List<Devis>();

            string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();

            var contenu = connection.CreateCommand();

            contenu.CommandText = "SELECT Devis.idDevis, DateValidation,NomClinet, Etat ,SUM((Prixht * Qte) + ((Prixht * Qte) * (TVA / 100)) - Remise),Devise from Devis INNER JOIN commandeDevis on Devis.idDevis = commandeDevis.IdDevis INNER JOIN   Client on Devis.idclient = Client.idclient WHERE Devis.Etat='Validé' GROUP by Devis.idDevis";
            contenu.ExecuteNonQuery();
            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                if (int.Parse(r[0].ToString()) != null && r[1].ToString() != null && r[2].ToString() != null && r[3].ToString() != null && float.Parse(r[4].ToString()) != null && r[5].ToString() != null)
                {
                    devis.Add(new Devis(int.Parse(r[0].ToString()), r[1].ToString(), r[2].ToString(), r[3].ToString(), float.Parse(r[4].ToString()), r[5].ToString()));

                }
                else
                {

                    Toast.MakeText(this, "Ok", ToastLength.Long).Show();

                }
            }

            connection.Close();
            return devis;



        }
        private List<Devis> selectionlistDevis1()
        {
            List<Devis> devis = new List<Devis>();

            string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();

            var contenu = connection.CreateCommand();

            contenu.CommandText = "SELECT Devis.idDevis, DateValidation,NomClinet, Etat ,SUM((Prixht * Qte) + ((Prixht * Qte) * (TVA / 100)) - Remise),Devise from Devis INNER JOIN commandeDevis on Devis.idDevis = commandeDevis.IdDevis INNER JOIN   Client on Devis.idclient = Client.idclient WHERE Devis.Etat='Validé' GROUP by Devis.idDevis";
            contenu.ExecuteNonQuery();
            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                if (int.Parse(r[0].ToString()) != null && r[1].ToString() != null && r[2].ToString() != null && r[3].ToString() != null && float.Parse(r[4].ToString()) != null && r[5].ToString() != null)
                {
                    devis.Add(new Devis(int.Parse(r[0].ToString()), r[1].ToString(), r[2].ToString(), r[3].ToString(), float.Parse(r[4].ToString()), r[5].ToString()));

                }
                else
                {

                    Toast.MakeText(this, "Ok", ToastLength.Long).Show();

                }
            }

            connection.Close();
            return devis;



        }
        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorDevis = sharedPreferences.Edit();
            var ListView = sender as ListView;
            var t = selectionlistDevis()[e.Position];
            int c = e.Position + 1;
            Intent intent = new Intent(this, typeof(UpdateDevis));//Facturee));
            //editorfacture.PutString("titre", t.NomClinet);
            //editorfacture.PutString("Typeclient", t.TypeClinet);
            //editorfacture.PutInt("IDclient", t.Idclient);
            //// editorfacture.PutInt("Item", c);
            editorDevis.PutInt("IDDevis", t.Nemrodevis);
            editorDevis.PutInt("Idevisinfo", InfoDevis(t.Nemrodevis)[0].Nemrodevis);
            editorDevis.PutString("DateCreation", InfoDevis(t.Nemrodevis)[0].Datecreation);
            editorDevis.PutString("DateValidation", InfoDevis(t.Nemrodevis)[0].Datefin);
            editorDevis.PutString("NomClient", t.Clientnom);
            editorDevis.PutInt("Idlient", t.Idclient);
            editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));

            string ma = Intent.GetStringExtra("Emailadmin");

            intent.PutExtra("Emailadmin", ma);
            editorDevis.Apply();
            StartActivity(intent);
            Android.Widget.Toast.MakeText(this, c.ToString(), Android.Widget.ToastLength.Short).Show();
        }
        private List<Devis> InfoDevis(int iddevis)
        {
            List<Devis> devis = new List<Devis>();
            //clients.Clear();

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();

            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@Iddevis", iddevis));

            contenu.CommandText = "SELECT idDevis, DateCreation, DateValidation  FROM Devis WHERE idDevis=@Iddevis";

            var r = contenu.ExecuteReader();
            while (r.Read())
            {
                devis.Add(new Devis(int.Parse(r[0].ToString()), r[1].ToString(), r[2].ToString()));
            }


            return devis;

        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {

                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorDevis = sharedPreferences.Edit();

                IDD = selectionCountDevis() + 1;


                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.ListDevise);
                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

                SetSupportActionBar(toolbar);
                SupportActionBar.Title = "List Devis";
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                toolbar.Title = "Liste Devis";


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

                ads1 = FindViewById<AdView>(Resource.Id.adView1ListDevis);
                ads2 = FindViewById<AdView>(Resource.Id.adView2ListDevis);
                var adRequest1 = new AdRequest.Builder().Build();
                ads1.LoadAd(adRequest1);
                var adRequest2 = new AdRequest.Builder().Build();
                ads2.LoadAd(adRequest2);



                imagelogo.SetImageBitmap(Android.Graphics.BitmapFactory.DecodeByteArray(selectionInfoEntreprise(sharedPreferences.GetString("Emailadmin", ""))[0].Logo, 0, selectionInfoEntreprise(sharedPreferences.GetString("Emailadmin", ""))[0].Logo.Length));

                EmailHeader.Text = selectionInfoEntreprise(sharedPreferences.GetString("Emailadmin", ""))[0].Email;

                NomEntreprise.Text = "Bonjour " + selectionInfoEntreprise(sharedPreferences.GetString("Emailadmin", ""))[0].NomClinet;



                var Ajouter = FindViewById<FloatingActionButton>(Resource.Id.floatingActionButton);

                var listDevis = FindViewById<ListView>(Resource.Id.listViewVpsHebergement);


                // List<Facturation.Class.Devis> devis = new List<Class.Devis>() { new Class.Devis("Dev1", "CFAO", "En cours", DateTime.Parse("10-10-2020"), DateTime.Parse("10-11-2020"), 500) };

                if (selectionCountDevis() == 0)
                {

                    listDevis.Adapter = new HomeScreenAdapterDevis(this, selectionlistDevis1());
                }
                else
                {

                    listDevis.Adapter = new HomeScreenAdapterDevis(this, selectionlistDevis());

                }
                listDevis.ItemClick += OnListItemClick;
                Ajouter.Click += delegate
                {
                    editorDevis.PutInt("IDDevis", selectionCountDevis() + 1);
                    editorDevis.PutString("Entreprise", "Moi");
                    editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                    editorDevis.PutString("DateCreation", DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year);
                    editorDevis.PutString("DateValidite", DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year);
                    editorDevis.PutString("Article", " ");
                    editorDevis.PutFloat("Qte", 0);
                    editorDevis.PutFloat("Prix", 0);
                    editorDevis.PutString("titre", "Choisier votre client ");
                    editorDevis.PutFloat("Tauxremise", 0);
                    editorDevis.PutString("ImagePath", " ");
                    editorDevis.PutString("Observation", " ");
                    editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));


                    editorDevis.Apply();
                    byte[] t = null;
                    Intent intent = new Intent(this, typeof(Deviss));
                    intent.PutExtra("ImagePath", t);
                    intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                    StartActivity(intent);


                };

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
                string m = Intent.GetStringExtra("Emailadmin");
                Intent intent = new Intent(this, typeof(Itemsapp));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();


                intent.PutExtra("Emailadmin", m);
                StartActivity(intent);

            }

            if (id == Resource.Id.Clients)
            {
                string m = Intent.GetStringExtra("Emailadmin");
                Intent intent = new Intent(this, typeof(ListCLientsStandart));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();


                intent.PutExtra("Emailadmin", m);


                StartActivity(intent);

            }
            else if (id == Resource.Id.Facture)
            {
                string m = Intent.GetStringExtra("Emailadmin");
                Intent intent = new Intent(this, typeof(Listacture));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();


                intent.PutExtra("Emailadmin", m);

                StartActivity(intent);

            }
            else if (id == Resource.Id.Devis)
            {
                string m = Intent.GetStringExtra("Emailadmin");
                Intent intent = new Intent(this, typeof(ListDevis));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();


                intent.PutExtra("Emailadmin", m);

                StartActivity(intent);
            }
            else if (id == Resource.Id.Produits)
            {
                string m = Intent.GetStringExtra("Emailadmin");
                Intent intent = new Intent(this, typeof(ListProduitsStandart));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();



                intent.PutExtra("Emailadmin", m);

                StartActivity(intent);
            }
            else if (id == Resource.Id.nav_share)
            {
                string m = Intent.GetStringExtra("Emailadmin");
                Intent intent = new Intent(this, typeof(PageHome));
                Etatconx();


                intent.PutExtra("Emailadmin", m);

                StartActivity(intent);

            }
            else if (id == Resource.Id.nav_parametre)
            {
                string m = Intent.GetStringExtra("Emailadmin");
                Intent intent = new Intent(this, typeof(parametre));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();


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