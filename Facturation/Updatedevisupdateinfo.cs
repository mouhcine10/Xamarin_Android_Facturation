
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Preferences;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Sql;
using System;

namespace Facturation
{
    [Activity(Label = "Devis Detail", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class Updatedevisupdateinfo : AppCompatActivity
    {
        EditText DatedeCreation;
        EditText dateechence;
        EditText Devisref;
        EditText DevisEntreptise;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {

                base.OnCreate(savedInstanceState);

                ISharedPreferences sharedPreferencesDevisDetail = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorDevis = sharedPreferencesDevisDetail.Edit();

                SetContentView(Resource.Layout.DevisDetail);

                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

                SetSupportActionBar(toolbar);
               
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));

                Devisref = FindViewById<EditText>(Resource.Id.textInputEditTextREfDetaildevis);

              
                Devisref.Enabled = true;
                
                DevisEntreptise = FindViewById<EditText>(Resource.Id.textInputEditTextEntrepriseDevisdetail);
                DevisEntreptise.Enabled = true;

                DatedeCreation = FindViewById<EditText>(Resource.Id.textInputEditTextDatecrationDevis);


                dateechence = FindViewById<EditText>(Resource.Id.textInputEditTextdateEchanceDevis);


                var btnvalider = FindViewById<Button>(Resource.Id.buttonValiderDevis);


                Devisref.Text = sharedPreferencesDevisDetail.GetString("IDdevis", " ");

                DevisEntreptise.Text = sharedPreferencesDevisDetail.GetString("Entreprise", "");

                DatedeCreation.Text = sharedPreferencesDevisDetail.GetString("DateCreation", "");

                dateechence.Text = sharedPreferencesDevisDetail.GetString("DateValidation", "");
                btnvalider.Click += delegate
                {

                    Intent intent = new Intent(this, typeof(UpdateDevis));


                    editorDevis.PutString("IDdevis", Devisref.Text);
                    editorDevis.PutString("Entreprise", DevisEntreptise.Text);
                    editorDevis.PutString("DateCreation", DatedeCreation.Text);
                    editorDevis.PutString("DateValidation", dateechence.Text);
                    editorDevis.Apply();

                    StartActivity(intent);




                };

                //  var valider = FindViewById<Button>(Resource.Id.buttonvaliderDetaildevis);

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
            return base.OnOptionsItemSelected(item); 
        }
        
    }
}