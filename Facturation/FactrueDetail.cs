using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Preferences;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;

namespace Facturation
{
    [Activity(Label = "Factrue Detail", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class FactrueDetail : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            try
            {
                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();


                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.FactDetail);

                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));

                var btnvalider = FindViewById<Button>(Resource.Id.buttonvaliderDetail);



                var textRef = FindViewById<EditText>(Resource.Id.textInputEditTextreffac);
                var textentreprise = FindViewById<EditText>(Resource.Id.textInputEditTextNomENtrepriseDetailFacture);


                var dateCreationfacture = FindViewById<EditText>(Resource.Id.editTextCreationDate);

                dateCreationfacture.Text = "";


                var datePaiementfacture = FindViewById<EditText>(Resource.Id.editTextpaiementDatefacture);

                dateCreationfacture.Text = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day.ToString();

                textRef.Text = sharedPreferences.GetInt("IDfacture", 0).ToString();
                textRef.Enabled = false;
                textentreprise.Text = sharedPreferences.GetString("Entreprise", "");
                textentreprise.Enabled = false;
                dateCreationfacture.Text = sharedPreferences.GetString("DateCreation", "");
                datePaiementfacture.Text = sharedPreferences.GetString("DateEcheance", "");

                byte[] t = Intent.GetByteArrayExtra("ImagePath");
                string ob = Intent.GetStringExtra("ob");

                btnvalider.Click += delegate
                {

                    bool info = Intent.GetBooleanExtra("info", true);




                    //   editorfacture.PutString("Entreprise", textentreprise.Text);
                    editorfacture.PutString("DateCreation", dateCreationfacture.Text);
                    editorfacture.PutString("DateEcheance", datePaiementfacture.Text);

                    editorfacture.Apply();
                    Intent intentFacture = new Intent(this, typeof(Facturee));
                    intentFacture.PutExtra("ImagePath", t);
                    intentFacture.PutExtra("ob", ob);
                    StartActivity(intentFacture);




                };



            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.menuUpdate, menu);
            return true;
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