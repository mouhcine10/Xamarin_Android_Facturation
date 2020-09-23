
using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Preferences;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;

namespace Facturation
{
    [Activity(Label = "DevisDetail", Theme = "@style/AppTheme.NoActionBar")]
    public class DevisDetail : AppCompatActivity
    {
        EditText  DatedeCreation;

        AdView Ads1, Ads2;
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
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));

                var Devisref = FindViewById<EditText>(Resource.Id.textInputEditTextREfDetaildevis);

                var DevisEntreptise = FindViewById<EditText>(Resource.Id.textInputEditTextEntrepriseDevisdetail);

                DatedeCreation = FindViewById<EditText>(Resource.Id.textInputEditTextDatecrationDevis);

                

                var dateechence = FindViewById<EditText>(Resource.Id.textInputEditTextdateEchanceDevis);

                var btnvalider = FindViewById<Button>(Resource.Id.buttonValiderDevis);


                Devisref.Text = sharedPreferencesDevisDetail.GetInt("IDDevis", 0).ToString();
                Devisref.Enabled = false;
                DevisEntreptise.Text = sharedPreferencesDevisDetail.GetString("Entreprise", "");
                DevisEntreptise.Enabled = false;
                DatedeCreation.Text = sharedPreferencesDevisDetail.GetString("DateCreation", "");

                dateechence.Text = sharedPreferencesDevisDetail.GetString("DateValidite", "");

                Ads1 = FindViewById<AdView>(Resource.Id.adView1DevisDetail);
                Ads2 = FindViewById<AdView>(Resource.Id.adView2DevisDetail);

                var adRequest1 = new AdRequest.Builder().Build();
                Ads1.LoadAd(adRequest1);
                var adRequest2 = new AdRequest.Builder().Build();
                Ads2.LoadAd(adRequest2);


                btnvalider.Click += delegate
                {

                    Intent intent = new Intent(this, typeof(Deviss));


                    editorDevis.PutInt("IDDEvis", int.Parse(Devisref.Text));
                    editorDevis.PutString("Entreprise", DevisEntreptise.Text);
                    editorDevis.PutString("DateCreation", DatedeCreation.Text);
                    editorDevis.PutString("DateValidite", dateechence.Text);
                    editorDevis.Apply();

                    StartActivity(intent);




                };

                //  var valider = FindViewById<Button>(Resource.Id.buttonvaliderDetaildevis);

            }catch(Exception ex)
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