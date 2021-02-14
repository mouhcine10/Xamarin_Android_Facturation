using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Facturation
{
    [Activity(Label = "Votre Informations", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class CreationClient1 : AppCompatActivity
    {
        AdView ad1, ad2;
        EditText nom;
        EditText Email;
        EditText Adresse;
        EditText tel;
        EditText CodePostal;
        EditText Ville;
        EditText Pays;
        Button btnsuivant;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();

                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.CreationClient1);
                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

                SetSupportActionBar(toolbar);
                //SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                //SupportActionBar.SetHomeButtonEnabled(true);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                toolbar.Title = "Vos Informations";
                nom = FindViewById<EditText>(Resource.Id.textInputEditTextNomCreation);
                Email = FindViewById<EditText>(Resource.Id.textInputEditTextEmailCreation);
                tel = FindViewById<EditText>(Resource.Id.editTextTelePhoneCration);
                Adresse = FindViewById<EditText>(Resource.Id.textInputEditTextAdresseCreation);
                CodePostal = FindViewById<EditText>(Resource.Id.textInputEditTextCodePostalCreation);
                Ville = FindViewById<EditText>(Resource.Id.textInputEditTextVilleCreation);
                Pays = FindViewById<EditText>(Resource.Id.textInputEditTextPaysCreation);
                btnsuivant = FindViewById<Button>(Resource.Id.buttonsuivantcreation1);

                ad1 = FindViewById<AdView>(Resource.Id.adView2Crearion);
                ad2 = FindViewById<AdView>(Resource.Id.adView3creation11);

                var adRequest1 = new AdRequest.Builder().Build();
                var adRequest2 = new AdRequest.Builder().Build();

                ad1.LoadAd(adRequest1);
                ad2.LoadAd(adRequest2);


                nom.Text = sharedPreferences.GetString("Nom", "");
                Email.Text = sharedPreferences.GetString("Email", "");
                tel.Text = sharedPreferences.GetString("Tel", "");



                Adresse.Text = sharedPreferences.GetString("Adresse", "");
                CodePostal.Text = sharedPreferences.GetString("CodePostal", "");
                Ville.Text = sharedPreferences.GetString("Ville", "");
                Pays.Text = sharedPreferences.GetString("Pays", "");
                btnsuivant.Click += delegate
                {

                    editorfacture.PutString("Nom", nom.Text);
                    editorfacture.PutString("Email", Email.Text);
                    editorfacture.PutString("Tel", tel.Text);
                    editorfacture.PutString("Adresse", Adresse.Text);
                    editorfacture.PutString("CodePostal", CodePostal.Text);
                    editorfacture.PutString("Ville", Ville.Text);
                    editorfacture.PutString("Pays", Pays.Text);
                    editorfacture.Apply();


                    Intent intent = new Intent(this, typeof(CreationClient2));

                    StartActivity(intent);






                };

                // Create your application here





            }
            catch (Exception ex)
            {

                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }
        }
      
        public override void OnBackPressed()
        {
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();


            Intent intent = new Intent(this, typeof(PageHome));
            //   intent.PutExtra("Emailadmin", Intent.GetStringExtra("Emailadmin"));
            StartActivity(intent);

            base.OnBackPressed();

        }
    }
}
