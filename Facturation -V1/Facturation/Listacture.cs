using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V4.View.Animation;
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
    [Activity(Label = "List Factures", Theme = "@style/AppTheme.NoActionBar")]
    public class Listacture : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        readonly string[] permissionGroupe = { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
        const int Requestid = 0;
        int id = 0;
        string ma = " ";
        AdView ads1, ads2;

        //private void DeleteFacture(int IDFacture)
        //{
        //    string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");

        //    var connection = new SqliteConnection("Data Source=" + dbpath);

        //    connection.Open();
        //    var contenu = connection.CreateCommand();

        //    contenu.CommandText = "Delete from Facture where IDfacture=" + IDFacture;


        //    contenu.ExecuteNonQuery();

        //    connection.Close();

        //}
        //private void DeleteCommandeFacture(int IDFacture)
        //{
        //    string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");

        //    var connection = new SqliteConnection("Data Source=" + dbpath);

        //    connection.Open();
        //    var contenu = connection.CreateCommand();

        //    contenu.CommandText = "Delete from commandeFacture where IDfacture=" + IDFacture;


        //    contenu.ExecuteNonQuery();

        //    connection.Close();

        //}
        //bool CheckLocationPermission()
        //{
        //    bool PermissionGranted = false;
        //    if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Android.Content.PM.Permission.Granted && ActivityCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != Android.Content.PM.Permission.Granted)
        //    {
        //        PermissionGranted = false;
        //        RequestPermissions(permissionGroupe, Requestid);
        //    }
        //    else
        //    {
        //        PermissionGranted = true;
        //    }
        //    return PermissionGranted;
        //}
        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        //{
        //    if (grantResults[0] == (int)Android.Content.PM.Permission.Granted)
        //    {
        //        Toast.MakeText(this, "Permission  was granted", ToastLength.Long).Show();
        //    }
        //    else
        //    {
        //        Toast.MakeText(this, "Permission  was dinaid", ToastLength.Long).Show();
        //    }
        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}




        List<Facture> factures = new List<Facture>();
        static int i = 0;
        public string selectionnomENreprise()
        {
            string nom = "";
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select ,NomClinet as Nom from  Client where role=admin";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                nom = r[0].ToString();

            }
            r.Close();
            connection.Close();
            return nom;


        }
        //(idFacture INTEGER primary key AUTOINCREMENT, DateFacturation Date,Datetranform date ,Devise ntext, idClient INTEGER  REFERENCES Client(IdClient) ,Etat ntext ,modepaiement ntext,observation ntext)
        private List<Facture> selectionlistfacture()
        {
            factures.Clear();
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "SELECT  Facture.idFacture, NomClinet ,SUM((Prixht*Qte)+((Prixht*Qte)*(TVA/100))-Remise),DateFacturation, DateEchance,Etat,Devise,DatePaiement FROM Facture INNER JOIN commandeFacture on Facture.idFacture=commandeFacture.idFacture INNER JOIN Client on Client.idClient=Facture.idClient GROUP by Facture.idFacture";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {


                factures.Add(new Facture(int.Parse(r[0].ToString()), r[1].ToString(), float.Parse(r[2].ToString()), r[3].ToString(), r[4].ToString(), r[5].ToString(), r[6].ToString(), r[7].ToString()));

            }


            return factures;



        }
        private List<Facture> selectionlistfacture1()
        {
            factures.Clear();
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "SELECT  Facture.idFacture, NomClinet ,SUM((Prixht*Qte)+((Prixht*Qte)*(TVA/100))-Remise),DateFacturation, DateEchance,Etat,Devise,DatePaiement FROM Facture INNER JOIN commandeFacture on Facture.idFacture=commandeFacture.idFacture INNER JOIN Client on Client.idClient=Facture.idClient GROUP by Facture.idFacture";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {


                factures.Add(new Facture(int.Parse(r[0].ToString()), r[1].ToString(), float.Parse(r[2].ToString()), r[3].ToString(), r[4].ToString(), r[5].ToString(), r[6].ToString(), r[7].ToString()));

            }


            return factures;



        }

        List<Clients> clients = new List<Clients>();
        private List<Clients> selectionInfoEntreprise(string email)
        {
            factures.Clear();
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

                    clients.Add(new Clients(r[0].ToString(), r[1].ToString(), (byte[])r[2]));
                }
            }


            return clients;



        }


        public int selectionCountFacture()
        {

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select Max(idFacture), count(*),Max(rowid) from  Facture";

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
            r.Close();
            connection.Close();
            return i;

        }
        public void CreationDatabase()
        {

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            bool Exists = File.Exists(dbpath);


            if (!Exists)
            {

                Mono.Data.Sqlite.SqliteConnection.CreateFile(dbpath);
                List<string> commands = new List<string>() { "Create Table Client( idClient INTEGER primary key AUTOINCREMENT, TypeClinet ntext,NomClinet ntext ,Telephone NUMERIC,Emailclient ntext,adresse ntext,CodePostal ntext, ville ntext, pays ntext,Logo text ,Mdp text,confirmdp text,role text)",
                    "Create Table Banck(IDInfobanck INTEGER primary key AUTOINCREMENT, Nombanc ntext ,BIC ntext,IBAN ntext,idclient INTEGER  REFERENCES Client(idClient) on delete cascade)",

                    "Create Table Facture( idFacture INTEGER primary key, DateFacturation ntext,Datetranform ntext ,DateEchance ntext ,DatePaiement ntext ,Devise ntext, idClient INTEGER  REFERENCES Client(IdClient) ,Etat ntext ,modepaiement ntext,observation ntext,signature BLOB)",
                    //"Create Table Imagefact(Idimage INTEGER primary key AUTOINCREMENT,Imagevvide BLOB, IdFacture INTEGER REFERENCES Facture(idFacture) on delete cascade)",

                    "Create Table commandeFacture(Idcomfact INTEGER primary key AUTOINCREMENT, IdFacture INTEGER REFERENCES Facture(idFacture) on delete cascade, Libille ntext,TypeProd ntext , PrixHT REAL,Qte INTEGER, TVA REAL,Remise REAL)",

                    "Create Table Devis(idDevis INTEGER primary key AUTOINCREMENT ,DateCreation ntext,DateValidation ntext ,Datetranform ntext, Devise ntext, idClient INTEGER  REFERENCES Client(IdClient),Etat ntext, modepaiement ntext ,observation ntext,signature BLOB)",

                    "Create Table commandeDevis(idcomdev INTEGER primary key AUTOINCREMENT, IdDevis INTEGER REFERENCES Devis(idDevis)on delete cascade ,libille ntext,TypeProd ntext, Prixht REAL, Qte INTEGER, TVA INTEGER  , Remise REAL)",
                    "Create Table Produit(idProduit INTEGER primary key AUTOINCREMENT, Nomproduit ntext ,Prix REAL,typePro ntext,codebar ntext)",

                    //"Create Table FilesFact(idFailefact INTEGER primary key AUTOINCREMENT, Nomfile ntext,Pathfile ntext,IdFacture INTEGER REFERENCES Facture(idFacture) on delete cascade)",

                    //"Create Table FilesDev(idFaileDev INTEGER primary key AUTOINCREMENT, Nomfile ntext,Pathfile ntext,IdDevis INTEGER REFERENCES Devis(idFacture) on delete cascade)"


                };
                connection.Open();

                var command = connection.CreateCommand();
                for (int i = 0; i < commands.Count; i++)
                {

                    command.CommandText = commands[i];
                    command.ExecuteNonQuery();
                    Toast.MakeText(this, commands[i].ToString(), ToastLength.Long).Show();


                }


                connection.Close();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //try
            //{

            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();
            id = selectionCountFacture() + 1;
            base.OnCreate(savedInstanceState);
            //if (CheckLocationPermission())
            //{

            //    var pathToNewFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/FolderName";
            //    Directory.CreateDirectory(pathToNewFolder);
            //    Toast.MakeText(this, File.Exists(pathToNewFolder).ToString(), ToastLength.Long).Show();
            //}

            SetContentView(Resource.Layout.ListFacture);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
            toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
            toolbar.Title = "Liste Factures";


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

            ads1 = FindViewById<AdView>(Resource.Id.adView4ListFacture);
            ads2 = FindViewById<AdView>(Resource.Id.adView5ListFacture);
            var adRequest1 = new AdRequest.Builder().Build();
            ads1.LoadAd(adRequest1);
            var adRequest2 = new AdRequest.Builder().Build();
            ads2.LoadAd(adRequest2);

            ma = Intent.GetStringExtra("Emailadmin");

            imagelogo.SetImageBitmap(BitmapFactory.DecodeByteArray(selectionInfoEntreprise(ma)[0].Logo, 0, selectionInfoEntreprise(ma)[0].Logo.Length));

            EmailHeader.Text = selectionInfoEntreprise(ma)[0].Email;

            NomEntreprise.Text = "Bonjour " + selectionInfoEntreprise(ma)[0].NomClinet;

            var Ajouter = FindViewById<FloatingActionButton>(Resource.Id.floatingActionButton);

            var listFactures = FindViewById<ListView>(Resource.Id.listViewVpsHebergement);

            // List<Facture> facturees = new List<Facture>() { new Facture("fac1", "CFAO", 500, DateTime.Parse("12-08-2015"), "Payé") };


            //if (selectionCountFacture() == 0)
            //{
            //    listFactures.Adapter = new HomeScreenAdapterFacture(this, selectionlistfacture1());

            //}
            //else 
            if (selectionCountFacture() > 0)
            {
                listFactures.Adapter = new HomeScreenAdapterFacture(this, selectionlistfacture());

            }


            listFactures.ItemClick += OnListItemClick;



            Ajouter.Click += delegate
            {
                editorfacture.PutInt("IDfacture", selectionCountFacture() + 1);
                editorfacture.PutString("Entreprise", "Moi");
                editorfacture.PutString("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                editorfacture.PutString("DateCreation", DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year);
                editorfacture.PutString("DateEcheance", DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year);
                editorfacture.PutString("Article", "");
                editorfacture.PutFloat("Qte", 0);
                editorfacture.PutFloat("Prix", 0);
                editorfacture.PutString("titre", "Choisier votre client ");
                editorfacture.PutFloat("Tauxremise", 0);
                editorfacture.PutString("ImagePath", " ");
                editorfacture.PutString("Observation", " ");
                editorfacture.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));

                editorfacture.Apply();


                Intent intent = new Intent(this, typeof(Facturee));
                intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));

                byte[] t = null;
                intent.PutExtra("ImagePath", t);

                StartActivity(intent);


                Toast.MakeText(this, id.ToString(), ToastLength.Long).Show();


            };
        }

        //}
        //catch (Exception ex)
        //{
        //    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();


        //    }




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
                intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));

                StartActivity(intent);

            }
            else if (id == Resource.Id.Facture)
            {
                Intent intent = new Intent(this, typeof(Listacture));

                intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                StartActivity(intent);
            }
            else if (id == Resource.Id.Devis)
            {
                Intent intent = new Intent(this, typeof(ListDevis));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();
                intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                StartActivity(intent);

            }
            else if (id == Resource.Id.Produits)
            {
                Intent intent = new Intent(this, typeof(ListProduitsStandart));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();

                intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                StartActivity(intent);
            }
            else if (id == Resource.Id.nav_share)
            {
                Intent intent = new Intent(this, typeof(PageHome));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();

                Etatconx();
                intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                StartActivity(intent);
            }
            else if (id == Resource.Id.nav_parametre)
            {
                Intent intent = new Intent(this, typeof(parametre));
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();

                intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
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
        private List<Facture> selectioninfo(int idfacture)
        {
            List<Facture> factures = new List<Facture>();
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");

            var connection = new SqliteConnection("Data Source=" + dbpath);

            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idfacture", idfacture));

            contenu.CommandText = "SELECT idFacture, DateFacturation ,DateEchance, modepaiement,observation  FROM Facture WHERE idFacture=@idfacture";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                factures.Add(new Facture(int.Parse(r[0].ToString()), r[1].ToString(), r[2].ToString(), r[3].ToString(), r[4].ToString()));

            }



            return factures;
        }
        private string Clientsinfos(string Email)
        {
            //clients.Clear();

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();

            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idclient", Email));

            contenu.CommandText = "Select NomClinet from ClientAdmin where Emailclient=@idclient";

            string r = (string)contenu.ExecuteScalar();



            return r;

        }
        private List<Facture> InfoFact(int idfacture)
        {
            List<Facture> factures = new List<Facture>();
            //clients.Clear();

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();

            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idfacture", idfacture));

            contenu.CommandText = "SELECT idFacture, DateFacturation, DateEchance FROM Facture  where  idFacture= @idfacture";

            var r = contenu.ExecuteReader();
            while (r.Read())
            {
                factures.Add(new Facture(int.Parse(r[0].ToString()), r[1].ToString(), r[2].ToString()));
            }


            return factures;

        }
        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();
                var ListView = sender as ListView;
                var t = selectionlistfacture()[e.Position];

                Intent intent = new Intent(this, typeof(Updatefacture));//Facturee));
                editorfacture.PutInt("IDfacture", t.Numerofacture);
                editorfacture.PutInt("IDclient", t.Idclient);
                editorfacture.PutString("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                //editorfacture.PutString("Typeclient", t.TypeClinet);
                //editorfacture.PutInt("IDclient", t.Idclient);
                // editorfacture.PutInt("Item", c);

                editorfacture.PutInt("IDFactureinfo", InfoFact(t.Numerofacture)[0].Numerofacture);
                //editorfacture.PutString("Entreprise",Clientsinfos(sharedPreferences.GetString("Emailadmin", "")));
                editorfacture.PutString("DateFacturation", selectioninfo(t.Numerofacture)[0].Datefacturation);
                editorfacture.PutString("DateEcheance", selectioninfo(t.Numerofacture)[0].Dateechence);
                // editorfacture.PutString("Modepaiement",selectioninfo(t.Numerofacture)[0].Modepaiement);
                // editorfacture.PutString("observation", selectioninfo(t.Numerofacture)[0].Observation);

                editorfacture.Apply();
                StartActivity(intent);


            }
            catch (Exception ex)
            {

                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }
        }

    }

}