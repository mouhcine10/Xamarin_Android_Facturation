using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Facturation.Class;
using Mono.Data.Sqlite;

namespace Facturation
{
    [Activity(Label = "Home", Theme = "@style/AppTheme.NoActionBar")]
    public class Itemsapp : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        string ma = "";

        private List<Clients> facturenonpayer(string email)
        {
            List<Clients> clientsta = new List<Clients>();
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@email", email));
            contenu.CommandText = "SELECT  NomClinet, Emailclient ,Logo FROM ClientAdmin WHERE Emailclient=@email";



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




        //-------------------------------------------------------------------------
        public int selectionCountDevis()
        {
            int i = 0;

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

        private float Devistotal()
        {
            float t = 0;
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();

            contenu.CommandText = "SELECT sum((Prixht*Qte)+(((Prixht*Qte)*TVA)/100)-Remise),count(*) from Devis INNER JOIN commandeDevis on Devis.idDevis=commandeDevis.IdDevis";



            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                if (int.Parse(r[1].ToString()) == 0)
                {
                    t = 0;

                }
                else
                {
                    t = float.Parse(r[0].ToString());


                }

            }


            return t;



        }
        private float Devisvalide()
        {
            float t = 0;
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();

            contenu.CommandText = "SELECT sum((Prixht*Qte)+(((Prixht*Qte)*TVA)/100)-Remise),count(*) from Devis INNER JOIN commandeDevis on Devis.idDevis=commandeDevis.idDevis WHERE Devis.Etat='Validé'";



            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                if (int.Parse(r[1].ToString()) == 0)
                {
                    t = 0;

                }
                else
                {
                    t = float.Parse(r[0].ToString());


                }

            }


            return t;



        }
        private float DevisEncour()
        {
            float t = 0;
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();

            contenu.CommandText = "SELECT sum((Prixht*Qte)+(((Prixht*Qte)*TVA)/100)-Remise),count(*) from Devis INNER JOIN commandeDevis on Devis.idDevis=commandeDevis.idDevis WHERE Devis.Etat='Encours'";



            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                if (int.Parse(r[1].ToString()) == 0)
                {
                    t = 0;

                }
                else
                {
                    t = float.Parse(r[0].ToString());


                }

            }


