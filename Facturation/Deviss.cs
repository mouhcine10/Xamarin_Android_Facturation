using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Facturation.Class;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace Facturation
{
    [Activity(Label = "Devis", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class Deviss : AppCompatActivity
    {
        LinearLayout infodevis;
        static byte[] inten;
        Bitmap bitmap;
        TextView IDdevis;

        TextView textclient;
        TextView Textviewnameentreprise;

        TextView textviewdatedevis;


        TextView textviewdatevalide;

        TextView textviewSoutotal;

        TextView textviewtaxe;

        TextView textviewremise;

        TextView textviewtotal;

        LinearLayout client;

        ListView listarticle;

        EditText textnote;

        LinearLayout tablemttdevis;

        AdView ads1, ads2;

        Java.IO.File dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Folderdevis");

        readonly string[] permissionGroupe = { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
        const int Requestid = 0;

        static int IdDevis;
        int idclient = 0;
        string toast = "";
        Spinner spinnerModepaiement;
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


        public void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            toast = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
        }
        List<int> listid = new List<int>();

        //
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

        private void InsertionDevis(string DateCreation, string DateValidite, string devis, int idclient, string Etat, string modepaiement, string observation, byte[] signature)
        {

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@Byte", signature));
            contenu.CommandText = "Insert into Devis(DateCreation,DateValidation,Devise,idClient,Etat,modepaiement,observation,signature) values('" + DateCreation + "','" + DateValidite + "','" + devis + "'," + idclient + ",'" + Etat + "','" + modepaiement + "','" + observation + "',@Byte)";

            contenu.ExecuteNonQuery();

            connection.Close();

        }

        private List<int> SelectioniditemDevis(int idDevis)
        {
            listid.Clear();

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select idcomdev from commandeDevis where idDevis=" + idDevis;

            var d = contenu.ExecuteReader();

            while (d.Read())
            {
                listid.Add(int.Parse(d[0].ToString()));

            }
            //connection.Close();

            return listid;
        }

        List<Articleitem> articleitems = new List<Articleitem>();
        private List<Articleitem> selectionCommendDevis(int idDevis)
        {
            articleitems.Clear();

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select idcomdev, Libille,Qte,PrixHT,TypeProd,Remise,TVA from commandeDevis where idDevis=" + idDevis + "";

            var r = contenu.ExecuteReader();


            while (r.Read())
            {

                articleitems.Add(new Articleitem(int.Parse(r[0].ToString()), r[1].ToString(), float.Parse(r[2].ToString()), float.Parse(r[3].ToString()), r[4].ToString(), float.Parse(r[5].ToString()), float.Parse(r[6].ToString())));

            }
            connection.Close();

            return articleitems;



        }

        public int selectionCountDevis(int IDDevis)
        {
            int i = 0;
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();


            contenu.Parameters.Add(new SqliteParameter("@IDDevis", IDDevis));
            contenu.CommandText = "Select idDevis  from  Devis where idDevis=@IDDevis";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {
                i = int.Parse(r[0].ToString());


            }

            connection.Close();
            return i;

        }

        private void DeleteListCommandeDevis(int IDDevis)
        {
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");

            var connection = new SqliteConnection("Data Source=" + dbpath);

            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@IDDevis", IDDevis));
            contenu.CommandText = "Delete from commandeDevis where idDevis=@IDDevis";


            contenu.ExecuteNonQuery();

            connection.Close();

        }
        private float SelectionSoutotalDevis(int idDevis)
        {
            float t = 0;
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select Sum(PrixHT*Qte),count(*) from commandeDevis where idDevis=" + idDevis + "";

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
        private float SelectionCalculTVA(int idDevis)
        {

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select Sum(((PrixHT*Qte)*TVA)/100),count(*) from commandeDevis where  idDevis=" + idDevis + "";


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
        private float SelectionRemiseDevis(int idDevis)
        {
            float t = 0;
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select Sum(Remise),count(*) from commandeDevis where idDevis=" + idDevis + "";


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
        protected override void OnCreate(Bundle savedInstanceState)
        {

           
                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editordevis = sharedPreferences.Edit();

                idclient = sharedPreferences.GetInt("IDclient", 0);

                IdDevis = sharedPreferences.GetInt("IDDevis", 0);
                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.Devis);

                //var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
                //toolbar.SetTitleTextColor(Android.Graphics.Color.GreenYellow);
                //toolbar.SetBackgroundColor(Android.Graphics.Color.White);
                //SetActionBar(toolbar);
                //ActionBar.Title = "Devis";
                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));

                infodevis = FindViewById<LinearLayout>(Resource.Id.ly2);

                IDdevis = FindViewById<TextView>(Resource.Id.textViewIDdevis);

                textclient = FindViewById<TextView>(Resource.Id.textViewCLientDevis);
                Textviewnameentreprise = FindViewById<TextView>(Resource.Id.textViewEnreptisedevis);

                textviewdatedevis = FindViewById<TextView>(Resource.Id.textViewdatedevise);


                textviewdatevalide = FindViewById<TextView>(Resource.Id.textViewdatevalidation);

                textviewSoutotal = FindViewById<TextView>(Resource.Id.textViewSoutoDevis);

                textviewtaxe = FindViewById<TextView>(Resource.Id.textViewTaxeDevis);

                textviewremise = FindViewById<TextView>(Resource.Id.textViewRemisDevis);

                textviewtotal = FindViewById<TextView>(Resource.Id.textViewTotalDevis);

                client = FindViewById<LinearLayout>(Resource.Id.linearLayoutCLient);

                listarticle = FindViewById<ListView>(Resource.Id.listViewarticleDevis);

                ads1 = FindViewById<AdView>(Resource.Id.adViewDevis);
                ads2 = FindViewById<AdView>(Resource.Id.adView2Devis);
                var adRequest1 = new AdRequest.Builder().Build();
                ads1.LoadAd(adRequest1);
                var adRequest2 = new AdRequest.Builder().Build();
                ads2.LoadAd(adRequest2);



                tablemttdevis = FindViewById<LinearLayout>(Resource.Id.linearLayoutTablemontant);


                textnote = FindViewById<EditText>(Resource.Id.editTextOvservation);
                // tablemttdevis.Visibility = Android.Views.ViewStates.Invisible;


                var ajouterarticleService = FindViewById<LinearLayout>(Resource.Id.linearLayoutAddArticle);



                var SIgnutre = FindViewById<Button>(Resource.Id.SignatureDevise);

                var imagsignature = FindViewById<ImageView>(Resource.Id.imageViewSignatureDevis);

                spinnerModepaiement = FindViewById<Spinner>(Resource.Id.spinnerModepaiementDevis);
                //  genpdf = FindViewById<Button>(Resource.Id.buttonGenererPDFFActure);

                var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.ModePaimenet, Android.Resource.Layout.SimpleSpinnerItem);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spinnerModepaiement.Adapter = adapter;
                spinnerModepaiement.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);


                //var buttonAjouterPiece = FindViewById<Button>(Resource.Id.buttonAjouterPiece);     


                IDdevis.Text = "Devis:" + IdDevis.ToString();
                Textviewnameentreprise.Text = "Entreprise:\n" + Clientsinfos(sharedPreferences.GetString("Emailadmin", ""));
                textviewdatedevis.Text = sharedPreferences.GetString("DateCreation", " ");
                textviewdatevalide.Text = sharedPreferences.GetString("DateValidite", " ");

                infodevis.Click += delegate

            {

                editordevis.PutInt("IDDevis", sharedPreferences.GetInt("IDDevis", 0));
                editordevis.PutString("Entreprise", Clientsinfos(sharedPreferences.GetString("Emailadmin", "")));
                editordevis.PutString("DateCreation", textviewdatedevis.Text);
                editordevis.PutString("DateValidite", textviewdatevalide.Text);
                editordevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editordevis.Apply();

                Intent intent = new Intent(this, typeof(DevisDetail));
                intent.PutExtra("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                StartActivity(intent);

            };

                //ajouterarticleService.Click += delegate
                //{


                //    Intent intent = new Intent(this, typeof(DetailDevisArticle));

                //    StartActivity(intent);



                //};

                client.Click += delegate
                {
                    editordevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                    editordevis.Apply();


                    Intent intent = new Intent(this, typeof(ListClientDevis));
                    intent.PutExtra("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                    StartActivity(intent);




                };
                textclient.Text = sharedPreferences.GetString("titre", "choisir votre client");
                ajouterarticleService.Click += delegate
                {
                    editordevis.PutInt("IDDevis", sharedPreferences.GetInt("IDDevis", 0));
                    editordevis.PutString("NomArticle", " ");
                    editordevis.PutFloat("Qte", 0);
                    editordevis.PutFloat("Prix", 0);
                    editordevis.PutString("Tyoeremose", " ");
                    editordevis.PutFloat("Tauxremise", 0);
                    editordevis.PutFloat("TVA", 0);
                    editordevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                    editordevis.Apply();

                    editordevis.Apply();
                    Intent intent = new Intent(this, typeof(DetailDevisArticle));
                    intent.PutExtra("Emailadmin", sharedPreferences.GetString("Emailadmin",""));
                    StartActivity(intent);


                };

                listarticle.Adapter = new HomeScreenAdapterArticleFact(this, selectionCommendDevis(sharedPreferences.GetInt("IDDevis", 0)));

                listarticle.ItemClick += OnListItemClick;
                textviewSoutotal.Text = SelectionSoutotalDevis(sharedPreferences.GetInt("IDDevis", 0)).ToString();
                textviewtaxe.Text = SelectionCalculTVA(sharedPreferences.GetInt("IDDevis", 0)).ToString();
                textviewremise.Text = SelectionRemiseDevis(sharedPreferences.GetInt("IDDevis", 0)).ToString();

                float c = SelectionSoutotalDevis(sharedPreferences.GetInt("IDDevis", 0)) + SelectionCalculTVA(sharedPreferences.GetInt("IDDevis", 0));

                float d = c - SelectionRemiseDevis(sharedPreferences.GetInt("IDDevis", 0));
                textviewtotal.Text = d.ToString();

                inten = Intent.GetByteArrayExtra("ImagePath");

                if (inten == null)
                {

                    SIgnutre.Click += delegate
                    {
                        Intent intent = new Intent(this, typeof(LayoutsignatureDevis));
                        editordevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                        editordevis.Apply();

                        intent.PutExtra("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));

                        StartActivity(intent);


                    };
                }
                else
                {

                    bitmap = BitmapFactory.DecodeByteArray(inten, 0, inten.Length);


                   // imagsignature.SetImageBitmap(bitmap);


                }





            
            //catch (Exception ex)
            //{

            //    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            //}


        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.menuDevis, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editordevis = sharedPreferences.Edit();
            int id = item.ItemId;

            if (item.ItemId == Android.Resource.Id.Home)
            {
                if (selectionCountDevis(sharedPreferences.GetInt("IDDevis", 0)) == sharedPreferences.GetInt("IDDevis", 0))
                {
                    editordevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin",""));
                    editordevis.Apply();
                    Intent intent = new Intent(this, typeof(ListDevis));
                    string m = Intent.GetStringExtra("Emailadmin");

                    intent.PutExtra("Emailadmin", m);

                    StartActivity(intent);

                }
                else
                {
                    DeleteListCommandeDevis(sharedPreferences.GetInt("IDDevis", 0));
                    textclient.Text = "choisir votre client";
                    string m = Intent.GetStringExtra("Emailadmin");


                    editordevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                    editordevis.Apply();
                    Intent intent = new Intent(this, typeof(ListDevis));
                    intent.PutExtra("Emailadmin", m);
                    StartActivity(intent);
                }


            }

            if (id == Resource.Id.Enregistrer)
            {

                if (inten != null)
                {

                    InsertionDevis(textviewdatedevis.Text, textviewdatevalide.Text, Devise(sharedPreferences.GetString("Emailadmin", "")), idclient, "Encours", toast, textnote.Text, inten);

                    editordevis.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                    editordevis.Apply();

                  
                    string m = Intent.GetStringExtra("Emailadmin");
                    Intent intent = new Intent(this, typeof(ListDevis));


                    // Toast.MakeText(this, "Your Devis has benn Added", ToastLength.Long).Show();

                    //CreatefaileFolder("Devis" + sharedPreferences.GetInt("IDDevis", 0) + ".pdf");

                    intent.PutExtra("Emailadmin", m);

                    StartActivity(intent);

                }

                else
                {
                    Toast.MakeText(this, "vérifier votre signature", ToastLength.Long).Show();

                }



            }
            if (id == Resource.Id.Envoyer)
            {


            }
            //if (id == Resource.Id.Preview)
            //{

            //    if (selectionCountDevis(sharedPreferences.GetInt("IDDevis", 0)) == sharedPreferences.GetInt("IDDevis", 0))
            //    {

            //        Intent intent = new Intent(this, typeof(PDFlayout));
            //        if (CheckLocationPermissionwRITE())
            //        {
            //            //                        CreatefaileFolder("Devis1.pdf");



            //            intent.PutExtra("a", Getfile(dir.AbsolutePath, "Devis" + sharedPreferences.GetInt("IDDevis", 0) + ".pdf")[0].ToString());




            //            StartActivity(intent);
            //        }
            //    }
            //    else
            //    {
            //        Android.App.AlertDialog.Builder b = new Android.App.AlertDialog.Builder(this);

            //        b.SetTitle("Information").SetMessage("Ajouter votre Devis d'abord").SetPositiveButton("OK", (sender, args) => { b.Dispose(); });

            //        b.Show();
            //    }
            //}

            return base.OnOptionsItemSelected(item);
        }


        public override void OnBackPressed()
        {
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();
            if (selectionCountDevis(sharedPreferences.GetInt("IDDevis", 0)) == sharedPreferences.GetInt("IDDevis", 0))
            {
                editorfacture.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorfacture.Apply();


                Intent intent = new Intent(this, typeof(ListDevis));
                intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));

                StartActivity(intent);

            }
            else
            {
                DeleteListCommandeDevis(sharedPreferences.GetInt("IDDevis", 0));
                textclient.Text = "choisir votre client";
                editorfacture.PutString("Emailadmin", sharedPreferences.GetString("Emailadmin", ""));
                editorfacture.Apply();

                Intent intent = new Intent(this, typeof(ListDevis));
                intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));

                StartActivity(intent);
            }
            base.OnBackPressed();


        }

        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();
            //   var ListView = sender as ListView;
            var t = selectionCommendDevis(IdDevis)[e.Position];
            // int c = e.Position + 1;
            Intent intent = new Intent(this, typeof(DetailDevisArticle));//Facturee));
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
            Java.IO.File dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Folderdevis");
            //string path = System.IO.Path.Combine(dir.AbsolutePath, filename);
            //bool Existe = System.IO.File.Exists(path);
            var pathcombine = System.IO.Path.Combine(dir.AbsolutePath, filename);
            bool Exists = File.Exists(pathcombine);
            if (!Exists)
            {

                dir.Mkdir();
            }


            Document document = new Document();

            PdfWriter pdfWriter = PdfWriter.GetInstance(document, new FileStream(dir.AbsolutePath + "/" + filename, FileMode.Create));
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

            PdfPCell pdfPCellLogo = new PdfPCell();
            pdfPCellLogo.Border = Rectangle.NO_BORDER;
            pdfPCellLogo.AddElement(new Paragraph(""));
            //cellvide

            PdfPCell pdfPCellmillieur = new PdfPCell
            {
                Border = Rectangle.NO_BORDER
            };
            pdfPCellmillieur.AddElement(new Paragraph("  "));

            // Cell infofacture
            PdfPTable pdfPTableinfo = new PdfPTable(1);

            pdfPTableinfo.DefaultCell.Border = Rectangle.NO_BORDER;
            pdfPTableinfo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
            pdfPTableinfo.AddCell("Facture N°: 1");
            pdfPTableinfo.WidthPercentage = 100F;
            pdfPTableinfo.AddCell("Facturation:20/05/2020");
            pdfPTableinfo.AddCell("Echence:20/08/2020");

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
            Tinfoentreprise.AddCell("Xamspft");
            Tinfoentreprise.AddCell("224 rue adaana");
            Tinfoentreprise.AddCell("75015");
            Tinfoentreprise.AddCell("Paris");
            Tinfoentreprise.AddCell("France");
            Tinfoentreprise.AddCell("mouhcine.ifouls");
            Tinfoentreprise.AddCell("0505454005");



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
            Tinfoclient.AddCell("CFAO:");
            Tinfoclient.AddCell("251,rue abdo");
            Tinfoclient.AddCell("casablanca");
            Tinfoclient.AddCell("Maroc");
            Tinfoclient.AddCell("Abdo@gmgm.com");
            Tinfoclient.AddCell("045454545");
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
            for (int i = 0; i < selectionCommendDevis(2).Count; i++)
            {

                itemsfactur.AddCell(selectionCommendDevis(2)[i].Ttyperemise);
                itemsfactur.AddCell(selectionCommendDevis(2)[i].NomArticle);
                itemsfactur.AddCell(selectionCommendDevis(2)[i].Qte.ToString());
                itemsfactur.AddCell(selectionCommendDevis(2)[i].Prix.ToString());
                itemsfactur.AddCell(selectionCommendDevis(2)[i].Tva.ToString());
                itemsfactur.AddCell(selectionCommendDevis(2)[i].Tauxremise.ToString());




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

            calcul.SetWidths(new float[] { 50, 50, 20 });

            PdfPCell soutotalText = new PdfPCell(new Paragraph("Sous-total:"));
            soutotalText.Border = Rectangle.NO_BORDER;
            PdfPCell soutotalSoume = new PdfPCell(new Paragraph(SelectionSoutotalDevis(2).ToString()));
            soutotalSoume.Border = Rectangle.NO_BORDER;
            PdfPCell soutotalDevis = new PdfPCell(new Paragraph("EUR"));
            soutotalDevis.Border = Rectangle.NO_BORDER;
            PdfPCell TVAText = new PdfPCell(new Paragraph("TVA:"));
            TVAText.Border = Rectangle.NO_BORDER;
            PdfPCell TVASoume = new PdfPCell(new Paragraph(SelectionCalculTVA(2).ToString()));
            TVASoume.Border = Rectangle.NO_BORDER;
            PdfPCell TVADevis = new PdfPCell(new Paragraph("EUR"));
            TVADevis.Border = Rectangle.NO_BORDER;
            float c = (SelectionSoutotalDevis(2) + SelectionCalculTVA(2)) - SelectionRemiseDevis(2);
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
            PdfPCell RemiseSoume = new PdfPCell(new Paragraph(SelectionRemiseDevis(2).ToString()));
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
            //  pdfPCellNote2.AddElement(new Paragraph(Note));

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
            // iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(dbbyte);
            //pic.ScaleToFit(50, 20);
            //PdfPCell pdfCellvideSignature1 = new PdfPCell(pic);
            //pdfCellvideSignature1.Border = Rectangle.NO_BORDER;
            //signature.AddCell(pdfCellvideSignature);
            //signature.AddCell(pic);

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






            // Toast.MakeText(this, dir.AbsolutePath.ToString() + "" + System.IO.File.Exists(path).ToString(), ToastLength.Long).Show();


            //Toast.MakeText(this,Java.IO.File.Separator.ToString(), ToastLength.Long).Show();

        }
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

    }
}