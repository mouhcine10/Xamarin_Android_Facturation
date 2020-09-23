using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;

namespace Facturation
{
    [Activity(Label = "QuickFact",MainLauncher=true,Icon="@drawable/Noir")]
    public class Lancer : AppCompatActivity
    {
        readonly string[] permissionGroupe = { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
        const int Requestid = 0;
        string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");

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
                    Toast.MakeText(this, commands[i].ToString(), ToastLength.Long).Show();


                }


                connection.Close();
            }
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
        private int selectioncount()
        {
            int email = 0;

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();

            contenu.CommandText = "SELECT count(*) from ClientAdmin";


            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                email = int.Parse(r[0].ToString());

            }

            connection.Close();

            return email;




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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Lancer);


        }
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() =>
            {
             
                SimulateStartup();


            });
            //if (CheckLocationPermission())
            //{CreationDatabase();

            startupWork.Start();
            //}
        }
        async void SimulateStartup()
        {

           
            Intent intent1 = new Intent(Application.Context, typeof(PageHome));
            await Task.Delay(2000);
            StartActivity(intent1);

        }
        //else
        //{








        //    if (Etatconex() == "C")
        //    {
        //        Intent intent2 = new Intent(Application.Context, typeof(Itemsapp));

        //        // intent.PutExtra("Emailadmin", );
        //        StartActivity(intent2);

        //    }
        //    if (Etatconex() == "N")
        //    {
        //        // Simulate a bit of startup work.

        //        Intent intent3 = new Intent(Application.Context, typeof(PageHome));

        //        //intent.PutExtra("Emailadmin", selectionEmail());
        //        StartActivity(intent3);

        //    }
        //    await Task.Delay(2000);

        //}
    }


}
