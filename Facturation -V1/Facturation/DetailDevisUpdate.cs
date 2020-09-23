using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Facturation.Class;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace Facturation
{
    [Activity(Label = "Article", Theme = "@style/AppTheme.NoActionBar")]
    public class DetaildevisUpdate : AppCompatActivity
    {
        private Button btnlistarticle;
        private EditText textArticle;
        private EditText textQuantite;
        private EditText textprix;
        private EditText texttaux;
        private EditText texttva;
        private TextView texttotal;

        private Spinner spiner;
        int idItemDevis = 0;
        int idDevis = 0;
        public string toast;
        static List<Articleitem> articleitems = new List<Articleitem>();

        public static float Somme;
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            toast = string.Format("{0}", spinner.GetItemAtPosition(e.Position));

        }

        private void UpdateItemDevis(string Article, float prix, int Qte, int TVA, string type, float remise, int iditemDevis)
        {

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();

            //Libille,TypeProd,PrixHT,Qte,TVA,Remise
            contenu.CommandText = "Update commandeDevis Set Libille='" + Article + "', PrixHT=" + prix + ",Qte=" + Qte + ",TVA=" + TVA + ",Remise=" + remise + ",TypeProd='" + type + "' where idcomdev=" + iditemDevis;


            contenu.ExecuteNonQuery();

            connection.Close();

        }
        private void DeleteItemDevis(int iditemDevis)
        {

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Delete from commandeDevis where  idcomdev=" + iditemDevis + "";


            contenu.ExecuteNonQuery();

            connection.Close();

        }


        private List<Articleitem> selectionCommendDevis(int idComfacture)
        {
            articleitems.Clear();
            string nom = "";
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select Libille,Qte,PrixHT,TypeProd,Remise,TVA from commandeDevis where idcomdev=" + idComfacture + "";

            var r = contenu.ExecuteReader();



            while (r.Read())
            {

                articleitems.Add(new Articleitem(r[0].ToString(), float.Parse(r[1].ToString()), float.Parse(r[2].ToString()), r[3].ToString(), float.Parse(r[4].ToString()), float.Parse(r[5].ToString())));

            }


            return articleitems;


            connection.Close();
        }


        public void InsertionArticleItem(string Article, float prix, int Qte, int TVA, string type, float remise, int idDevis)
        {

            string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);

            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = "insert into commandeDevis(Libille,TypeProd,PrixHT,Qte,TVA,Remise,IdDevis) values('" + Article + "','" + type + "'," + prix + "," + Qte + "," + TVA + ",'" + remise + "'," + idDevis + ")";

            command.ExecuteNonQuery();

            connection.Close();

            Toast.MakeText(this, command.ToString(), ToastLength.Long).Show();

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();

                idDevis = sharedPreferences.GetInt("IDDevis", 0);

                base.OnCreate(savedInstanceState);


                SetContentView(Resource.Layout.DetailFacture);

                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);


                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);

                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                SupportActionBar.Title = "Article Devis ";


                idItemDevis = sharedPreferences.GetInt("idArticle", 0);
                btnlistarticle = FindViewById<Button>(Resource.Id.buttonListArticle);
                textArticle = FindViewById<EditText>(Resource.Id.textInputEditTextArticle);
                textQuantite = FindViewById<EditText>(Resource.Id.editTextQte);
                textprix = FindViewById<EditText>(Resource.Id.editTextPrix);
                texttaux = FindViewById<EditText>(Resource.Id.editTextRemisetaux);
                texttva = FindViewById<EditText>(Resource.Id.editTextTvaFact);
                texttotal = FindViewById<TextView>(Resource.Id.textViewTotal);
                spiner = FindViewById<Spinner>(Resource.Id.spinnerTypeRemise);


                textArticle.Text = sharedPreferences.GetString("NomArticle", "");
                textQuantite.Text = sharedPreferences.GetFloat("Qte", 0).ToString();
                textprix.Text = sharedPreferences.GetFloat("Prix", 0).ToString();
                texttaux.Text = sharedPreferences.GetFloat("Tauxremise", 0).ToString();
                texttva.Text = sharedPreferences.GetFloat("TVA", 0).ToString();


                var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.Produit, Android.Resource.Layout.SimpleSpinnerItem);

                //adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                //spiner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

                //spiner.Adapter = adapter;

                var btnEnregister = FindViewById<Button>(Resource.Id.buttonEnregistrerArticle);
                btnEnregister.Visibility = Android.Views.ViewStates.Invisible;
                btnlistarticle.Click += delegate
                {

                    Intent intent = new Intent(this, typeof(ListProduits));


                    StartActivity(intent);



                };

                //btnEnregister.Click += delegate
                //{
                //    //List<Articleitem> articleitems = new List<Articleitem>();

                //    //articleitems.Add(new Articleitem(textArticle.Text, float.Parse(textQuantite.Text),float.Parse(textprix.Text)));

                //    // string Itemarticle= JsonConvert.SerializeObject(articleitems);

                //    InsertionArticleItem(textArticle.Text, float.Parse(textprix.Text), int.Parse(textQuantite.Text), int.Parse(texttva.Text), toast, float.Parse(texttaux.Text), idDevis);

                //    Intent intent = new Intent(this, typeof(Deviss));


                //    //editorfacture.PutString("Article", textArticle.Text);
                //    //editorfacture.PutFloat("Qte", float.Parse(textQuantite.Text));
                //    //editorfacture.PutFloat("Prix", float.Parse(textprix.Text));
                //    //editorfacture.Apply();
                //    //intent.PutExtra("NomArticle", textArticle.Text);
                //    //intent.PutExtra("", float.Parse(textQuantite.Text));
                //    //intent.PutExtra("prix", float.Parse(textprix.Text));
                //    //  editorfacture.PutFloat("Tauxremise", Remise);
                //    editorfacture.Apply();


                //    StartActivity(intent);

                //};

                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spiner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

                spiner.Adapter = adapter;


                //Somme = int.Parse(textQuantite.Text) * decimal.Parse(textprix.Text);

                int o = adapter.GetPosition(sharedPreferences.GetString("Tyoeremose", ""));

                spiner.SetSelection(o);


            }
            catch (System.Exception ex)
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
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();

            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
            }


            int id = item.ItemId;
            if (id == Resource.Id.Update)
            {
                UpdateItemDevis(textArticle.Text, float.Parse(textprix.Text), int.Parse(textQuantite.Text), int.Parse(texttva.Text), toast, float.Parse(texttaux.Text), sharedPreferences.GetInt("idArticle", 0));

                Intent intent = new Intent(this, typeof(UpdateDevis));
                StartActivity(intent);

            }
            if (id == Resource.Id.Delete)
            {
                DeleteItemDevis(idItemDevis);
                Toast.MakeText(this, "Your Items inovice has been deleted", ToastLength.Long).Show();
                Intent intent = new Intent(this, typeof(UpdateDevis));
                StartActivity(intent);



            }

            return base.OnOptionsItemSelected(item);
        }

    }
}