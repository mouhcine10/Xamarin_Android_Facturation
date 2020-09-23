
using Android;
using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Facturation.Class;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Java.Sql;
using Mono.Data.Sqlite;
using Org.Apache.Http.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using Document = iTextSharp.text.Document;

namespace Facturation
{
    [Activity(Label = "UpdateDevis", Theme = "@style/AppTheme.NoActionBar")]
    public class UpdateDevis : AppCompatActivity
    {
        readonly string[] permissionGroupe = { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
        const int Requestid = 0;
        AdView ads1, ads2;
        string ma = "";
        string toast = "";
        Java.IO.File dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/FolderDevis");
        string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
        List<int> listid = new List<int>();
        private List<int> SelectioniditemDevis(int idDevis)
        {
            listid.Clear();

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select idcomdev from commandeDevis where IdDevis=" + idDevis;

            var d = contenu.ExecuteReader();

            while (d.Read())
            {
                listid.Add(int.Parse(d[0].ToString()));

            }
            //connection.Close();

            return listid;
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
        List<Articleitem> articleitems = new List<Articleitem>();
        private List<Articleitem> selectionCommendDevis(int idDevis)
        {
            articleitems.Clear();

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select idcomdev, Libille,Qte,PrixHT,TypeProd,Remise,TVA from commandeDevis where IdDevis=" + idDevis + "";

            var r = contenu.ExecuteReader();


            while (r.Read())
            {

                articleitems.Add(new Articleitem(int.Parse(r[0].ToString()), r[1].ToString(), float.Parse(r[2].ToString()), float.Parse(r[3].ToString()), r[4].ToString(), float.Parse(r[5].ToString()), float.Parse(r[6].ToString())));

            }
            connection.Close();



            return articleitems;

        }
        private List<Articleitem> selectionCommendDevis()
        {
            articleitems.Clear();

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select idcomdev, Libille,Qte,PrixHT,TypeProd,Remise,TVA from commandeDevis";

            var r = contenu.ExecuteReader();


            while (r.Read())
            {

                articleitems.Add(new Articleitem(int.Parse(r[0].ToString()), r[1].ToString(), float.Parse(r[2].ToString()), float.Parse(r[3].ToString()), r[4].ToString(), float.Parse(r[5].ToString()), float.Parse(r[6].ToString())));

            }
            connection.Close();



            return articleitems;

        }
        private float SelectionSoutotalDevis(int idDevis)
        {
            float t = 0;
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select Sum(PrixHT*Qte),count(*) from commandeDevis where IdDevis=" + idDevis + "";

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
            contenu.CommandText = "Select Sum(((PrixHT*Qte)*TVA)/100),count(*) from commandeDevis where  IdDevis=" + idDevis + "";


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
        private float SelectionRemiseDevis(int idDevis)
        {
            float t = 0;
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select Sum(Remise),count(*) from commandeDevis where IdDevis=" + idDevis + "";


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
        private byte[] GetSignatur(int iddevis)
        {
            byte[] t = null;
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var transaction = connection.BeginTransaction();


            var contenu = connection.CreateCommand();

            contenu.CommandText = "Select signature from Devis where IdDevis=" + iddevis + "";

            var r = contenu.ExecuteReader();
            while (r.Read())
            {
                if ((byte[])r[0] == null)
                {

                    t = null;
                }
                else
                {

                    t = (byte[])r[0];
                }
            }

            connection.Close();
            return t;

        }

        private void DeleteDevis(int IDdevis)
        {
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");

            var connection = new SqliteConnection("Data Source=" + dbpath);

            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idDevis", IDdevis));
            contenu.CommandText = "Delete from Devis where idDevis=@idDevis";


            contenu.ExecuteNonQuery();

            connection.Close();

        }


        //UpdateDevis

        private void UpdateDevisglobale(string datevalidation, string modepaiement, string observation, int iddevis)
        {
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");

            var connection = new SqliteConnection("Data Source=" + dbpath);

            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@datevalidation", datevalidation));
            contenu.Parameters.Add(new SqliteParameter("@iddevis", iddevis));
            contenu.Parameters.Add(new SqliteParameter("@modepaiement", modepaiement));

            contenu.Parameters.Add(new SqliteParameter("@observation", observation));
            contenu.CommandText = "Update Devis Set  DateValidation=@datevalidation,modepaiement=@modepaiement,observation=@observation WHERE idDevis=@iddevis";

            contenu.ExecuteNonQuery();

            connection.Close();




        }

        //SELECT idDevis, DateCreation, DateValidation, modepaiement from Devis

        private List<Devis> selectioninfo(int iddevis)
        {
            List<Devis> devis = new List<Devis>();
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");

            var connection = new SqliteConnection("Data Source=" + dbpath);

            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idDevis", iddevis));

            contenu.CommandText = "SELECT idDevis, DateCreation, DateValidation, modepaiement,observation from Devis where idDevis=@idDevis";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                devis.Add(new Devis(int.Parse(r[0].ToString()), r[1].ToString(), r[2].ToString(), r[3].ToString(), r[4].ToString()));

            }



            return devis;
        }

        private void UpdateEtat(string etat, int iddevis, string datetransformation)
        {
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");

            var connection = new SqliteConnection("Data Source=" + dbpath);

            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idDevis", iddevis));
            contenu.Parameters.Add(new SqliteParameter("@Etat", etat));
            contenu.Parameters.Add(new SqliteParameter("@datetransfo", datetransformation));
            contenu.CommandText = "Update Devis Set Etat=@Etat ,Datetransform=@datetransfo  where idDevis=@idDevis";

            contenu.ExecuteNonQuery();

            connection.Close();




        }
        private void DeleteCommandeDevis(int IDdevis)
        {
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");

            var connection = new SqliteConnection("Data Source=" + dbpath);

            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idDevis", IDdevis));

            contenu.CommandText = "DELETE from commandeDevis WHERE IdDevis=@idDevis";


            contenu.ExecuteNonQuery();

            connection.Close();

        }

        int Devisid;
        TextView idDevis;
        TextView DateCreationDevis;
        TextView DatevalideDevis;
        TextView textViewCLientupdateDevis;
        LinearLayout linearLayoutArticleUpdateDevis;
        LinearLayout linearLayoutCLientUpdatedevis;
        LinearLayout infodevisupdate;
        ListView listViewArticleUpdateDevis;
        TextView MttsoutotalUpdateDevis;
        TextView textViewMontantRemiseUpdateDevis;
        TextView textViewMontantTaxeUpdateDevis;
        TextView textViewMontanttotalttcUpdateDevis;
        Spinner spinupdateDevis;
        EditText textInputLayoutnoteDevis;
        ImageView ImageViewupdateDevis;
        TextView textViewNomEntreprise;
        Button transfaufacture;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                ma = Intent.GetStringExtra("Emailadmin");
                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();
                base.OnCreate(savedInstanceState);

                // Create your application here

                SetContentView(Resource.Layout.Updatedevis);

                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);


                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                SupportActionBar.Title = "Modifier Devis";
                Devisid = sharedPreferences.GetInt("IDDevis", 0);
                int idclient = sharedPreferences.GetInt("IDclient", 0);

                idDevis = FindViewById<TextView>(Resource.Id.textViewRefeDevis);

                idDevis.Text = sharedPreferences.GetInt("Idevisinfo", 0).ToString();
                DateCreationDevis = FindViewById<TextView>(Resource.Id.DateCreationupdateDevis);

                DateCreationDevis.Text = sharedPreferences.GetString("DateCreation", "");

                DatevalideDevis = FindViewById<TextView>(Resource.Id.DatevalideDevisUpdate);

                DatevalideDevis.Text = sharedPreferences.GetString("DateValidation", "");

                textViewCLientupdateDevis = FindViewById<TextView>(Resource.Id.textViewCLientupdate);

                textViewNomEntreprise = FindViewById<TextView>(Resource.Id.textViewNomEntreprise);

                textViewNomEntreprise.Text = Clientsinfos(sharedPreferences.GetString("Emailadmin", ""));

                linearLayoutArticleUpdateDevis = FindViewById<LinearLayout>(Resource.Id.linearLayoutArticleUpdateDevis);

                infodevisupdate = FindViewById<LinearLayout>(Resource.Id.linearLayout19Updatedevis);

                MttsoutotalUpdateDevis = FindViewById<TextView>(Resource.Id.MttsoutotalUpdateDevis);

                textViewMontantRemiseUpdateDevis = FindViewById<TextView>(Resource.Id.textViewMontantRemiseUpdateDevis);

                textViewMontantTaxeUpdateDevis = FindViewById<TextView>(Resource.Id.textViewMontantTaxeUpdateDevis);

                textViewMontanttotalttcUpdateDevis = FindViewById<TextView>(Resource.Id.textViewMontanttotalttcUpdateDevis);
                textInputLayoutnoteDevis = FindViewById<EditText>(Resource.Id.editTextOvservationUpdatedevis);

                textInputLayoutnoteDevis.Text = selectioninfo(Devisid)[0].Observation;
                spinupdateDevis = FindViewById<Spinner>(Resource.Id.spinnerModepaiementUpdatedevis);

                var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.ModePaimenet, Android.Resource.Layout.SimpleSpinnerItem);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spinupdateDevis.Adapter = adapter;
                spinupdateDevis.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

                ads1 = FindViewById<AdView>(Resource.Id.adView2updatedevis);
                ads2 = FindViewById<AdView>(Resource.Id.adView1updatedevis);

                var adRequest1 = new AdRequest.Builder().Build();
                ads1.LoadAd(adRequest1);
                var adRequest2 = new AdRequest.Builder().Build();
                ads2.LoadAd(adRequest2);


                int o = adapter.GetPosition(selectioninfo(Devisid)[0].Modepaiement);
                spinupdateDevis.SetSelection(o);

                ImageViewupdateDevis = FindViewById<ImageView>(Resource.Id.imageViewUpdateDevis);

                listViewArticleUpdateDevis = FindViewById<ListView>(Resource.Id.listViewArticleUpdateDevis);
                listViewArticleUpdateDevis.Adapter = new HomeScreenAdapterArticleFact(this, selectionCommendDevis(Devisid));

                linearLayoutCLientUpdatedevis = FindViewById<LinearLayout>(Resource.Id.linearLayoutCLientUpdatedevis);

                infodevisupdate.Click += delegate
                {

                    Intent intent = new Intent(this, typeof(Updatedevisupdateinfo));

                    editorfacture.PutString("IDdevis", idDevis.Text);
                    editorfacture.PutString("Entreprise", textViewNomEntreprise.Text);
                    editorfacture.PutString("DateCreation", DateCreationDevis.Text);
                    editorfacture.PutString("DateValidation", DatevalideDevis.Text);
                    editorfacture.Apply();
                    StartActivity(intent);



                };




                linearLayoutCLientUpdatedevis.Click += delegate
                {

                    Intent intent = new Intent(this, typeof(ListClientDevisupdate));

                    StartActivity(intent);




                };


                textViewCLientupdateDevis.Text = sharedPreferences.GetString("NomClient", "");

                linearLayoutArticleUpdateDevis.Click += delegate
                {

                    Intent intent = new Intent(this, typeof(AjouteritemDevisuodate));
                    editorfacture.PutInt("idArticle", 0);
                    editorfacture.PutString("NomArticle", "");
                    editorfacture.PutFloat("Qte", 0);
                    editorfacture.PutFloat("Prix", 0);
                    editorfacture.PutString("Tyoeremose", "");
                    editorfacture.PutFloat("Tauxremise", 0);
                    editorfacture.PutFloat("Prix", 0);
                    editorfacture.PutFloat("TVA", 0);

                    editorfacture.Apply();

                    string ma = Intent.GetStringExtra("Emailadmin");
                    intent.PutExtra("Emailadmin", ma);
                    StartActivity(intent);





                };


                listViewArticleUpdateDevis.ItemClick += OnListItemClick;


                MttsoutotalUpdateDevis.Text = SelectionSoutotalDevis(Devisid).ToString();

                textViewMontantRemiseUpdateDevis.Text = SelectionRemiseDevis(Devisid).ToString();

                textViewMontantTaxeUpdateDevis.Text = SelectionCalculTVA(Devisid).ToString();

                float d = (SelectionSoutotalDevis(Devisid) + SelectionCalculTVA(Devisid)) - SelectionRemiseDevis(Devisid);

                textViewMontanttotalttcUpdateDevis.Text = d.ToString();

                transfaufacture = FindViewById<Button>(Resource.Id.buttontransf);


                // ImageViewupdateDevis.SetImageBitmap(BitmapFactory.DecodeByteArray(GetSignatur(Devisid), 0, GetSignatur(Devisid).Length));
                transfaufacture.Click += delegate
                {

                    Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);

                    LayoutInflater layoutInflater = LayoutInflater.From(this);
                    View view = layoutInflater.Inflate(Resource.Layout.TikitBoxalert, null);
                    var userdata = view.FindViewById<EditText>(Resource.Id.textdateechacebox);
                    builder.SetView(view);
                    builder.SetTitle("Date Echéance").SetMessage("Ex: " + DateTime.Now.ToString("dd-MM-yyyy")).SetPositiveButton("Yes", (senderAlert, args) =>
                    {

                        Toast.MakeText(this, userdata.Text, ToastLength.Long).Show();

                        Intent intentsig = new Intent(this, typeof(Signaturetransfor));

                        editorfacture.PutInt("IDDevis", sharedPreferences.GetInt("IDDevis", 0));
                        editorfacture.PutString("NomClient", sharedPreferences.GetString("NomClient", " "));
                        editorfacture.PutString("DateEchence", userdata.Text);
                        //  editorfacture.PutString("Modepaiement", userdata.Text);
                        editorfacture.PutString("Observation", textInputLayoutnoteDevis.Text);
                        editorfacture.Apply();
                        string EMAIL = Intent.GetStringExtra("Emailadmin");

                       intentsig.PutExtra("Emailadmin",EMAIL);

                        StartActivity(intentsig);


                    }).SetNegativeButton("Reuron", (senderAlert, args) => { builder.Dispose(); });

                    builder.Show();

                    UpdateEtat("Validé", Devisid, DateTime.Now.ToString("dd/MM/yy"));
                    //transformationFacture(DateTime.Now.ToString("dd-MM-yy"),"", "EUR", 1, "Non Payé", " ","",GetSignatur(selectionCountFacture()+1)) ;


                    //InsertionArticleItem(selectionCommendDevis(), selectionCountFacture() + 1);






                };
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();


            }

        }
        public void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            toast = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.menuUpdateDevis, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();
            int id = item.ItemId;

            if (id == Android.Resource.Id.Home)
            {


                Intent intent = new Intent(this, typeof(ListDevis));
                string ma = Intent.GetStringExtra("Emailadmin");

                intent.PutExtra("Emailadmin", ma);
                StartActivity(intent);


            }



            if (id == Resource.Id.Delete)
            {
                try
                {

                    DeleteCommandeDevis(Devisid);
                    DeleteDevis(Devisid);
                }
                catch (Exception ex)
                {

                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

                }
                Intent intent = new Intent(this, typeof(ListDevis));
                StartActivity(intent);

            }
            if (id == Resource.Id.Update)
            {
                //Intent intent = new Intent(this, typeof(PDFlayout));
                //StartActivity(intent);


                UpdateDevisglobale(DatevalideDevis.Text, toast, textInputLayoutnoteDevis.Text, Devisid);
            }

            if (id == Resource.Id.dar)
            {


                Intent intent = new Intent(this, typeof(PDFlayoutDevis));

                if (CheckLocationPermissionwRITE())
                {
                    CreatefaileFolder("Devis" + Devisid + ".pdf");
                    intent.PutExtra("a", Getfile(dir.AbsolutePath, "Devis" + Devisid + ".pdf")[0].ToString());
                    intent.PutExtra("EmailEnt", Clientsinfosdev(Intent.GetStringExtra("Emailadmin"))[0].Email);
                    intent.PutExtra("NomEnt", Clientsinfosdev(Intent.GetStringExtra("Emailadmin"))[0].NomClinet);
                    intent.PutExtra("Nomclient", Clientsinfos(selectionclient(Devisid)[0].Idclient)[0].NomClinet);
                    intent.PutExtra("Emailclient", Clientsinfos(selectionclient(Devisid)[0].Idclient)[0].Email);
                    StartActivity(intent);

                }


            }

            return base.OnOptionsItemSelected(item);
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


        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();
            var ListView = sender as ListView;
            var t = selectionCommendDevis(Devisid)[e.Position];
            int c = e.Position + 1;
            Intent intent = new Intent(this, typeof(DetaildevisUpdate));//Facturee));
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

        void CreatefaileFolder(string filename)
        {
            //  Java.IO.File dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/FolderName");
            //string path = System.IO.Path.Combine(dir.AbsolutePath, filename);
            //bool Existe = System.IO.File.Exists(path);

            var pathcombine = System.IO.Path.Combine(dir.AbsolutePath, filename);
            bool Exists = System.IO.File.Exists(pathcombine);
            if (!Exists)
            {
                System.IO.File.Delete(filename);
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
                    iTextSharp.text.Font text = new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, Android.Graphics.Color.Black);

                    PdfPTable Headerfactrue = new PdfPTable(3);
                    Headerfactrue.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    //CellLogo 

                    iTextSharp.text.Image pica = iTextSharp.text.Image.GetInstance(Clientsinfosdev(ma)[0].Logo);
                    pica.ScaleToFit(100, 100);
                    PdfPCell pdfPCellLogo = new PdfPCell();
                    pdfPCellLogo.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    pdfPCellLogo.AddElement(pica);
                    //cellvide

                    PdfPCell pdfPCellmillieur = new PdfPCell();
                    pdfPCellmillieur.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    pdfPCellmillieur.AddElement(new Paragraph("  "));

                    // Cell infofacture
                    PdfPTable pdfPTableinfo = new PdfPTable(1);

                    pdfPTableinfo.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    pdfPTableinfo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    pdfPTableinfo.AddCell("Devis N°:" + Devisid);
                    pdfPTableinfo.WidthPercentage = 100F;
                    pdfPTableinfo.AddCell("Date creation:" + DateCreationDevis.Text);
                    pdfPTableinfo.AddCell("Date validiter:" + DatevalideDevis.Text);

                    PdfPCell pdfPCellInfoFact = new PdfPCell();
                    pdfPCellInfoFact.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    pdfPCellInfoFact.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    pdfPCellInfoFact.AddElement(pdfPTableinfo);

                    Headerfactrue.AddCell(pdfPCellLogo);
                    Headerfactrue.AddCell(pdfPCellmillieur);
                    Headerfactrue.AddCell(pdfPTableinfo);


                    iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.BOLD);
                    //info Entreprise Et client 

                    PdfPTable cliententreprise = new PdfPTable(3);

                    cliententreprise.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //cellEntreprise 

                    cliententreprise.SetWidths(new float[] { 100f, 50f, 100f });
                    PdfPCell cellentreprise = new PdfPCell();

                    cellentreprise.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    PdfPTable Tinfoentreprise = new PdfPTable(1);
                    Tinfoentreprise.WidthPercentage = 100;
                    Tinfoentreprise.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
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
                    Chunk da = new Chunk("Devis De:", FontFactory.GetFont("Arial", 15, iTextSharp.text.Color.GRAY));

                    Tinfoentreprise.AddCell(new Paragraph(da));
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
                    pdfPCellvideinfo.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    //PdfPCell pdfPCellvideinfo1 = new PdfPCell();
                    //pdfPCellvideinfo1.Border = Rectangle.NO_BORDER;


                    cliententreprise.AddCell(pdfPCellvideinfo);
                    //   cliententreprise.AddCell(pdfPCellvideinfo1);

                    //cellinfoclient
                    PdfPCell cellClinet = new PdfPCell();
                    cellClinet.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    PdfPTable Tinfoclient = new PdfPTable(1);
                    Tinfoclient.WidthPercentage = 100;
                    Tinfoclient.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    Tinfoclient.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Chunk d = new Chunk("Devis à:", FontFactory.GetFont("Arial", 15, iTextSharp.text.Color.GRAY));

                    Tinfoclient.AddCell(new Paragraph(d));
                    Tinfoclient.AddCell(Clientsinfos(selectionclient(Devisid)[0].Idclient)[0].NomClinet);
                    Tinfoclient.AddCell(Clientsinfos(selectionclient(Devisid)[0].Idclient)[0].Adresse);
                    Tinfoclient.AddCell(Clientsinfos(selectionclient(Devisid)[0].Idclient)[0].Codepostal);
                    Tinfoclient.AddCell(Clientsinfos(selectionclient(Devisid)[0].Idclient)[0].Ville);
                    Tinfoclient.AddCell(Clientsinfos(selectionclient(Devisid)[0].Idclient)[0].Pays);
                    Tinfoclient.AddCell(Clientsinfos(selectionclient(Devisid)[0].Idclient)[0].Email);
                    Tinfoclient.AddCell(Clientsinfos(selectionclient(Devisid)[0].Idclient)[0].Telephone);
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
                    //PdfPCell paysclient = new PdfPCell();
                    //paysclient.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    //paysclient.Border = Rectangle.NO_BORDER;
                    //paysclient.AddElement(new Paragraph("Maroc"));
                    //Tinfoclient.AddCell(paysclient);




                    cliententreprise.AddCell(Tinfoclient);

                    //Tableau items Produits

                    PdfPTable itemsfactur = new PdfPTable(6);
                    itemsfactur.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    itemsfactur.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    itemsfactur.HorizontalAlignment = 0;
                    itemsfactur.WidthPercentage = 100;
                    itemsfactur.DefaultCell.BorderWidthBottom = 0f;
                    // itemsfactur.SetWidths(new float[] { 5, 40, 10, 20, 25 });
                    itemsfactur.SpacingAfter = 40;
                    PdfPCell type = new PdfPCell(new Paragraph("Type"));
                    type.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    type.BackgroundColor = iTextSharp.text.Color.GREEN;
                    type.BorderWidthBottom = 1f;
                    PdfPCell Designation = new PdfPCell(new Paragraph("Désignation"));
                    Designation.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Designation.BackgroundColor = iTextSharp.text.Color.GREEN;
                    Designation.BorderWidthBottom = 1f;
                    PdfPCell Qte = new PdfPCell(new Paragraph("Qté"));
                    Qte.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Qte.BackgroundColor = iTextSharp.text.Color.GREEN;
                    Qte.BorderWidthBottom = 1f;
                    PdfPCell Prix = new PdfPCell(new Paragraph("Prix"));
                    Prix.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Prix.BackgroundColor = iTextSharp.text.Color.GREEN;
                    Prix.BorderWidthBottom = 1f;
                    PdfPCell Tva = new PdfPCell(new Paragraph("TVA"));
                    Tva.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Tva.BackgroundColor = iTextSharp.text.Color.GREEN;
                    Tva.BorderWidthBottom = 1f;
                    PdfPCell Remise = new PdfPCell(new Paragraph("Remise"));
                    Remise.Border = iTextSharp.text.Rectangle.NO_BORDER;
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
                    itemsfacturBody.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    itemsfacturBody.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    for (int i = 0; i < selectionCommendDevis(Devisid).Count; i++)
                    {

                        itemsfactur.AddCell(selectionCommendDevis(Devisid)[i].Ttyperemise);
                        itemsfactur.AddCell(selectionCommendDevis(Devisid)[i].NomArticle);
                        itemsfactur.AddCell(selectionCommendDevis(Devisid)[i].Qte.ToString());
                        itemsfactur.AddCell(selectionCommendDevis(Devisid)[i].Prix.ToString());
                        itemsfactur.AddCell(selectionCommendDevis(Devisid)[i].Tva.ToString());
                        itemsfactur.AddCell(selectionCommendDevis(Devisid)[i].Tauxremise.ToString());




                    }

                    //Detao


                    PdfPTable pdftabtotal = new PdfPTable(3);
                    pdftabtotal.SetWidths(new float[] { 50, 50, 100 });
                    pdftabtotal.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    PdfPCell pdfCell1 = new PdfPCell(new Paragraph(" "));
                    pdfCell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    PdfPCell pdfCell2 = new PdfPCell(new Paragraph(" "));
                    pdfCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    PdfPTable calcul = new PdfPTable(3);
                    calcul.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    calcul.DefaultCell.BorderWidthBottom = 0f;

                    calcul.SetWidths(new float[] { 50, 55, 20 });

                    PdfPCell soutotalText = new PdfPCell(new Paragraph("Sous-total:"));
                    soutotalText.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    PdfPCell soutotalSoume = new PdfPCell(new Paragraph(SelectionSoutotalDevis(Devisid).ToString()));
                    soutotalSoume.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    PdfPCell soutotalDevis = new PdfPCell(new Paragraph(Devise(Intent.GetStringExtra("Emailadmin"))));
                    soutotalDevis.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    PdfPCell TVAText = new PdfPCell(new Paragraph("TVA:"));
                    TVAText.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    PdfPCell TVASoume = new PdfPCell(new Paragraph(SelectionCalculTVA(Devisid).ToString()));
                    TVASoume.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    PdfPCell TVADevis = new PdfPCell(new Paragraph(Devise(Intent.GetStringExtra("Emailadmin"))));
                    TVADevis.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    float c = (SelectionSoutotalDevis(Devisid) + SelectionCalculTVA(Devisid)) - SelectionRemiseDevis(Devisid);
                    PdfPCell totalText = new PdfPCell(new Paragraph("Total TTC:"));
                    totalText.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    totalText.BorderWidthBottom = 1f;
                    totalText.BorderWidthTop = 1f;
                    PdfPCell totalSoume = new PdfPCell(new Paragraph(c.ToString()));
                    totalSoume.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    totalSoume.BorderWidthBottom = 1f;
                    totalSoume.BorderWidthTop = 1f;
                    PdfPCell totalDevis = new PdfPCell(new Paragraph(Devise(Intent.GetStringExtra("Emailadmin"))));
                    totalDevis.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    totalDevis.BorderWidthBottom = 1f;
                    totalDevis.BorderWidthTop = 1f;
                    PdfPCell RemiseText = new PdfPCell(new Paragraph("Remise:"));
                    RemiseText.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    PdfPCell RemiseSoume = new PdfPCell(new Paragraph(SelectionRemiseDevis(Devisid).ToString()));
                    RemiseSoume.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    PdfPCell RemiseDevis = new PdfPCell(new Paragraph(Devise(Intent.GetStringExtra("Emailadmin"))));
                    RemiseDevis.Border = iTextSharp.text.Rectangle.NO_BORDER;
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
                    pdfCell3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    pdfCell3.BorderWidthBottom = 1f;

                    pdftabtotal.AddCell(pdfCell1);
                    pdftabtotal.AddCell(pdfCell2);
                    pdftabtotal.AddCell(pdfCell3);

                    //Mode Paiement Donné bancaier

                    PdfPTable ModepaiementdonnerBancaire = new PdfPTable(3);
                    ModepaiementdonnerBancaire.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ModepaiementdonnerBancaire.SetWidths(new float[] { 120, 20, 200 });

                    PdfPTable pdfPTablemode = new PdfPTable(1);
                    pdfPTablemode.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    pdfPTablemode.AddCell("Mode Paiement:" + selectioninfo(Devisid)[0].Modepaiement);
                    PdfPCell pdfPCell11 = new PdfPCell(pdfPTablemode);
                    pdfPCell11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    pdfPCell11.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    PdfPCell pdfPCell22 = new PdfPCell();
                    pdfPCell22.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    PdfPCell pdfPCell33 = new PdfPCell();
                    pdfPCell33.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    PdfPTable banque = new PdfPTable(2);
                    banque.WidthPercentage = 100;
                    banque.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    banque.SetWidths(new float[] { 90, 190 });
                    banque.AddCell(new Paragraph("Banque:"));
                    banque.AddCell(new Paragraph(Infobanck()[0].Nombanque));
                    banque.AddCell(new Paragraph("BIC"));
                    banque.AddCell(new Paragraph(Infobanck()[0].Bic));
                    banque.AddCell(new Paragraph("IBAN"));
                    banque.AddCell(new Paragraph(Infobanck()[0].Iban));

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
                    Note.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    PdfPCell pdfPCellNote1 = new PdfPCell();
                    pdfPCellNote1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    pdfPCellNote1.AddElement(new Paragraph("Note:"));
                    PdfPCell pdfPCellNote2 = new PdfPCell();
                    pdfPCellNote2.MinimumHeight = 20;
                    pdfPCellNote2.AddElement(new Paragraph(textInputLayoutnoteDevis.Text));

                    Note.AddCell(pdfPCellNote1);
                    Note.AddCell(pdfPCellNote2);

                    document.Add(Note);
                    //signtaure

                    PdfPTable pdftabSegnature = new PdfPTable(3);
                    pdftabSegnature.SetWidths(new float[] { 10, 10, 50 });
                    pdftabSegnature.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    PdfPCell pdfCellvide1s = new PdfPCell(new Paragraph(" "));
                    pdfCellvide1s.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    PdfPCell pdfCellvide2s = new PdfPCell(new Paragraph(" "));
                    pdfCellvide2s.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    PdfPTable signature = new PdfPTable(1);
                    signature.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    signature.SetWidths(new float[] { 20 });
                    PdfPCell pdfCellvideSignature = new PdfPCell(new Paragraph("Signature:"));
                    pdfCellvideSignature.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    byte[] f = null;
                    iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(GetSignatur(Devisid));
                    pic.ScaleToFit(20, 20);
                    PdfPCell pdfCellvideSignature1 = new PdfPCell(pic);
                    pdfCellvideSignature1.Border = iTextSharp.text.Rectangle.NO_BORDER;
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




            //  Toast.MakeText(this, dir.AbsolutePath.ToString() + "" + System.IO.File.Exists(path).ToString(), ToastLength.Long).Show();
        }

        private List<Clients> selectionclient(int iddevis)
        {
            List<Clients> Clients = new List<Clients>();
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@idDevis", iddevis));
            contenu.CommandText = "select  Client.idClient , NomClinet from Devis INNER JOIN Client on Devis.idclient=Client.idclient WHERE idDevis=@idDevis";
            var r = contenu.ExecuteReader();
            while (r.Read())
            {

                Clients.Add(new Clients(int.Parse(r[0].ToString()), r[1].ToString()));

            }
            connection.Close();
            return Clients;
        }

        private void additem(Document document, string text, int align, iTextSharp.text.Font font)
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
        List<Banque> banques = new List<Banque>();
        List<Banque> Infobanck()
        {

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();

            // contenu.Parameters.Add(new SqliteParameter("@role",role));
            contenu.CommandText = " SELECT Banck.IDInfobanck, Banck.Nombanc, Banck.BIC, Banck.IBAN from Banck INNER JOIN ClientAdmin on Banck.idclientAdmin=ClientAdmin.idclientAdmin WHERE ClientAdmin.role='Admin'";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                banques.Add(new Banque(int.Parse(r[0].ToString()), r[1].ToString(), r[2].ToString(), r[3].ToString()));

            }

            connection.Close();

            return banques;
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






    }
}