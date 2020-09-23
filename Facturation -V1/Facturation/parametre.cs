using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Facturation.Class;
using Mono.Data.Sqlite;

namespace Facturation
{
    [Activity(Label = "parametre", Theme = "@style/AppTheme.NoActionBar")]
    public class parametre : AppCompatActivity
    {
        LinearLayout infoclient;
        LinearLayout devise;
        LinearLayout changemdp;
        LinearLayout donneesbancaire;
        List<Banque> banques = new List<Banque>();
        AdView ads1, ads2,ads3;
        
        
        private string selectionEmail(string role)
        {
            string email = "";

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();

             contenu.Parameters.Add(new SqliteParameter("@role",role));
            contenu.CommandText = "SELECT Emailclient from ClientAdmin WHERE role=@role";


            var r = contenu.ExecuteReader();

            while (r.Read())
            {

                email = r[0].ToString();
               
            }

            connection.Close();

            return email;




        }
        
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

                banques.Add(new Banque(int.Parse(r[0].ToString()),r[1].ToString(), r[2].ToString(), r[3].ToString()));

            }

            connection.Close();

            return banques;
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
            }
            return base.OnOptionsItemSelected(item); ;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorDevis = sharedPreferences.Edit();


                base.OnCreate(savedInstanceState);

               
                SetContentView(Resource.Layout.Parametre);

                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);


                SetSupportActionBar(toolbar);
                //SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                //SupportActionBar.SetHomeButtonEnabled(true);

                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                SupportActionBar.Title = "Paramétre";
                // Create your application here

                infoclient = FindViewById<LinearLayout>(Resource.Id.linearLayoutupdateClient);
                devise = FindViewById<LinearLayout>(Resource.Id.linearLayoutchangeDevise);
                changemdp = FindViewById<LinearLayout>(Resource.Id.linearLayoutchangemdp);
                donneesbancaire = FindViewById<LinearLayout>(Resource.Id.linearLayoutBanque);

                ads1 = FindViewById<AdView>(Resource.Id.adView1par);
                ads2 = FindViewById<AdView>(Resource.Id.adView2par);
                ads3 = FindViewById<AdView>(Resource.Id.adView2par);
                var adRequest1 = new AdRequest.Builder().Build();
                ads1.LoadAd(adRequest1);
                var adRequest2 = new AdRequest.Builder().Build();
                ads2.LoadAd(adRequest2);

                var adRequest3 = new AdRequest.Builder().Build();
                ads3.LoadAd(adRequest3);


                infoclient.Click += delegate
                {
                    editorDevis.PutString("Emailadmin", selectionEmail("Admin"));

                    editorDevis.Apply();
                    
                    Intent intent = new Intent(this, typeof(UpdateAdminInfo));
                    intent.PutExtra("Emailadmin", selectionEmail("Admin"));

                    StartActivity(intent);


                };

                devise.Click += delegate {

                    Intent intent = new Intent(this, typeof(Updatedevise));

                    editorDevis.PutString("Emailadmin", selectionEmail("Admin"));

                    editorDevis.Apply();
                    intent.PutExtra("Emailadmin", selectionEmail("Admin"));

                    StartActivity(intent);




                };

                changemdp.Click += delegate {

                    Intent intent = new Intent(this, typeof(Updatepassword));

                    editorDevis.PutString("Emailadmin", selectionEmail("Admin"));
                    
                    editorDevis.Apply();
                    intent.PutExtra("Emailadmin", selectionEmail("Admin"));

                    StartActivity(intent);

                              
                
                };

                donneesbancaire.Click += delegate
                {

                    editorDevis.PutString("Emailadmin", selectionEmail("Admin"));

                   
                    editorDevis.PutInt("ID",Infobanck()[0].Idbanque) ;
                    editorDevis.PutString("NomBanque",Infobanck()[0].Nombanque) ;
                    editorDevis.PutString("Bic",Infobanck()[0].Bic) ;
                    editorDevis.PutString("Iban",Infobanck()[0].Iban) ;
                    editorDevis.Apply();

                    Intent intent = new Intent(this, typeof(UpdateInfoBanque));

                    intent.PutExtra("Emailadmin", selectionEmail("Admin"));

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