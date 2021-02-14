using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Preferences;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Facturation.Class;
using iTextSharp.text;
using Java.IO;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Controls;

namespace Facturation
{
    [Activity(Label = "Signature", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation/*,MainLauncher =true*/)]
    public class Signaturetransfor : AppCompatActivity
    {
        List<Articleitem> articleitems = new List<Articleitem>();
        private List<Articleitem> selectionCommendDevis(int idDevis)
        {
            articleitems.Clear();
            string nom = "";
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


            return articleitems;

            connection.Close();

        }
        private void transformationFacture(string DateFacturation, string tranf, string Echeance, string devis, int idclient, string Etat, string modepaiement, string observation, byte[] signature)
        {

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@Byte", signature));
            contenu.CommandText = "Insert into Facture(DateFacturation,Datetranform,DateEchance,Devise,idClient,Etat,modepaiement,observation,signature) values('" + DateFacturation + "','" + tranf + "','" + Echeance + "','" + devis + "'," + idclient + ",'" + Etat + "','" + modepaiement + "','" + observation + "',@Byte)";

            contenu.ExecuteNonQuery();

            connection.Close();

        }
        private List<Devis> selectionlistDevis(int IdDevis)
        {
            List<Devis> devis = new List<Devis>();

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();

            var contenu = connection.CreateCommand();

            contenu.CommandText = "SELECT idDevis,Devise, idClient, modepaiement, observation from Devis   where IdDevis=" + IdDevis;

            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                devis.Add(new Devis(int.Parse(r[0].ToString()), r[1].ToString(), int.Parse(r[2].ToString()), r[3].ToString(), r[4].ToString()));


            }

            connection.Close();
            return devis;



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
        void InsertionArticleItem(List<Articleitem> articleitem, int idfacture)
        {

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);


            connection.Open();

            var command = connection.CreateCommand();
            for (int i = 0; i < articleitem.Count; i++)
            {
                command.CommandText = "insert into commandeFacture(Libille,TypeProd,PrixHT,Qte,TVA,Remise,IdFacture) values('" + articleitem[i].NomArticle + "','" + articleitem[i].Ttyperemise + "'," + articleitem[i].Prix + "," + articleitem[i].Qte + "," + articleitem[i].Tva + ",'" + articleitem[i].Tauxremise + "'," + idfacture + ")";
                command.ExecuteNonQuery();

            }
            

            connection.Close();

       //     Toast.MakeText(this, command.ToString(), ToastLength.Long).Show();

        }


        //private void InsertionImage(byte[] image,int IDfacture)
        //{

        //    string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "andtadgFactureDevis.db3");
        //    var connection = new SqliteConnection("Data Source=" + dbpath);
        //    connection.Open();
        //    var contenu = connection.CreateCommand();
        //    contenu.Parameters.Add(new SqliteParameter("@param1", image));
        //    contenu.Parameters.Add(new SqliteParameter("@param2", IDfacture));
        //    contenu.CommandText = "Insert into Imagefact(Imagevvide,IdFacture) values(@param1,@param2)";

        //    contenu.ExecuteNonQuery();

        //    connection.Close();

        //}

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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();
                base.OnCreate(savedInstanceState);

                int id = sharedPreferences.GetInt("IDDevis", 0);
                int iDfactransf = selectionCountFacture() + 1;

                string Dataechance = sharedPreferences.GetString("DateEchence", "");

                SetContentView(Resource.Layout.layoutSignature);

                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
                SetSupportActionBar(toolbar);
                      
                //SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                //SupportActionBar.SetHomeButtonEnabled(true);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));

                //Get a reference to the FingerPaintCanvasView from the Main.axml file
                //  fingerPaintCanvasView = FindViewById<FingerPaintCanvasView>(Resource.Id.fingerPaintCanvasView1);

                Button signature = FindViewById<Button>(Resource.Id.buttonrenregis);



                var signatureView = FindViewById<SignaturePadView>(Resource.Id.signaturePadView5);

                signatureView.BackgroundColor = Android.Graphics.Color.White;


                //  var img = FindViewById<ImageView>(Resource.Id.imageViewsign);


                signatureView.GetImageStreamAsync(SignatureImageFormat.Png);






                signature.Click += delegate
                {
                    Bitmap image = signatureView.GetImage();

                    //var tra = signatureView.GetImageStreamAsync(SignatureImageFormat.Png);
                    //int id = Intent.GetIntExtra("IDfacture", 0);
                    //string filename = "SignatureFacture" + id.ToString() + "";
                    //FileStream stream=null;
                    //Java.IO.File dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "SignatureImage");
                    //string path = System.IO.Path.Combine(dir.AbsolutePath, filename);
                    //bool Existe = System.IO.File.Exists(path);
                    //if (!dir.Exists())
                    //{
                    //    dir.Mkdir();

                    //    if (Existe)
                    //    {

                    MemoryStream bos = new MemoryStream();
                    image.Compress(Bitmap.CompressFormat.Png, 100, bos);
                    byte[] bitmapdata = bos.ToArray();
                    Intent intent = new Intent(this, typeof(Listacture));


                    //InsertionImage(bitmapdata, Intent.GetIntExtra("IDfacture",0));
                    intent.PutExtra("ImagePath", bitmapdata);

                    // img.SetImageBitmap(image);


                    if (bitmapdata == null && bos == null && image == null)
                    {


                        Toast.MakeText(this, "Signature oblier", ToastLength.Long).Show();


                    }
                    else
                    {
                        InsertionArticleItem(selectionCommendDevis(sharedPreferences.GetInt("IDDevis", 0)), selectionCountFacture() + 1);
                        string t= Intent.GetStringExtra("Emailadmin");
                            
                        transformationFacture(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"), sharedPreferences.GetString("DateEchence", ""), Devise(t), selectionlistDevis(id)[0].Idclient, "Non Payée", selectionlistDevis(id)[0].Modepaiement, selectionlistDevis(id)[0].Observation, bitmapdata);
                        intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
                        StartActivity(intent);
                    }

                };
            }
            catch (Exception ex)
            {

                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }
        }

    }




}





