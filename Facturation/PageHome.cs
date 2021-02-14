using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;

namespace Facturation
{
    [Activity(Label = "PageHome", Theme = "@style/AppTheme", Icon = "@drawable/Noir", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class PageHome : Activity
    {
        const int Requestid = 0;
        readonly string[] permissionGroupe = { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };

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
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Android.Content.PM.Permission.Granted)
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
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != Android.Content.PM.Permission.Granted)
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

        private string selectionEmail(string Email)
        {
            string email = "";

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();

            contenu.Parameters.Add(new SqliteParameter("@email", Email));
            contenu.CommandText = "SELECT Emailclient from ClientAdmin WHERE Emailclient=@email";


            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                email = r[0].ToString();

            }

            connection.Close();

            return email;




        }

        private void Etatconx()
        {
            string email = "";

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();

            contenu.Parameters.Add(new SqliteParameter("@email", Email));
            contenu.CommandText = "Update Clientadmin  set Conexteat='C' where Role='Admin' ";


            contenu.ExecuteNonQuery();

        }
        private string selectionEmail()
        {
            string email = "";

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();

            contenu.CommandText = "SELECT Emailclient from ClientAdmin WHERE role='Admin'";


            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                email = r[0].ToString();

            }

            connection.Close();

            return email;




        }
        //public void CreationDatabase()
        //{

        //    string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
        //    var connection = new SqliteConnection("Data Source=" + dbpath);
        //    bool Exists = File.Exists(dbpath);


        //    if (!Exists)
        //    {

        //        Mono.Data.Sqlite.SqliteConnection.CreateFile(dbpath);
        //        List<string> commands = new List<string>() { "Create Table ClientAdmin( idClientadmin INTEGER primary key, TypeClinet ntext,NomClinet ntext ,Telephone NUMERIC,Emailclient ntext,adresse ntext,CodePostal ntext, ville ntext, pays ntext,Logo BLOB ,Mdp text,confirmdp text,role text)",
        //            "Create Table Banck(IDInfobanck INTEGER primary key AUTOINCREMENT, Nombanc ntext ,BIC ntext,IBAN ntext,idclientAdmin INTEGER  REFERENCES ClientAdmin(idClient) on delete cascade,Devise ntext)",
        //            "Create Table Client( idClient INTEGER primary key, TypeClinet ntext,NomClinet ntext ,Telephone NUMERIC,Emailclient ntext,adresse ntext,CodePostal ntext, ville ntext, pays ntext,Logo BLOB ,Mdp text,confirmdp text,role text)",
        //            "Create Table Facture( idFacture INTEGER primary key, DateFacturation ntext, Datetranform ntext ,DateEchance ntext , DatePaiement ntext ,Devise ntext, idClient INTEGER  REFERENCES Client(IdClient) ,Etat ntext ,modepaiement ntext,observation ntext,signature BLOB)",
        //            //"Create Table Imagefact(Idimage INTEGER primary key AUTOINCREMENT,Imagevvide BLOB, IdFacture INTEGER REFERENCES Facture(idFacture) on delete cascade)",

        //            "Create Table commandeFacture(Idcomfact INTEGER primary key AUTOINCREMENT, IdFacture INTEGER REFERENCES Facture(idFacture) on delete cascade, Libille ntext,TypeProd ntext , PrixHT REAL,Qte INTEGER, TVA REAL,Remise REAL)",

        //            "Create Table Devis(idDevis INTEGER primary key ,DateCreation ntext,DateValidation ntext ,Datetransform ntext , Devise ntext, idClient INTEGER  REFERENCES Client(IdClient),Etat ntext, modepaiement ntext ,observation ntext,signature BLOB)",

        //            "Create Table commandeDevis(idcomdev INTEGER primary key AUTOINCREMENT, IdDevis INTEGER REFERENCES Devis(idDevis)on delete cascade ,libille ntext,TypeProd ntext, Prixht REAL, Qte INTEGER, TVA REAL, Remise REAL)",
        //            "Create Table Produit(idProduit INTEGER primary key AUTOINCREMENT, Nomproduit ntext ,Prix REAL,typePro ntext,codebar ntext)",

        //            //"Create Table FilesFact(idFailefact INTEGER primary key AUTOINCREMENT, Nomfile ntext,Pathfile ntext,IdFacture INTEGER REFERENCES Facture(idFacture) on delete cascade)",

        //            //"Create Table FilesDev(idFaileDev INTEGER primary key AUTOINCREMENT, Nomfile ntext,Pathfile ntext,IdDevis INTEGER REFERENCES Devis(idFacture) on delete cascade)"


        //        };
        //        connection.Open();

        //        var command = connection.CreateCommand();
        //        for (int i = 0; i < commands.Count; i++)
        //        {

        //            command.CommandText = commands[i];
        //            command.ExecuteNonQuery();
        //            Toast.MakeText(this, commands[i].ToString(), ToastLength.Long).Show();


        //        }


        //        connection.Close();
        //    }
        //}
        //public void CreationDatabase()
        //{

        //    string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
        //    var connection = new SqliteConnection("Data Source=" + dbpath);
        //    bool Exists = File.Exists(dbpath);


        //    if (!Exists)
        //    {

        //        Mono.Data.Sqlite.SqliteConnection.CreateFile(dbpath);
        //        List<string> commands = new List<string>() { "Create Table ClientAdmin( idClientadmin INTEGER primary key, TypeClinet ntext,NomClinet ntext ,Telephone NUMERIC,Emailclient ntext,adresse ntext,CodePostal ntext, ville ntext, pays ntext,Logo BLOB ,Mdp text,confirmdp text,role text,Conexteat ntext)",
        //            "Create Table Banck(IDInfobanck INTEGER primary key AUTOINCREMENT, Nombanc ntext ,BIC ntext,IBAN ntext,idclientAdmin INTEGER  REFERENCES ClientAdmin(idClient) on delete cascade,Devise ntext)",
        //            "Create Table Client( idClient INTEGER primary key, TypeClinet ntext,NomClinet ntext ,Telephone NUMERIC,Emailclient ntext,adresse ntext,CodePostal ntext, ville ntext, pays ntext,Logo BLOB ,Mdp text,confirmdp text,role text)",
        //            "Create Table Facture( idFacture INTEGER primary key, DateFacturation ntext, Datetranform ntext ,DateEchance ntext , DatePaiement ntext ,Devise ntext, idClient INTEGER  REFERENCES Client(IdClient) ,Etat ntext ,modepaiement ntext,observation ntext,signature BLOB)",
        //            //"Create Table Imagefact(Idimage INTEGER primary key AUTOINCREMENT,Imagevvide BLOB, IdFacture INTEGER REFERENCES Facture(idFacture) on delete cascade)",

        //            "Create Table commandeFacture(Idcomfact INTEGER primary key AUTOINCREMENT, IdFacture INTEGER REFERENCES Facture(idFacture) on delete cascade, Libille ntext,TypeProd ntext , PrixHT REAL,Qte INTEGER, TVA REAL,Remise REAL)",

        //            "Create Table Devis(idDevis INTEGER primary key ,DateCreation ntext,DateValidation ntext ,Datetransform ntext , Devise ntext, idClient INTEGER  REFERENCES Client(IdClient),Etat ntext, modepaiement ntext ,observation ntext,signature BLOB)",

        //            "Create Table commandeDevis(idcomdev INTEGER primary key AUTOINCREMENT, IdDevis INTEGER REFERENCES Devis(idDevis)on delete cascade ,libille ntext,TypeProd ntext, Prixht REAL, Qte INTEGER, TVA REAL, Remise REAL)",
        //            "Create Table Produit(idProduit INTEGER primary key AUTOINCREMENT, Nomproduit ntext ,Prix REAL,typePro ntext,codebar ntext)",

        //            //"Create Table FilesFact(idFailefact INTEGER primary key AUTOINCREMENT, Nomfile ntext,Pathfile ntext,IdFacture INTEGER REFERENCES Facture(idFacture) on delete cascade)",

        //            //"Create Table FilesDev(idFaileDev INTEGER primary key AUTOINCREMENT, Nomfile ntext,Pathfile ntext,IdDevis INTEGER REFERENCES Devis(idFacture) on delete cascade)"


        //        };
        //        connection.Open();

        //        var command = connection.CreateCommand();
        //        for (int i = 0; i < commands.Count; i++)
        //        {

        //            command.CommandText = commands[i];
        //            command.ExecuteNonQuery();
        //            Toast.MakeText(this, commands[i].ToString(), ToastLength.Long).Show();


        //        }


        //        connection.Close();
        //    }
        //}
        public void CreationDatabase()
        {

            string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            bool Exists = File.Exists(dbpath);


            if (!Exists)
            {

                Mono.Data.Sqlite.SqliteConnection.CreateFile(dbpath);
                List<string> commands = new List<string>() { "Create Table ClientAdmin( idClientadmin INTEGER primary key, TypeClinet ntext,NomClinet ntext ,Telephone NUMERIC,Emailclient ntext,adresse ntext,CodePostal ntext, ville ntext, pays ntext,Logo BLOB ,Mdp text,confirmdp text,role text,Conexteat ntext)",
                    "Create Table Banck(IDInfobanck INTEGER primary key AUTOINCREMENT, Nombanc ntext ,BIC ntext,IBAN ntext,idclientAdmin INTEGER  REFERENCES ClientAdmin(idClient) on delete cascade,Devise ntext)",
                    "Create Table Client( idClient INTEGER primary key, TypeClinet ntext,NomClinet ntext ,Telephone NUMERIC,Emailclient ntext,adresse ntext,CodePostal ntext, ville ntext, pays ntext,Logo BLOB ,Mdp text,confirmdp text,role text)",
                    "Create Table Facture( idFacture INTEGER primary key, DateFacturation ntext, Datetranform ntext ,DateEchance ntext , DatePaiement ntext ,Devise ntext, idClient INTEGER  REFERENCES Client(IdClient) ,Etat ntext ,modepaiement ntext,observation ntext,signature BLOB)",
                    //"Create Table Imagefact(Idimage INTEGER primary key AUTOINCREMENT,Imagevvide BLOB, IdFacture INTEGER REFERENCES Facture(idFacture) on delete cascade)",

                    "Create Table commandeFacture(Idcomfact INTEGER primary key AUTOINCREMENT, IdFacture INTEGER REFERENCES Facture(idFacture) on delete cascade, Libille ntext,TypeProd ntext , PrixHT REAL,Qte INTEGER, TVA REAL,Remise REAL)",

                    "Create Table Devis(idDevis INTEGER primary key ,DateCreation ntext,DateValidation ntext ,Datetransform ntext , Devise ntext, idClient INTEGER  REFERENCES Client(IdClient),Etat ntext, modepaiement ntext ,observation ntext,signature BLOB)",

                    "Create Table commandeDevis(idcomdev INTEGER primary key AUTOINCREMENT, IdDevis INTEGER REFERENCES Devis(idDevis)on delete cascade ,libille ntext,TypeProd ntext, Prixht REAL, Qte INTEGER, TVA REAL, Remise REAL)",
                    "Create Table Produit(idProduit INTEGER primary key AUTOINCREMENT, Nomproduit ntext ,Prix REAL,typePro ntext,tva REAL)",

                    //"Create Table FilesFact(idFailefact INTEGER primary key AUTOINCREMENT, Nomfile ntext,Pathfile ntext,IdFacture INTEGER REFERENCES Facture(idFacture) on delete cascade)",

                    //"Create Table FilesDev(idFaileDev INTEGER primary key AUTOINCREMENT, Nomfile ntext,Pathfile ntext,IdDevis INTEGER REFERENCES Devis(idFacture) on delete cascade)"


                };
                connection.Open();

                var command = connection.CreateCommand();
                for (int i = 0; i < commands.Count; i++)
                {

                    command.CommandText = commands[i];
                    command.ExecuteNonQuery();
                  //  Toast.MakeText(this, commands[i].ToString(), ToastLength.Long).Show();


                }


                connection.Close();
            }
        }
        private bool Con(string Email, string mdp)
        {
            bool check = false;
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.Parameters.Add(new SqliteParameter("@Email", Email));
            contenu.Parameters.Add(new SqliteParameter("@mdp", mdp));
            contenu.CommandText = "SELECT Emailclient ,Mdp ,role from ClientAdmin WHERE Emailclient=@Email and Mdp=@mdp and role='Admin'";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {
                if (r[0].ToString() == Email && r[1].ToString() == mdp)
                {

                    check = true;

                }



            }


            return check;

        }



        AdView ads1, ads2;
        TextView textViewMdpoblier;
        EditText Email, MDP;
        Button Creation, Connecte;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            CreationDatabase();
            string kata = "N";
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorDevis = sharedPreferences.Edit();

            base.OnCreate(savedInstanceState);

            if (kata == "C")
            {
                Intent intent2 = new Intent(Application.Context, typeof(Itemsapp));

                intent2.PutExtra("Emailadmin", selectionEmail());
                StartActivity(intent2);

            }
            if (kata == "N")
            {
                // Simulate a bit of startup work.


                

                SetContentView(Resource.Layout.PageHome);

                ads1 = FindViewById<AdView>(Resource.Id.adView1Home);
                ads2 = FindViewById<AdView>(Resource.Id.adView2Home);


                var adRequest1 = new AdRequest.Builder().Build();
                ads1.LoadAd(adRequest1);
                var adRequest2 = new AdRequest.Builder().Build();
                ads2.LoadAd(adRequest2);


                Email = FindViewById<EditText>(Resource.Id.textInputEditTextEmailHome);
                MDP = FindViewById<EditText>(Resource.Id.editTextPasseHome);

                textViewMdpoblier = FindViewById<TextView>(Resource.Id.textViewMdpoblier);

                Connecte = FindViewById<Button>(Resource.Id.buttonConnexion);

                Creation = FindViewById<Button>(Resource.Id.buttonCreationHome);


                textViewMdpoblier.Click += delegate
                {

                    Intent intent = new Intent(this, typeof(Oublier));

                    StartActivity(intent);


                };

                Creation.Click += delegate
                {

                    editorDevis.PutString("Nom", "");
                    editorDevis.PutString("Email", "");
                    editorDevis.PutString("Tel", "");
                    editorDevis.PutString("Adresse", "");
                    editorDevis.PutString("CodePostal", "");
                    editorDevis.PutString("Ville", "");
                    editorDevis.PutString("Pays", "");
                    editorDevis.Apply();
                    Intent intent = new Intent(this, typeof(CreationClient1));

                    StartActivity(intent);


                };


                Connecte.Click += delegate
                {

                    Intent intent = new Intent(this, typeof(Itemsapp));

                    if (Con(Email.Text, MDP.Text) == true)
                    {
                        Etatconx();
                        editorDevis.PutString("Emailadmin", Email.Text);
                        editorDevis.Apply();

                        intent.PutExtra("Emailadmin", Email.Text);
                        if (Etatconex() == "C")
                        {
                            kata = "C";
                        }
                        StartActivity(intent);
                    }
                    else
                    {
                        Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);

                        builder.SetTitle("Avertissement").SetMessage("votre Email ou Mot de passe incorrecte").SetPositiveButton("OK", (sender, args) => { builder.Dispose(); });
                        builder.Show();
                    }

                };

            }


            // Create your application here
        }
        private string Etatconex()
        {
            string etat = " ";
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            //   contenu.Parameters.Add(new SqliteParameter("@Email", Email));

            contenu.CommandText = "SELECT Conexteat from ClientAdmin WHERE role='Admin'";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                etat = r[0].ToString();


            }


            return etat;

        }
    }
}