            return t;



        }
        //private List<Clients> facturepayer(string email)
        //{
        //    List<Clients> clientsta = new List<Clients>();
        //    string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
        //    var connection = new SqliteConnection("Data Source=" + dbpath);
        //    connection.Open();
        //    var contenu = connection.CreateCommand();
        //    contenu.Parameters.Add(new SqliteParameter("@email", email));
        //    contenu.CommandText = "SELECT  NomClinet, Emailclient ,Logo FROM ClientAdmin WHERE Emailclient=@email";



        //    var r = contenu.ExecuteReader();

        //    while (r.Read())
        //    {

        //        if (r != null)
        //        {

        //            clientsta.Add(new Clients(r[0].ToString(), r[1].ToString(), (byte[])r[2]));
        //        }
        //    }


        //    return clientsta;



        //}
        private float totalfacture()
        {//SELECT ((Prixht*Qte)+(((Prixht*Qte)*TVA)/100)-Remise) from Facture INNER JOIN commandeFacture on Facture.idFacture=commandeFacture.idFacture GROUP by Facture.idFacture

            float t = 0;
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            //  contenu.Parameters.Add(new SqliteParameter("@email", email));
            contenu.CommandText = "SELECT sum((Prixht*Qte)+(((Prixht*Qte)*TVA)/100)-Remise),count(*) from Facture INNER JOIN commandeFacture on Facture.idFacture=commandeFacture.idFacture ";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                if (int.Parse(r[1].ToString()) == 0)
                {
                    t = 0;

                }
                else
                {
                    t = float.Parse(r[0].ToString());


                }
            }

            return t;
        }
        private float totalfacturepayer()
        {//SELECT ((Prixht*Qte)+(((Prixht*Qte)*TVA)/100)-Remise) from Facture INNER JOIN commandeFacture on Facture.idFacture=commandeFacture.idFacture GROUP by Facture.idFacture

            float t = 0;
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            //  contenu.Parameters.Add(new SqliteParameter("@email", email));
            contenu.CommandText = "SELECT sum((Prixht*Qte)+(((Prixht*Qte)*TVA)/100)-Remise),count(*) from Facture INNER JOIN commandeFacture on Facture.idFacture=commandeFacture.idFacture WHERE Facture.Etat='Payée'";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                if (int.Parse(r[1].ToString()) == 0)
                {
                    t = 0;

                }
                else
                {
                    t = float.Parse(r[0].ToString());


                }
            }

            return t;
        }
        private float totalfacturenonpayer()
        {//SELECT ((Prixht*Qte)+(((Prixht*Qte)*TVA)/100)-Remise) from Facture INNER JOIN commandeFacture on Facture.idFacture=commandeFacture.idFacture GROUP by Facture.idFacture

            float t = 0;
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            //  contenu.Parameters.Add(new SqliteParameter("@email", email));
            contenu.CommandText = "SELECT sum((Prixht*Qte)+(((Prixht*Qte)*TVA)/100)-Remise),count(*) from Facture INNER JOIN commandeFacture on Facture.idFacture=commandeFacture.idFacture WHERE Facture.Etat='Non Payée'";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {


                if (int.Parse(r[1].ToString()) == 0)
                {
                    t = 0;

                }
                else
                {
                    t = float.Parse(r[0].ToString());


                }
            }

            return t;
        }
        public int selectionCountFacture()
        {
            int i = 0;

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
        private List<Clients> selectionInfoEntreprise(string email)
        {
            List<Clients> clientsta = new List<Clients>();
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@email", email));
            contenu.CommandText = "SELECT  NomClinet, Emailclient ,Logo FROM ClientAdmin WHERE Emailclient=@email";



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


        public void CreationDatabase()
        {

            string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            bool Exists = File.Exists(dbpath);


            if (!Exists)
            {

                Mono.Data.Sqlite.SqliteConnection.CreateFile(dbpath);
                List<string> commands = new List<string>() { "Create Table Client( idClient INTEGER primary key, TypeClinet ntext,NomClinet ntext ,Telephone NUMERIC,Emailclient ntext,adresse ntext,CodePostal ntext, ville ntext, pays ntext,Logo BLOB ,Mdp text,confirmdp text,role text)",
                    "Create Table Banck(IDInfobanck INTEGER primary key AUTOINCREMENT, Nombanc ntext ,BIC ntext,IBAN ntext,idclient INTEGER  REFERENCES Client(idClient) on delete cascade)",

                    "Create Table Facture( idFacture INTEGER primary key, DateFacturation ntext, Datetranform ntext ,DateEchance ntext , DatePaiement ntext ,Devise ntext, idClient INTEGER  REFERENCES Client(IdClient) ,Etat ntext ,modepaiement ntext,observation ntext,signature BLOB)",
                    //"Create Table Imagefact(Idimage INTEGER primary key AUTOINCREMENT,Imagevvide BLOB, IdFacture INTEGER REFERENCES Facture(idFacture) on delete cascade)",

                    "Create Table commandeFacture(Idcomfact INTEGER primary key AUTOINCREMENT, IdFacture INTEGER REFERENCES Facture(idFacture) on delete cascade, Libille ntext,TypeProd ntext , PrixHT REAL,Qte INTEGER, TVA REAL,Remise REAL)",

                    "Create Table Devis(idDevis INTEGER primary key ,DateCreation ntext,DateValidation ntext ,Datetransform ntext , Devise ntext, idClient INTEGER  REFERENCES Client(IdClient),Etat ntext, modepaiement ntext ,observation ntext,signature BLOB)",

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

        LinearLayout facturenopaye;
        LinearLayout facturepaye;
        LinearLayout devisnonpaye;
        LinearLayout devispaye;
        AdView ads1, ads2;

        TextView textView6;

        Button save;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ma = Intent.GetStringExtra("Emailadmin");
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorDevis = sharedPreferences.Edit();
            //  CreationDatabase();
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Home);
            //
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

            SetSupportActionBar(toolbar);

            toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
            toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
            toolbar.Title = "Home";
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            Android.Support.V7.App.ActionBarDrawerToggle toggle = new Android.Support.V7.App.ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);

            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            var hederview = navigationView.GetHeaderView(0);

            //var imagelogo = hederview.FindViewById<ImageView>(Resource.Id.imageViewnavheaderlogo);

            //var EmailHeader = hederview.FindViewById<TextView>(Resource.Id.EmailHeder);

            //var NomEntreprise = hederview.FindViewById<TextView>(Resource.Id.NomHeader);
            ads2 = FindViewById<AdView>(Resource.Id.adView2Home);
            ads1 = FindViewById<AdView>(Resource.Id.adView1Home);


            var adRequest1 = new AdRequest.Builder().Build();
            ads1.LoadAd(adRequest1);
            var adRequest2 = new AdRequest.Builder().Build();
            ads2.LoadAd(adRequest2);

            var devisencour = FindViewById<LinearLayout>(Resource.Id.linearLayoutDevisencours);

            var devisvalider = FindViewById<LinearLayout>(Resource.Id.linearLayoutDevisvaliser);

            var facturenonpayer = FindViewById<LinearLayout>(Resource.Id.linearLayoutFacturenonpayer);

            var facturepayer = FindViewById<LinearLayout>(Resource.Id.linearLayoutFacturepayer);

            textView6 = FindViewById<TextView>(Resource.Id.textView6);
            textView6.Text = "Total des Factures Generer";

            devisencour.Click += delegate
            {
                string m = Intent.GetStringExtra("Emailadmin");
                Intent intent = new Intent(this, typeof(ListDevisEncours));

                intent.PutExtra("Emailadmin", m);
                StartActivity(intent);



            };

            devisvalider.Click += delegate
            {

                string m = Intent.GetStringExtra("Emailadmin");
                Intent intent = new Intent(this, typeof(ListDevisHome));
                intent.PutExtra("Emailadmin", m);
                StartActivity(intent);


            };

            facturenonpayer.Click += delegate
            {

                string m = Intent.GetStringExtra("Emailadmin");
                Intent intent = new Intent(this, typeof(Listfacturenonpayer));
                intent.PutExtra("Emailadmin", m);
                StartActivity(intent);


            };

            facturepayer.Click += delegate
            {

                string m = Intent.GetStringExtra("Emailadmin");
                Intent intent = new Intent(this, typeof(ListactureHome));
                intent.PutExtra("Emailadmin", m);
                StartActivity(intent);


            };

            //imagelogo.SetImageBitmap(Android.Graphics.BitmapFactory.DecodeByteArray(selectionInfoEntreprise(ma)[0].Logo, 0, selectionInfoEntreprise(ma)[0].Logo.Length));

            //EmailHeader.Text = selectionInfoEntreprise(ma)[0].Email;

            //NomEntreprise.Text = "Bonjour " + selectionInfoEntreprise(ma)[0].NomClinet;


            //facturenopaye = FindViewById<LinearLayout>(Resource.Id.linearLayoutfacturenonpaye);

            //facturepaye = FindViewById<LinearLayout>(Resource.Id.linearLayoutFacturePaye);

            //devispaye = FindViewById<LinearLayout>(Resource.Id.linearLayoutDevisPaye);

            //devisnonpaye = FindViewById<LinearLayout>(Resource.Id.linearLayoutDevisnonpaye);

            ////save = FindViewById<Button>(Resource.Id.buttonSauvgarde);

            //facturenopaye.Click += delegate {

            //    Intent intent = new Intent(this, typeof(ListfactureHome));

            //   editorDevis.PutString("Emailadmin",sharedPreferences.GetString("Emailadmin"," "));
            //    editorDevis.Apply();
            //    intent.PutExtra("Facture", "Non payé");

            //    StartActivity(intent);



            //};

            //facturepaye.Click += delegate {

            //    Intent intent = new Intent(this, typeof(ListfactureHome));

            //    editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", " "));
            //    editorDevis.Apply();
            //    intent.PutExtra("Facture", " Payé");

            //    StartActivity(intent);




            //};

            //devisnonpaye.Click += delegate {

            //    Intent intent = new Intent(this, typeof(ListDevisHome));


            //    intent.PutExtra("Devis", "Encours");
            //    editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", " "));
            //    editorDevis.Apply();
            //    StartActivity(intent);





            //};

            //devispaye.Click += delegate {

            //    Intent intent = new Intent(this, typeof(ListDevisHome));

            //    editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", " "));
            //    editorDevis.Apply();
            //    intent.PutExtra("Devis", "Validé");

            //    StartActivity(intent);



            //};
            var texttotalfacture = FindViewById<TextView>(Resource.Id.textViewTotalfacture);
            var textnonpayerfact = FindViewById<TextView>(Resource.Id.textViewtotalfacturenonpyer);
            var textviewpayer = FindViewById<TextView>(Resource.Id.textViewtotalfacturepayer);
            var texttotaldevis = FindViewById<TextView>(Resource.Id.textViewtotalDevis);
            var textDevisencours = FindViewById<TextView>(Resource.Id.textViewDevisEncours);
            var textdevivalide = FindViewById<TextView>(Resource.Id.textViewDevisValider);

            if (selectionCountDevis() > 0)
            {
                texttotaldevis.Text = Devistotal() + " " + Devise(Intent.GetStringExtra("Emailadmin"));
                textDevisencours.Text = DevisEncour() + " " + Devise(Intent.GetStringExtra("Emailadmin"));
                textdevivalide.Text = Devisvalide() + " " + Devise(Intent.GetStringExtra("Emailadmin"));



            }
            else
            {
                texttotaldevis.Text = "0" + " " + Devise(Intent.GetStringExtra("Emailadmin"));
                textDevisencours.Text = "0 " + " " + Devise(Intent.GetStringExtra("Emailadmin"));
                textdevivalide.Text = "0 " + " " + Devise(Intent.GetStringExtra("Emailadmin"));


            }


            if (selectionCountFacture() > 0)
            {
                texttotalfacture.Text = totalfacture().ToString() + " " + Devise(Intent.GetStringExtra("Emailadmin"));

                textnonpayerfact.Text = totalfacturenonpayer().ToString() + " " + Devise(Intent.GetStringExtra("Emailadmin"));

                textviewpayer.Text = totalfacturepayer().ToString() + " " + Devise(Intent.GetStringExtra("Emailadmin"));
            }
            else
            {


                texttotalfacture.Text = " 0 " + Devise(Intent.GetStringExtra("Emailadmin"));

                textnonpayerfact.Text = " 0 " + Devise(Intent.GetStringExtra("Emailadmin"));

                textviewpayer.Text = " 0 " + Devise(Intent.GetStringExtra("Emailadmin"));

            }
            //}
            //catch (Exception ex)
            //{
            //    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            //}

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
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorDevis = sharedPreferences.Edit();
            sharedPreferences.GetString("Emailadmin", "");
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
            sharedPreferences.GetString("Emailadmin", "");

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
                editorDevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorDevis.Apply();
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
                Etatconx();
                intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                StartActivity(intent);

            }
            else if (id == Resource.Id.nav_parametre)
            {
                Intent intent = new Intent(this, typeof(parametre));

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
    }

}