using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Facturation.Class;
using Mono.Data.Sqlite;
using System;
using System.IO;

namespace Facturation
{
    [Activity(Label = "Clients", Theme = "@style/AppTheme.NoActionBar")]
    public class Clientss : AppCompatActivity
    {

        public string toast;
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            toast = string.Format("{0}", spinner.GetItemAtPosition(e.Position));

        }

        private void CreationClient(string typeclient, string nom, string tele, string email, string adresse, int codepostal, string ville, string pays, string img, string role = "user")
        {
            string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            //idClient INTEGER primary key AUTOINCREMENT, TypeClinet ntext,NomClinet ntext ,Telephone NUMERIC,Emailclient ntext,adresse ntext,CodePostal ntext, ville ntext, pays ntext,Logo text, signature text,Mdp text,confirmdp text,role text)
            contenu.CommandText = "Insert into Client(TypeClinet,NomClinet,Telephone,Emailclient,adresse,CodePostal,ville,pays,Logo)values('" + typeclient + "','" + nom + "'," + tele + ",'" + email + "','" + adresse + "'," + codepostal + ",'" + ville + "','" + pays + "','" + img + "')";

            contenu.ExecuteNonQuery();

            connection.Close();


        }

        protected override void OnCreate(Bundle savedInstanceState)
        {

            try
            {
                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.Clients);
                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));

                var spiner = FindViewById<Spinner>(Resource.Id.spinnerTypeClient);
                var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.ClientType, Android.Resource.Layout.SimpleSpinnerItem);

                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spiner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

                spiner.Adapter = adapter;


                var Nom = FindViewById<EditText>(Resource.Id.editTextNomsociete);

                var Telephone = FindViewById<EditText>(Resource.Id.editTextTelephone);

                var email = FindViewById<EditText>(Resource.Id.editTextEMail);
                var adresse = FindViewById<EditText>(Resource.Id.editTextAdresse);

                var Codepostal = FindViewById<EditText>(Resource.Id.editTextCodePostal);

                var Pays = FindViewById<EditText>(Resource.Id.textInputEditTextPays);

                var ajouter = FindViewById<Button>(Resource.Id.buttonAdd);


                var textvilleclient = FindViewById<EditText>(Resource.Id.textInputEditville);

                //btnaddlogo.Click += delegate
                //{
                //    int PickImageId = 1000;

                //    Intent = new Intent();
                //    Intent.SetType("image/*");
                //    Intent.SetAction(Intent.ActionGetContent);
                //    StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);


                //};


                ajouter.Click += delegate
                {

                    CreationClient(toast, Nom.Text, Telephone.Text, email.Text, adresse.Text, int.Parse(Codepostal.Text), textvilleclient.Text, Pays.Text, "");


                    Intent intent = new Intent(this, typeof(ListCLients));

                    StartActivity(intent);

                };


            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();


            }


        }
    }
}