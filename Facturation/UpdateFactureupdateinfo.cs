using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Facturation
{
    [Activity(Label = "modifier Info facture", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class UpdateFactureupdateinfo : AppCompatActivity
    {
        EditText DatedeCreation;
        EditText dateechence;
        EditText Devisref;
        EditText DevisEntreptise;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                ISharedPreferences sharedPreferencesDevisDetail = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorfacture = sharedPreferencesDevisDetail.Edit();


                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.FactDetail);
                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                Devisref = FindViewById<EditText>(Resource.Id.textInputEditTextreffac);
                Devisref.Enabled = true;
                DevisEntreptise = FindViewById<EditText>(Resource.Id.textInputEditTextNomENtrepriseDetailFacture);
                DevisEntreptise.Enabled = true;

                DatedeCreation = FindViewById<EditText>(Resource.Id.editTextCreationDate);


                dateechence = FindViewById<EditText>(Resource.Id.editTextpaiementDatefacture);
                Devisref.Text = sharedPreferencesDevisDetail.GetString("IDFacture", "");

                DevisEntreptise.Text = sharedPreferencesDevisDetail.GetString("Entreprise", "");

                DatedeCreation.Text = sharedPreferencesDevisDetail.GetString("DateFacturation", "");


                dateechence.Text = sharedPreferencesDevisDetail.GetString("DateEcheance", "");



                var btnvalider = FindViewById<Button>(Resource.Id.buttonvaliderDetail);
                btnvalider.Click += delegate
                {

                    Intent intent = new Intent(this, typeof(Updatefacture));


                    editorfacture.PutString("IDFacture", Devisref.Text);
                    editorfacture.PutString("Entreprise", DevisEntreptise.Text);
                    editorfacture.PutString("DateFacturation", DatedeCreation.Text);
                    editorfacture.PutString("DateEcheance", dateechence.Text);
                    editorfacture.Apply();

                    StartActivity(intent);




                };
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
            }
            return base.OnOptionsItemSelected(item); ;
        }
      
    }
}