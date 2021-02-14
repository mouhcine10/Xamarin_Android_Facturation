using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Gms.Ads;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Hardware.Camera2;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Facturation.Class;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Java.Util;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using Document = iTextSharp.text.Document;
using Font = iTextSharp.text.Font;
using Uri = Android.Net.Uri;

namespace Facturation
{
    [Activity(Label = "Facture", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class Facturee : AppCompatActivity
    {
        Button genpdf;
        Bitmap bitmap;
        TextView textidFacture;
        TextView TextEntreptise;
        TextView textDateCreation;
        TextView datePaiement;
        LinearLayout infofacture;
        LinearLayout Client;
        TextView textclient;
        LinearLayout Ajouter;
        LinearLayout tablemontant;
        int PickImageId = 1000;
        string path = "";
        Uri uri = null;
        int idfacture;
        int numero;
        ListView itemfacture;
        TextView soustotaltext;
        TextView Remiseview;
        TextView taxeview;
        TextView TotalTTCview;
        byte[] dbbyte = null;
        EditText textobservation;
        TextView tet;
        Spinner spinnerModepaiement;
        int idclient = 0;
        public string toast;
        static List<Articleitem> articleitems = new List<Articleitem>();
        public string nonArticle;
        Java.IO.File dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/FolderName");
        string Email = "";
        AdView ads1, ads2;
        public float Qte;
        public float Prix;
        bool infofact = true;
        List<Clients> clients = new List<Clients>();
        readonly string[] permissionGroupe = { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
        const int Requestid = 0;
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (grantResults[0] == (int)Android.Content.PM.Permission.Granted)
            {
                Toast.MakeText(this, "Permission  was granted", ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(this, "Permission  was dinaid", ToastLength.Long).Show();
            }
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        bool CheckLocationPermission()
        {
            bool PermissionGranted = false;
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Android.Content.PM.Permission.Granted && ActivityCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != Android.Content.PM.Permission.Granted)
            {
                PermissionGranted = false;
                RequestPermissions(permissionGroupe, Requestid);
            }
            else
            {
                PermissionGranted = true;
            }
            return PermissionGranted;
        }
        bool CheckLocationPermissionwRITE()
        {
            bool PermissionGranted = false;
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == Android.Content.PM.Permission.Granted)
            {
                PermissionGranted = true;
            }
            else
            {
                PermissionGranted = false;
                RequestPermissions(permissionGroupe, Requestid);

            }
            return PermissionGranted;
        }

        private void DeleteListCommande(int IDFacture)
        {
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");

            var connection = new SqliteConnection("Data Source=" + dbpath);

            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idfacture", IDFacture));
            contenu.CommandText = "Delete from commandefacture where IDfacture=@idfacture";


            contenu.ExecuteNonQuery();

            connection.Close();

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
        private string Clientsinfos(string Email)
        {
            //clients.Clear();

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();

            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@Email", Email));

            contenu.CommandText = "Select NomClinet from ClientAdmin where Emailclient=@Email";

            string r = (string)contenu.ExecuteScalar();



            return r;

        }
        private List<Clients> Clientsinfosdev(string Email)
        {
            List<Clients> clients = new List<Clients>();

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();

            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@Email", Email));

            contenu.CommandText = "SELECT Logo,NomClinet,adresse, CodePostal, ville ,pays,Emailclient,Telephone from ClientAdmin WHERE Emailclient=@email";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                clients.Add(new Clients((byte[])r[0], r[1].ToString(), r[2].ToString(), r[3].ToString(), r[4].ToString(), r[5].ToString(), r[6].ToString(), r[7].ToString()));

            }


            return clients;

        }
        List<int> listid = new List<int>();

        private List<int> Selectioniditemfacture(int idfacture)
        {
            listid.Clear();


            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idFacture", idfacture));
            contenu.CommandText = "Select Idcomfact from commandeFacture where IdFacture=@idFacture";

            var d = contenu.ExecuteReader();

            while (d.Read())
            {
                listid.Add(int.Parse(d[0].ToString()));

            }
            //connection.Close();

            return listid;
        }
        private List<Articleitem> selectionItemCommendfacture(int idComfacture)
        {
            articleitems.Clear();
            string nom = "";
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idComfacture", idComfacture));
            contenu.CommandText = "Select Libille,Qte,PrixHT,TypeProd,Remise,TVA from commandeFacture where Idcomfact=@idComfacture";

            var r = contenu.ExecuteReader();



            while (r.Read())
            {

                articleitems.Add(new Articleitem(r[0].ToString(), float.Parse(r[1].ToString()), float.Parse(r[2].ToString()), r[3].ToString(), float.Parse(r[4].ToString()), float.Parse(r[5].ToString())));

            }
            connection.Close();

            return articleitems;



        }

        private int Checkid(int idFacture)
        {
            int i = 0;

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idfacture", idFacture));
            contenu.CommandText = "Select IdFacture from Facture where IdFacture=@idfacture";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {


                i = int.Parse(r[0].ToString());

            }

            connection.Close();

            return i;

        }
        private List<Articleitem> selectionCommendfacture(int idfacture)
        {
            articleitems.Clear();
            string nom = "";
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idfacture", idfacture));
            contenu.CommandText = "Select Idcomfact, Libille,Qte,PrixHT,TypeProd,Remise,TVA from commandeFacture where IdFacture=@idfacture";

            var r = contenu.ExecuteReader();


            while (r.Read())
            {

                articleitems.Add(new Articleitem(int.Parse(r[0].ToString()), r[1].ToString(), float.Parse(r[2].ToString()), float.Parse(r[3].ToString()), r[4].ToString(), float.Parse(r[5].ToString()), float.Parse(r[6].ToString())));

            }


            return articleitems;

            connection.Close();

        }
        private List<Clients> Clientsinfos(int idclient)
        {
            List<Clients> listclient = new List<Clients>();
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();

            contenu.Parameters.Add(new SqliteParameter("@IDclient", idclient));
            contenu.CommandText = "SELECT NomClinet,adresse, CodePostal, ville ,pays,Emailclient,Telephone from Client WHERE idClient =@IDclient";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {


                listclient.Add(new Clients(r[0].ToString(), r[1].ToString(), r[2].ToString(), r[3].ToString(), r[4].ToString(), r[5].ToString(), r[6].ToString()));

            }

            return listclient;

        }
        private float SelectionSoutotal(int idFacture)
        {
            float t = 0;
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idfacture", idFacture));
            contenu.CommandText = "Select Sum(PrixHT*Qte),count(*) from commandeFacture where IdFacture=@idfacture";

            var r = contenu.ExecuteReader();

            if (float.Parse(r[1].ToString()) == 0)
            {
                t = 0;
            }

            else
            {

                while (r.Read())
                {
                    t = float.Parse(r[0].ToString());
                }

            }
            connection.Close();
            return t;

        }
        private float SelectionRemise(int idFacture)
        {
            float t = 0;
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);

            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idfacture", idFacture));
            contenu.CommandText = "Select Sum(Remise),count(*) from commandeFacture where IdFacture=@idfacture";


            var r = contenu.ExecuteReader();



            if (float.Parse(r[1].ToString()) == 0)
            {

                t = 0;

            }
            else
            {
                while (r.Read())
                {
                    t = float.Parse(r[0].ToString());
                }

            }
            connection.Close();
            return t;

        }
        float t = 0;
        private float SelectionCalculTVA(int idFacture)
        {

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idfacture", idFacture));
            contenu.CommandText = "Select Sum(((PrixHT*Qte)*TVA)/100),count(*) from commandeFacture where IdFacture=@idfacture";


            var r = contenu.ExecuteReader();



            if (float.Parse(r[1].ToString()) == 0)
            {

                t = 0;

            }
            else
            {
                while (r.Read())
                {
                    t = float.Parse(r[0].ToString());
                }

            }
            connection.Close();
            return t;

        }

        //idFacture INTEGER primary key AUTOINCREMENT, DateFacturation Date,Datetranform date ,Devise ntext, idClient INTEGER  REFERENCES Client(IdClient) ,Etat ntext ,modepaiement ntext,observation ntext
        private void InsertionFacture(string DateFacturation, string Dateechance, string devis, int idclient, string Etat, string modepaiement, string observation, byte[] signature)
        {

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@Byte", signature));
            contenu.CommandText = "Insert into Facture(DateFacturation,DateEchance,Devise,idClient,Etat,modepaiement,observation,signature) values('" + DateFacturation + "','" + Dateechance + "','" + devis + "'," + idclient + ",'" + Etat + "','" + modepaiement + "','" + observation + "',@Byte)";

            contenu.ExecuteNonQuery();

            connection.Close();

        }

        public int selectionCountFacture(int idFacture)
        {
            int i = 0;
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();


            contenu.Parameters.Add(new SqliteParameter("@idfacture", idFacture));
            contenu.CommandText = "Select idFacture from  Facture where idFacture=@idfacture";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {
                i = int.Parse(r[0].ToString());


            }

            connection.Close();
            return i;

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
                idclient = sharedPreferences.GetInt("IDclient", 0);
                idfacture = sharedPreferences.GetInt("IDfacture", 0);


                base.OnCreate(savedInstanceState);

                //  Toast.MakeText(this, dir.AbsolutePath, ToastLength.Long).Show();
                SetContentView(Resource.Layout.Fact);

                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                // Back Home

                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                SupportActionBar.Title = "Facture";


                //tet = FindViewById<TextView>(Resource.Id.tet);
                //tet.Text = path;
                textidFacture = FindViewById<TextView>(Resource.Id.textViewRefefacture);
                TextEntreptise = FindViewById<TextView>(Resource.Id.textViewNomEntreprise);
                textDateCreation = FindViewById<TextView>(Resource.Id.textViewDatecreation);
                textDateCreation.Text = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                datePaiement = FindViewById<TextView>(Resource.Id.textViewDatepaiement);
                infofacture = FindViewById<LinearLayout>(Resource.Id.LinearLayoutFactureDetail);
                Client = FindViewById<LinearLayout>(Resource.Id.linearLayoutCLient);
                textclient = FindViewById<TextView>(Resource.Id.textViewCLient);
                Ajouter = FindViewById<LinearLayout>(Resource.Id.linearLayoutArticle);
                tablemontant = FindViewById<LinearLayout>(Resource.Id.linearLayout14);
                tablemontant.Visibility = Android.Views.ViewStates.Invisible;
                itemfacture = FindViewById<ListView>(Resource.Id.listViewArticle);
                soustotaltext = FindViewById<TextView>(Resource.Id.textViewMttsoutotal);
                Remiseview = FindViewById<TextView>(Resource.Id.textViewMontantRemise);
                taxeview = FindViewById<TextView>(Resource.Id.textViewMontantTaxe);
                TotalTTCview = FindViewById<TextView>(Resource.Id.textViewMontanttotalttc);
                textobservation = FindViewById<EditText>(Resource.Id.editTextOb);
                spinnerModepaiement = FindViewById<Spinner>(Resource.Id.spinnerModepaiement);

                ads1 = FindViewById<AdView>(Resource.Id.adView1Facture);
                ads2 = FindViewById<AdView>(Resource.Id.adView2Facture);
                var adRequest1 = new AdRequest.Builder().Build();
                ads1.LoadAd(adRequest1);
                var adRequest2 = new AdRequest.Builder().Build();
                ads2.LoadAd(adRequest2);


                //  genpdf = FindViewById<Button>(Resource.Id.buttonGenererPDFFActure);
                var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.ModePaimenet, Android.Resource.Layout.SimpleSpinnerItem);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spinnerModepaiement.Adapter = adapter;
                spinnerModepaiement.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);


                // spinnerModepaiement.SetSelection("");

                // var btnpiece = FindViewById<Button>(Resource.Id.buttonpiece);
                var btnsegnature = FindViewById<Button>(Resource.Id.buttonSignatureFacte);

                // Button suprimerSignatur = FindViewById<Button>(Resource.Id.buttonsupsigna);



                textidFacture.Text = "N°Facture:\n" + sharedPreferences.GetInt("IDfacture", 0);
                TextEntreptise.Text = "Stociété:\n" + Clientsinfos(sharedPreferences.GetString("Emailadmin", ""));
                textDateCreation.Text = sharedPreferences.GetString("DateCreation", DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year);
                datePaiement.Text = sharedPreferences.GetString("DateEcheance", DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year);

                infofacture.Click += delegate
                {
                    Intent intent = new Intent(this, typeof(FactrueDetail));
                    intent.PutExtra("info", infofact);
                    editorfacture.PutInt("IDfacture", sharedPreferences.GetInt("IDfacture", 0));
                    editorfacture.PutString("Entreprise", Clientsinfos(sharedPreferences.GetString("Emailadmin", "")));
                    editorfacture.PutString("DateCreation", textDateCreation.Text);
                    editorfacture.PutString("DateEcheance", datePaiement.Text);
                    editorfacture.PutString("ob", textobservation.Text);
                    editorfacture.Apply();
                    intent.PutExtra("ImagePath", dbbyte);

                    StartActivity(intent);





                };




                Client.Click += delegate
                {


                    Intent intentClient = new Intent(this, typeof(ListCLients));
                    editorfacture.PutInt("IDclienet", idclient);
                    editorfacture.PutString("ob", textobservation.Text);
                    editorfacture.Apply();
                    intentClient.PutExtra("ImagePath", dbbyte);


                    StartActivity(intentClient);



                };

                textclient.Text = sharedPreferences.GetString("titre", "choisir votre client");




                Ajouter.Click += delegate
                {

                    Intent intent = new Intent(this, typeof(DetailFacture));
                    intent.PutExtra("ImagePath", dbbyte);
                    editorfacture.PutString("ob", textobservation.Text);
                    editorfacture.PutInt("IDfacture", sharedPreferences.GetInt("IDfacture", 0));
                    editorfacture.PutString("NomArticle", " ");
                    editorfacture.PutFloat("Qte", 0);
                    editorfacture.PutFloat("Prix", 0);
                    editorfacture.PutString("Tyoeremose", " ");
                    editorfacture.PutFloat("Tauxremise", 0);
                    editorfacture.PutFloat("TVA", 0);
                    editorfacture.PutString("Emailadmin", "");
                    editorfacture.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                    editorfacture.Apply();
                    intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                    StartActivity(intent);
                   
                    intent.PutExtra("ImagePath", dbbyte);
                    StartActivity(intent);



                };

                //nonArticle = sharedPreferences.GetString("Article", "");
                //Qte = sharedPreferences.GetFloat("Qte", 0);
                //Prix = sharedPreferences.GetFloat("Prix", 0);
                //Articleitem articleitem = new Articleitem(nonArticle,Qte,Prix);
                //ADDItems(articleitem);

                //selectionCommendfacture(idfacture);


                itemfacture.Adapter = new HomeScreenAdapterArticleFact(this, selectionCommendfacture(sharedPreferences.GetInt("IDfacture", 0)));

                itemfacture.ItemClick += OnListItemClick;

                tablemontant.Visibility = Android.Views.ViewStates.Visible;




                soustotaltext.Text = SelectionSoutotal(sharedPreferences.GetInt("IDfacture", 0)).ToString();

                taxeview.Text = (SelectionCalculTVA(sharedPreferences.GetInt("IDfacture", 0))).ToString();

                float c = SelectionSoutotal(sharedPreferences.GetInt("IDfacture", 0)) + SelectionCalculTVA(idfacture);

                Remiseview.Text = SelectionRemise(sharedPreferences.GetInt("IDfacture", 0)).ToString();

                float d = c - float.Parse(Remiseview.Text);
                TotalTTCview.Text = d.ToString();
                // Create your application here
                //btnpiece.Click += delegate
                //{
                //    Intent = new Intent();
                //    Intent.SetType("image/*");
                //    Intent.SetAction(Intent.ActionGetContent);
                //    StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
                //};
                //var image = FindViewById<ImageView>(Resource.Id.imageViewsrk);

                byte[] inten = Intent.GetByteArrayExtra("ImagePath");




                if (inten == null)
                {
                    btnsegnature.Click += delegate
                    {


                        Intent intent = new Intent(this, typeof(LayoutSegnature));

                        intent.PutExtra("IDfacture", sharedPreferences.GetInt("IDfacture", 0));

                        StartActivity(intent);


                    };
                }
                else
                {

                    bitmap = BitmapFactory.DecodeByteArray(inten, 0, inten.Length);


                    // image.SetImageBitmap(bitmap);


                }

                dbbyte = inten;


                //genpdf.Enabled = false;

                //genpdf.Click += delegate
                // {
                //     string a = "";


                //     if (CheckLocationPermission()) 
                //     {
                //         CreatefaileFolder("jeloo.pdf", this);
                //     }

                //     Intent intent = new Intent(this, typeof(PDFlayout));

                //     intent.PutExtra("a", Getfile(dir.AbsolutePath)[1]);

                //     StartActivity(intent);
                // };



            }
            catch (Exception ex)
            {

                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }


        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.menuFacture, menu);
            // menu.GetItem(2).SetEnabled(false);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();

            int id = item.ItemId;

            if (id == Android.Resource.Id.Home)
            {

                if (selectionCountFacture(sharedPreferences.GetInt("IDfacture", 0)) == idfacture)
                {
                    editorfacture.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                    editorfacture.Apply();
                    string ma = Intent.GetStringExtra("Emailadmin");
                    Intent intent = new Intent(this, typeof(Listacture));
                    intent.PutExtra("Emailadmin", ma);
                    StartActivity(intent);

                }
                else
                {
                    editorfacture.PutString("Emailadmin", "");
                    editorfacture.Apply();
                    DeleteListCommande(sharedPreferences.GetInt("IDfacture", 0));
                    textclient.Text = "choisir votre client";
                    Intent intent = new Intent(this, typeof(Listacture));
                    intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                    StartActivity(intent);
                }

            }

            if (id == Resource.Id.Enregistrer)
            {
                Intent intent = new Intent(this, typeof(Listacture));
                if (dbbyte == null)
                {
                    Toast.MakeText(this, "vérifier votre signature", ToastLength.Long).Show();

                }
                else
                {
                    string ma = sharedPreferences.GetString("Emailadmin", "");
                    InsertionFacture(textDateCreation.Text, datePaiement.Text, Devise(sharedPreferences.GetString("Emailadmin", "")), idclient, "Non Payée", toast, textobservation.Text, dbbyte);
                    // Intent intent = new Intent(this, typeof(PDFlayout));
                    editorfacture.PutString("Emailadmin", "");
                    editorfacture.Apply();

                   
                    intent.PutExtra("Emailadmin", ma);

                    //  CreatefaileFolder("Facture N°" + sharedPreferences.GetInt("IDfacture", 0));

                    StartActivity(intent);




                    //StartActivity(intent);
                }
            }
            //if (id == Resource.Id.Preview)
            //{
            //    //if (Checkid(idfacture).Equals(idfacture))
            //    //{

            //    Intent intent = new Intent(this, typeof(PDFlayout));

            //    if (CheckLocationPermissionwRITE())
            //    {
            //        CreatefaileFolder("Facture1.pdf");



            //        intent.PutExtra("a", Getfile(dir.AbsolutePath, "Facture1.pdf")[0].ToString());

            //        StartActivity(intent);
            //    }




            //}
            //else
            //{
            //    Toast.MakeText(this, "add your inivuc and try", ToastLength.Long).Show();


            //}





            return base.OnOptionsItemSelected(item);
        }
        public override void OnBackPressed()
        {
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();
            if (selectionCountFacture(sharedPreferences.GetInt("IDfacture", 0)) == idfacture)
            {
                Intent intent = new Intent(this, typeof(Listacture));
                //   intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                StartActivity(intent);

            }
            else
            {
                DeleteListCommande(sharedPreferences.GetInt("IDfacture", 0));
                textclient.Text = "choisir votre client";
                Intent intent = new Intent(this, typeof(Listacture));
                // intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                StartActivity(intent);
            }
            base.OnBackPressed();

        }

        List<string> Getfile(string path, string filepdf)
        {
            List<string> file = new List<string>();

            if (Directory.Exists(path))
            {
                var pdfFiles = Directory.EnumerateFiles(path, filepdf, SearchOption.AllDirectories);

                foreach (string currentFile in pdfFiles)
                {
                    file.Add(currentFile);
                }
            }
            return file;
        }


        void CreatefaileFolder(string filename)
        {
            //  Java.IO.File dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/FolderName");
            //string path = System.IO.Path.Combine(dir.AbsolutePath, filename);
            //bool Existe = System.IO.File.Exists(path);

            var pathcombine = System.IO.Path.Combine(dir.AbsolutePath, filename);
            bool Exists = File.Exists(pathcombine);
            if (!Exists)
            {
                File.Delete(pathcombine);
                dir.Mkdir();
            }

            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();
            Document document = new Document();
            string t = dir.AbsolutePath + "/" + filename;
            if (CheckLocationPermissionwRITE())
            {
                if (CheckLocationPermission())
                {

                    PdfWriter pdfWriter = PdfWriter.GetInstance(document, new FileStream(t, FileMode.Create));
                    //open document 
                    document.Open();
                    pdfWriter.Open();
                    document.SetPageSize(PageSize.A4);
                    document.AddCreationDate();
                    document.AddAuthor("hello");
                    Android.Graphics.Color color = new Android.Graphics.Color(0, 153, 204, 255);
                    Font text = new Font(Font.NORMAL, Android.Graphics.Color.Black);

                    PdfPTable Headerfactrue = new PdfPTable(3);
                    Headerfactrue.DefaultCell.Border = Rectangle.NO_BORDER;

                    //CellLogo 
                    string codelogo = Intent.GetStringExtra("Emailadmin");
                    iTextSharp.text.Image pica = iTextSharp.text.Image.GetInstance(Clientsinfosdev(codelogo)[0].Logo);
                    pica.ScaleToFit(50, 50);
                    PdfPCell pdfPCellLogo = new PdfPCell();
                    pdfPCellLogo.Border = Rectangle.NO_BORDER;
                    pdfPCellLogo.AddElement(pica);
                    //cellvide

                    PdfPCell pdfPCellmillieur = new PdfPCell();
                    pdfPCellmillieur.Border = Rectangle.NO_BORDER;
                    pdfPCellmillieur.AddElement(new Paragraph("  "));

                    // Cell infofacture
                    PdfPTable pdfPTableinfo = new PdfPTable(1);

                    pdfPTableinfo.DefaultCell.Border = Rectangle.NO_BORDER;
                    pdfPTableinfo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    pdfPTableinfo.AddCell("Facture N°:" + sharedPreferences.GetInt("IDfacture", 0));
                    pdfPTableinfo.WidthPercentage = 100F;
                    pdfPTableinfo.AddCell("Facturation:" + textDateCreation.Text);
                    pdfPTableinfo.AddCell("Echence:" + datePaiement.Text);

                    PdfPCell pdfPCellInfoFact = new PdfPCell();
                    pdfPCellInfoFact.Border = Rectangle.NO_BORDER;
                    pdfPCellInfoFact.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    pdfPCellInfoFact.AddElement(pdfPTableinfo);

                    Headerfactrue.AddCell(pdfPCellLogo);
                    Headerfactrue.AddCell(pdfPCellmillieur);
                    Headerfactrue.AddCell(pdfPTableinfo);


                    iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.BOLD);
                    //info Entreprise Et client 

                    PdfPTable cliententreprise = new PdfPTable(3);

                    cliententreprise.DefaultCell.Border = Rectangle.NO_BORDER;
                    //cellEntreprise 

                    cliententreprise.SetWidths(new float[] { 100f, 50f, 100f });
                    PdfPCell cellentreprise = new PdfPCell();

                    cellentreprise.Border = Rectangle.NO_BORDER;
                    PdfPTable Tinfoentreprise = new PdfPTable(1);
                    Tinfoentreprise.WidthPercentage = 100;
                    Tinfoentreprise.DefaultCell.Border = Rectangle.NO_BORDER;
                    Tinfoentreprise.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;

                    //Tinfoentreprise.AddCell("Entreprise:");
                    //PdfPCell nomEntreprise = new PdfPCell();
                    //nomEntreprise.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    //nomEntreprise.Border = Rectangle.NO_BORDER;
                    //nomEntreprise.AddElement(new Paragraph("darna"));
                    //Tinfoentreprise.AddCell(nomEntreprise);
                    ////
                    //PdfPCell EmailEntreprise = new PdfPCell();
                    //EmailEntreprise.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    //EmailEntreprise.Border = Rectangle.NO_BORDER;
                    //EmailEntreprise.AddElement(new Paragraph("Mouhcine558@mail.com"));
                    //Tinfoentreprise.AddCell(EmailEntreprise);
                    ////
                    //PdfPCell NumeroEntreprise = new PdfPCell();
                    //NumeroEntreprise.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    //NumeroEntreprise.Border = Rectangle.NO_BORDER;
                    //NumeroEntreprise.AddElement(new Paragraph("07554545454"));
                    //Tinfoentreprise.AddCell(NumeroEntreprise);

                    ////
                    //PdfPCell AdresseEntreprise = new PdfPCell();
                    //AdresseEntreprise.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    //AdresseEntreprise.Border = Rectangle.NO_BORDER;
                    //AdresseEntreprise.AddElement(new Paragraph("235,rue 9atolahhd"));
                    //Tinfoentreprise.AddCell(AdresseEntreprise);
                    ////
                    //PdfPCell CodePostalEntreprise = new PdfPCell();
                    //CodePostalEntreprise.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    //CodePostalEntreprise.Border = Rectangle.NO_BORDER;
                    //CodePostalEntreprise.AddElement(new Paragraph("75054"));
                    //Tinfoentreprise.AddCell(CodePostalEntreprise);

                    ////
                    //PdfPCell VilleEntreprise = new PdfPCell();
                    //VilleEntreprise.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    //VilleEntreprise.Border = Rectangle.NO_BORDER;
                    //VilleEntreprise.AddElement(new Paragraph("casablanca"));
                    //Tinfoentreprise.AddCell(VilleEntreprise);

                    ////
                    //PdfPCell paysEntreprise = new PdfPCell();
                    //paysEntreprise.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    //paysEntreprise.Border = Rectangle.NO_BORDER;
                    //paysEntreprise.AddElement(new Paragraph("Maroc"));
                    //Tinfoentreprise.AddCell(paysEntreprise);

                    Tinfoentreprise.AddCell("Facture De:");
                    Tinfoentreprise.AddCell(Clientsinfosdev(Intent.GetStringExtra("Emailadmin"))[0].NomClinet);
                    Tinfoentreprise.AddCell(Clientsinfosdev(Intent.GetStringExtra("Emailadmin"))[0].Adresse);
                    Tinfoentreprise.AddCell(Clientsinfosdev(Intent.GetStringExtra("Emailadmin"))[0].Codepostal);
                    Tinfoentreprise.AddCell(Clientsinfosdev(Intent.GetStringExtra("Emailadmin"))[0].Ville);
                    Tinfoentreprise.AddCell(Clientsinfosdev(Intent.GetStringExtra("Emailadmin"))[0].Pays);
                    Tinfoentreprise.AddCell(Clientsinfosdev(Intent.GetStringExtra("Emailadmin"))[0].Email);
                    Tinfoentreprise.AddCell(Clientsinfosdev(Intent.GetStringExtra("Emailadmin"))[0].Telephone);



                    cellentreprise.AddElement(Tinfoentreprise);


                    cliententreprise.AddCell(cellentreprise);
                    ////cellvide
                    PdfPCell pdfPCellvideinfo = new PdfPCell();
                    pdfPCellvideinfo.Border = Rectangle.NO_BORDER;

                    //PdfPCell pdfPCellvideinfo1 = new PdfPCell();
                    //pdfPCellvideinfo1.Border = Rectangle.NO_BORDER;


                    cliententreprise.AddCell(pdfPCellvideinfo);
                    //   cliententreprise.AddCell(pdfPCellvideinfo1);

                    //cellinfoclient
                    PdfPCell cellClinet = new PdfPCell();
                    cellClinet.Border = Rectangle.NO_BORDER;
                    PdfPTable Tinfoclient = new PdfPTable(1);
                    Tinfoclient.WidthPercentage = 100;
                    Tinfoclient.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    Tinfoclient.DefaultCell.Border = Rectangle.NO_BORDER;

                    Tinfoclient.AddCell("Facture à:");
                    Tinfoclient.AddCell(Clientsinfos(idclient)[0].NomClinet);
                    Tinfoclient.AddCell(Clientsinfos(idclient)[0].Adresse);
                    Tinfoclient.AddCell(Clientsinfos(idclient)[0].Codepostal);
                    Tinfoclient.AddCell(Clientsinfos(idclient)[0].Ville);
                    Tinfoclient.AddCell(Clientsinfos(idclient)[0].Pays);
                    Tinfoclient.AddCell(Clientsinfos(idclient)[0].Email);
                    Tinfoclient.AddCell(Clientsinfos(idclient)[0].Telephone);
                    //Tinfoclient.AddCell("Client:");
                    //PdfPCell nomclient = new PdfPCell();
                    //nomclient.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    //nomclient.Border = Rectangle.NO_BORDER;
                    //nomclient.AddElement(new Paragraph("Nom:darna"));
                    //Tinfoclient.AddCell(nomclient);
                    ////
                    //PdfPCell Emailclient = new PdfPCell();
                    //Emailclient.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    //Emailclient.Border = Rectangle.NO_BORDER;
                    //Emailclient.AddElement(new Paragraph("Mouhcine558@mail.com"));
                    //Tinfoclient.AddCell(Emailclient);
                    ////
                    //PdfPCell Numeroclient = new PdfPCell();
                    //Numeroclient.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    //Numeroclient.Border = Rectangle.NO_BORDER;
                    //Numeroclient.AddElement(new Paragraph("07554545454"));
                    //Tinfoclient.AddCell(Numeroclient);

                    ////
                    //PdfPCell Adresseclient = new PdfPCell();
                    //Adresseclient.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    //Adresseclient.Border = Rectangle.NO_BORDER;
                    //Adresseclient.AddElement(new Paragraph("235,rue 9atolahhd"));
                    //Tinfoclient.AddCell(Adresseclient);
                    ////
                    //PdfPCell CodePostallient = new PdfPCell();
                    //CodePostallient.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    //CodePostallient.Border = Rectangle.NO_BORDER;
                    //CodePostallient.AddElement(new Paragraph("75054"));
                    //Tinfoclient.AddCell(CodePostallient);

                    ////
                    //PdfPCell Villeclient = new PdfPCell();
                    //Villeclient.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    //Villeclient.Border = Rectangle.NO_BORDER;
                    //Villeclient.AddElement(new Paragraph("casablanca"));
                    //Tinfoclient.AddCell(Villeclient);

                    ////
                    //PdfPCell paysclient = new PdfPCell();
                    //paysclient.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    //paysclient.Border = Rectangle.NO_BORDER;
                    //paysclient.AddElement(new Paragraph("Maroc"));
                    //Tinfoclient.AddCell(paysclient);




                    cliententreprise.AddCell(Tinfoclient);

                    //Tableau items Produits

                    PdfPTable itemsfactur = new PdfPTable(6);
                    itemsfactur.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    itemsfactur.DefaultCell.Border = Rectangle.NO_BORDER;
                    itemsfactur.HorizontalAlignment = 0;
                    itemsfactur.WidthPercentage = 100;
                    itemsfactur.DefaultCell.BorderWidthBottom = 0f;
                    // itemsfactur.SetWidths(new float[] { 5, 40, 10, 20, 25 });
                    itemsfactur.SpacingAfter = 40;
                    PdfPCell type = new PdfPCell(new Paragraph("Type"));
                    type.Border = Rectangle.NO_BORDER;
                    type.BackgroundColor = iTextSharp.text.Color.GREEN;
                    type.BorderWidthBottom = 1f;
                    PdfPCell Designation = new PdfPCell(new Paragraph("Désignation"));
                    Designation.Border = Rectangle.NO_BORDER;
                    Designation.BackgroundColor = iTextSharp.text.Color.GREEN;
                    Designation.BorderWidthBottom = 1f;
                    PdfPCell Qte = new PdfPCell(new Paragraph("Qté"));
                    Qte.Border = Rectangle.NO_BORDER;
                    Qte.BackgroundColor = iTextSharp.text.Color.GREEN;
                    Qte.BorderWidthBottom = 1f;
                    PdfPCell Prix = new PdfPCell(new Paragraph("Prix"));
                    Prix.Border = Rectangle.NO_BORDER;
                    Prix.BackgroundColor = iTextSharp.text.Color.GREEN;
                    Prix.BorderWidthBottom = 1f;
                    PdfPCell Tva = new PdfPCell(new Paragraph("TVA"));
                    Tva.Border = Rectangle.NO_BORDER;
                    Tva.BackgroundColor = iTextSharp.text.Color.GREEN;
                    Tva.BorderWidthBottom = 1f;
                    PdfPCell Remise = new PdfPCell(new Paragraph("Remise"));
                    Remise.Border = Rectangle.NO_BORDER;
                    Remise.BackgroundColor = iTextSharp.text.Color.GREEN;
                    Remise.BorderWidthBottom = 1f;
                    itemsfactur.AddCell(type);
                    itemsfactur.AddCell(Designation);
                    itemsfactur.AddCell(Qte);
                    itemsfactur.AddCell(Prix);
                    itemsfactur.AddCell(Tva);
                    itemsfactur.AddCell(Remise);

                    PdfPTable itemsfacturBody = new PdfPTable(6);
                    itemsfacturBody.WidthPercentage = 100;

                    itemsfacturBody.HorizontalAlignment = 0;
                    itemsfacturBody.DefaultCell.Border = Rectangle.NO_BORDER;
                    itemsfacturBody.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    for (int i = 0; i < selectionItemCommendfacture(2).Count; i++)
                    {

                        itemsfactur.AddCell(selectionItemCommendfacture(sharedPreferences.GetInt("IDfacture", 0))[i].Ttyperemise);
                        itemsfactur.AddCell(selectionItemCommendfacture(sharedPreferences.GetInt("IDfacture", 0))[i].NomArticle);
                        itemsfactur.AddCell(selectionItemCommendfacture(sharedPreferences.GetInt("IDfacture", 0))[i].Qte.ToString());
                        itemsfactur.AddCell(selectionItemCommendfacture(sharedPreferences.GetInt("IDfacture", 0))[i].Prix.ToString());
                        itemsfactur.AddCell(selectionItemCommendfacture(sharedPreferences.GetInt("IDfacture", 0))[i].Tva.ToString());
                        itemsfactur.AddCell(selectionItemCommendfacture(sharedPreferences.GetInt("IDfacture", 0))[i].Tauxremise.ToString());




                    }

                    //Detao


                    PdfPTable pdftabtotal = new PdfPTable(3);
                    pdftabtotal.SetWidths(new float[] { 50, 50, 100 });
                    pdftabtotal.DefaultCell.Border = Rectangle.NO_BORDER;
                    PdfPCell pdfCell1 = new PdfPCell(new Paragraph(" "));
                    pdfCell1.Border = Rectangle.NO_BORDER;
                    PdfPCell pdfCell2 = new PdfPCell(new Paragraph(" "));
                    pdfCell2.Border = Rectangle.NO_BORDER;
                    PdfPTable calcul = new PdfPTable(3);
                    calcul.DefaultCell.Border = Rectangle.NO_BORDER;
                    calcul.DefaultCell.BorderWidthBottom = 0f;

                    calcul.SetWidths(new float[] { 50, 60, 10 });

                    PdfPCell soutotalText = new PdfPCell(new Paragraph("Sous-total:"));
                    soutotalText.Border = Rectangle.NO_BORDER;
                    PdfPCell soutotalSoume = new PdfPCell(new Paragraph(SelectionSoutotal(2).ToString()));
                    soutotalSoume.Border = Rectangle.NO_BORDER;
                    PdfPCell soutotalDevis = new PdfPCell(new Paragraph("EUR"));
                    soutotalDevis.Border = Rectangle.NO_BORDER;
                    PdfPCell TVAText = new PdfPCell(new Paragraph("TVA:"));
                    TVAText.Border = Rectangle.NO_BORDER;
                    PdfPCell TVASoume = new PdfPCell(new Paragraph(SelectionSoutotal(2).ToString()));
                    TVASoume.Border = Rectangle.NO_BORDER;
                    PdfPCell TVADevis = new PdfPCell(new Paragraph("EUR"));
                    TVADevis.Border = Rectangle.NO_BORDER;
                    float c = (SelectionSoutotal(2) + SelectionCalculTVA(2)) - SelectionRemise(2);
                    PdfPCell totalText = new PdfPCell(new Paragraph("Total TTC:"));
                    totalText.Border = Rectangle.NO_BORDER;
                    totalText.BorderWidthBottom = 1f;
                    totalText.BorderWidthTop = 1f;
                    PdfPCell totalSoume = new PdfPCell(new Paragraph(c.ToString()));
                    totalSoume.Border = Rectangle.NO_BORDER;
                    totalSoume.BorderWidthBottom = 1f;
                    totalSoume.BorderWidthTop = 1f;
                    PdfPCell totalDevis = new PdfPCell(new Paragraph("EUR"));
                    totalDevis.Border = Rectangle.NO_BORDER;
                    totalDevis.BorderWidthBottom = 1f;
                    totalDevis.BorderWidthTop = 1f;
                    PdfPCell RemiseText = new PdfPCell(new Paragraph("Remise:"));
                    RemiseText.Border = Rectangle.NO_BORDER;
                    PdfPCell RemiseSoume = new PdfPCell(new Paragraph(SelectionSoutotal(2).ToString()));
                    RemiseSoume.Border = Rectangle.NO_BORDER;
                    PdfPCell RemiseDevis = new PdfPCell(new Paragraph("EUR"));
                    RemiseDevis.Border = Rectangle.NO_BORDER;
                    calcul.AddCell(soutotalText);
                    calcul.AddCell(soutotalSoume);
                    calcul.AddCell(soutotalDevis);
                    calcul.AddCell(TVAText);
                    calcul.AddCell(TVASoume);
                    calcul.AddCell(TVADevis);
                    calcul.AddCell(RemiseText);
                    calcul.AddCell(RemiseSoume);
                    calcul.AddCell(RemiseDevis);
                    calcul.AddCell(totalText);
                    calcul.AddCell(totalSoume);
                    calcul.AddCell(totalDevis);

                    PdfPCell pdfCell3 = new PdfPCell(calcul);
                    pdfCell3.BorderWidthTop = 1f;
                    pdfCell3.Border = Rectangle.NO_BORDER;
                    pdfCell3.BorderWidthBottom = 1f;

                    pdftabtotal.AddCell(pdfCell1);
                    pdftabtotal.AddCell(pdfCell2);
                    pdftabtotal.AddCell(pdfCell3);

                    //Mode Paiement Donné bancaier

                    PdfPTable ModepaiementdonnerBancaire = new PdfPTable(3);
                    ModepaiementdonnerBancaire.DefaultCell.Border = Rectangle.NO_BORDER;
                    ModepaiementdonnerBancaire.SetWidths(new float[] { 120, 20, 200 });

                    PdfPTable pdfPTablemode = new PdfPTable(1);
                    pdfPTablemode.DefaultCell.Border = Rectangle.NO_BORDER;
                    pdfPTablemode.AddCell("Mode Paiement:" + toast);
                    PdfPCell pdfPCell11 = new PdfPCell(pdfPTablemode);
                    pdfPCell11.Border = Rectangle.NO_BORDER;
                    pdfPCell11.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    PdfPCell pdfPCell22 = new PdfPCell();
                    pdfPCell22.Border = Rectangle.NO_BORDER;

                    PdfPCell pdfPCell33 = new PdfPCell();
                    pdfPCell33.Border = Rectangle.NO_BORDER;

                    PdfPTable banque = new PdfPTable(2);
                    banque.WidthPercentage = 100;
                    banque.DefaultCell.Border = Rectangle.NO_BORDER;
                    banque.SetWidths(new float[] { 90, 190 });
                    banque.AddCell(new Paragraph("Banque:"));
                    banque.AddCell(new Paragraph("BNPParibas"));
                    banque.AddCell(new Paragraph("BIC"));
                    banque.AddCell(new Paragraph("qsdqsfdf"));
                    banque.AddCell(new Paragraph("IBAN"));
                    banque.AddCell(new Paragraph("FR54545454dgdfgfdgfdfdg"));

                    pdfPCell33.AddElement(banque);


                    ModepaiementdonnerBancaire.AddCell(pdfPCell11);
                    ModepaiementdonnerBancaire.AddCell(pdfPCell22);
                    ModepaiementdonnerBancaire.AddCell(pdfPCell33);
                    document.Add(Headerfactrue);

                    additem(document, " ", 1, text);
                    additem(document, " ", 1, text);

                    document.Add(cliententreprise);
                    additem(document, " ", 1, text);
                    document.Add(itemsfactur);
                    document.Add(itemsfacturBody);
                    addseparater(document);
                    document.Add(pdftabtotal);

                    additem(document, " ", 2, font);
                    document.Add(ModepaiementdonnerBancaire);

                    //note 

                    PdfPTable Note = new PdfPTable(1);

                    Note.WidthPercentage = 100f;
                    Note.DefaultCell.Border = Rectangle.NO_BORDER;
                    PdfPCell pdfPCellNote1 = new PdfPCell();
                    pdfPCellNote1.Border = Rectangle.NO_BORDER;
                    pdfPCellNote1.AddElement(new Paragraph("Note:"));
                    PdfPCell pdfPCellNote2 = new PdfPCell();
                    pdfPCellNote2.MinimumHeight = 20;
                    pdfPCellNote2.AddElement(new Paragraph(textobservation.Text));

                    Note.AddCell(pdfPCellNote1);
                    Note.AddCell(pdfPCellNote2);

                    document.Add(Note);
                    //signtaure

                    PdfPTable pdftabSegnature = new PdfPTable(3);
                    pdftabSegnature.SetWidths(new float[] { 10, 10, 50 });
                    pdftabSegnature.DefaultCell.Border = Rectangle.NO_BORDER;
                    PdfPCell pdfCellvide1s = new PdfPCell(new Paragraph(" "));
                    pdfCellvide1s.Border = Rectangle.NO_BORDER;
                    PdfPCell pdfCellvide2s = new PdfPCell(new Paragraph(" "));
                    pdfCellvide2s.Border = Rectangle.NO_BORDER;

                    PdfPTable signature = new PdfPTable(1);
                    signature.DefaultCell.Border = Rectangle.NO_BORDER;
                    signature.SetWidths(new float[] { 20 });
                    PdfPCell pdfCellvideSignature = new PdfPCell(new Paragraph("Ségnature:"));
                    pdfCellvideSignature.Border = Rectangle.NO_BORDER;
                    iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(dbbyte);
                    pic.ScaleToFit(20, 20);
                    PdfPCell pdfCellvideSignature1 = new PdfPCell(pic);
                    pdfCellvideSignature1.Border = Rectangle.NO_BORDER;
                    signature.AddCell(pdfCellvideSignature);
                    signature.AddCell(pic);

                    pdftabSegnature.AddCell(pdfCellvide1s);
                    pdftabSegnature.AddCell(pdfCellvide2s);
                    pdftabSegnature.AddCell(signature);


                    additem(document, " ", 2, font);
                    document.Add(pdftabSegnature);

                    document.Close();

                    pdfWriter.Close();

                    //additem(document, "Logo", iTextSharp.text.Element.ALIGN_LEFT, text);
                    //additem(document, "N°Facture:", iTextSharp.text.Element.ALIGN_RIGHT, text);
                    //additem(document, "N°Facture:", iTextSharp.text.Element.ALIGN_RIGHT, text);
                    //additem(document, "Date Facturation:", iTextSharp.text.Element.ALIGN_RIGHT, text);
                    //additem(document, "Client :", iTextSharp.text.Element.ALIGN_RIGHT, text);
                    //additem(document, "Adresse :", iTextSharp.text.Element.ALIGN_RIGHT, text);


                }
            }




            Toast.MakeText(this, dir.AbsolutePath.ToString() + "" + System.IO.File.Exists(path).ToString(), ToastLength.Long).Show();
        }

        //Toast.MakeText(this,Java.IO.File.Separator.ToString(), ToastLength.Long).Show();








        private void additem(Document document, string text, int align, Font font)
        {
            Chunk chunk = new Chunk(text);
            Chunk imge = new Chunk();
            Paragraph p = new Paragraph(chunk);
            p.Alignment = align;
            document.Add(p);

        }

        private void addseparater(Document document)
        {
            LineSeparator lineSeparator = new LineSeparator();

            lineSeparator.LineColor = new iTextSharp.text.Color(0, 0, 100);
            addlinr(document);
            document.Add(new Chunk(lineSeparator));
            addlinr(document);
        }

        private void addlinr(Document document)
        {
            document.Add(new Paragraph(""));

        }
        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();
            var ListView = sender as ListView;
            var t = selectionCommendfacture(idfacture)[e.Position];
            int c = e.Position + 1;
            Intent intent = new Intent(this, typeof(DetailFacture));//Facturee));
            editorfacture.PutInt("idArticle", t.Idcomfact);
            editorfacture.PutString("NomArticle", t.NomArticle);
            editorfacture.PutFloat("Qte", t.Qte);
            editorfacture.PutFloat("Prix", t.Prix);
            editorfacture.PutString("Tyoeremose", t.Ttyperemise);
            editorfacture.PutFloat("Tauxremise", t.Tauxremise);
            editorfacture.PutFloat("Prix", t.Prix);
            editorfacture.PutFloat("TVA", t.Tva);
            //editorfacture.PutString("Typeclient", t.TypeClinet);
            //editorfacture.PutInt("IDclient", t.Idclient);
            // editorfacture.PutInt("Item", c);
            editorfacture.Apply();
            StartActivity(intent);
            Android.Widget.Toast.MakeText(this, t.Idcomfact.ToString(), Android.Widget.ToastLength.Short).Show();
        }

        private byte[] selectimage(int idfacture)
        {
            byte[] t = null;
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            Bitmap bit = null;
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                using (var contenu = connection.CreateCommand())
                {

                    contenu.CommandText = "Select Imagevvide from Imagefact where IdFacture=" + idfacture + "";

                    using (var r = contenu.ExecuteReader())
                    {


                        while (r.Read())
                        {

                            t = (byte[])r[0];

                        }
                    }


                }
                transaction.Commit();
            }

            connection.Close();

            return t;
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && data != null)
            {
                uri = data.Data;


                tet.Text = uri.Path;

            }
        }

        //btnsegnature.Click += delegate
        //{


        //    Intent intent = new Intent(this, typeof(LayoutSegnature));

        //    intent.PutExtra("IDfacture", idfacture);

        //    StartActivity(intent);


        //};

        // image.SetImageURI(Uri.FromFile(new Java.IO.File(sharedPreferences.GetString("ImagePath", ""))));
        //string path = sharedPreferences.GetString("ImagePath", " ");//  Uri imgUri = Uri.Parse(path);
        //BitmapDrawable bitmap= new BitmapDrawable();

        //private void creationpdf(string nomfichier)
        //{

        //    try
        //    {
        //        string path = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, nomfichier);

        //        bool Existe = System.IO.File.Exists(path);

        //        if (!Existe)
        //        {



        //            Document document = new Document();

        //            PdfWriter.GetInstance(document, new FileStream(nomfichier, FileMode.Create));


        //            document.Open();
        //            document.SetPageSize(PageSize.A4);
        //            document.AddCreationDate();
        //            document.AddAuthor(Clientsinfos(idclient));

        //            Android.Graphics.Color color = new Android.Graphics.Color(0, 153, 204, 255);
        //            float fontsize = 20.0f, valuesize = 20.0f;
        //            BaseFont baseFont = BaseFont.CreateFont();
        //            Font text = new Font(Font.NORMAL, Android.Graphics.Color.Black);
        //            additem(document, "Logo", iTextSharp.text.Element.ALIGN_LEFT, text);

        //            additem(document, "N°Facture:", iTextSharp.text.Element.ALIGN_RIGHT, text);
        //            additem(document, "N°Facture:", iTextSharp.text.Element.ALIGN_RIGHT, text);
        //            additem(document, "Date Facturation:", iTextSharp.text.Element.ALIGN_RIGHT, text);
        //            additem(document, "Client :", iTextSharp.text.Element.ALIGN_RIGHT, text);
        //            additem(document, "Adresse :", iTextSharp.text.Element.ALIGN_RIGHT, text);

        //            //titre de document 

        //            //    Font title = new Font(fontsize, 36, Font.BOLD, Android.Graphics.Color.Black);
        //            Toast.MakeText(this, System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles).ToString(), ToastLength.Long).Show();



        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

        //    }




        //}


    }
}
