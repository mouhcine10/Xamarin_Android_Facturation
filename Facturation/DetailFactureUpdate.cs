using Android.App;
using Android.Content;
using Android.Content.PM;
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
    [Activity(Label = "ActivityDetail", Theme = "@style/AppTheme.NoActionBar",Icon ="@drawable/Noir", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class DetailFactureUpdate : AppCompatActivity
    {
        private Button btnlistarticle;
        private EditText textArticle;
        private EditText textQuantite;
        private EditText textprix;
        private EditText texttaux;
        private EditText texttva;
        private TextView texttotal;

        private Spinner spiner;
        int idItemfacture = 0;
        int idfacture = 0;
        public string toast;
        static List<Articleitem> articleitems = new List<Articleitem>();

        public static float Somme;
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            toast = string.Format("{0}", spinner.GetItemAtPosition(e.Position));

        }

        private void UpdateItemFacture(string Article, float prix, int Qte, int TVA, string type, float remise, int iditemcommande)
        {

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();

            //Libille,TypeProd,PrixHT,Qte,TVA,Remise
            contenu.CommandText = "Update commandeFacture Set Libille='" + Article + "', PrixHT=" + prix + ",Qte=" + Qte + ",TVA=" + TVA + ",Remise=" + remise + ",TypeProd='" + type + "' where Idcomfact=" + iditemcommande;


            contenu.ExecuteNonQuery();

            connection.Close();

        }
        private void DeleteItemFacture(int iditemcommande)
        {

            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Delete from commandeFacture where  Idcomfact=" + iditemcommande + "";


            contenu.ExecuteNonQuery();

            connection.Close();

        }


        private List<Articleitem> selectionCommendfacture(int idComfacture)
        {
            articleitems.Clear();
            string nom = "";
            string dbpath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select Libille,Qte,PrixHT,TypeProd,Remise,TVA from commandeFacture where Idcomfact=" + idComfacture + "";

            var r = contenu.ExecuteReader();



            while (r.Read())
            {

                articleitems.Add(new Articleitem(r[0].ToString(), float.Parse(r[1].ToString()), float.Parse(r[2].ToString()), r[3].ToString(), float.Parse(r[4].ToString()), float.Parse(r[5].ToString())));

            }


            return articleitems;


            connection.Close();
        }


        public void InsertionArticleItem(string Article, float prix, int Qte, int TVA, string type, float remise, int idfacture)
        {

            string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);

            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = "insert into commandeFacture(Libille,TypeProd,PrixHT,Qte,TVA,Remise,IdFacture) values('" + Article + "','" + type + "'," + prix + "," + Qte + "," + TVA + ",'" + remise + "'," + idfacture + ")";

            command.ExecuteNonQuery();

            connection.Close();

            //Toast.MakeText(this, command.ToString(), ToastLength.Long).Show();

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editorfacture = sharedPreferences.Edit();

                idfacture = sharedPreferences.GetInt("IDfacture", 0);

                base.OnCreate(savedInstanceState);


                SetContentView(Resource.Layout.DetailFacture);

                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);


                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);

                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                SupportActionBar.Title = "Article ";


                idItemfacture = sharedPreferences.GetInt("idArticle", 0);
                btnlistarticle = FindViewById<Button>(Resource.Id.buttonListArticle);
                textArticle = FindViewById<EditText>(Resource.Id.textInputEditTextArticle);
                textQuantite = FindViewById<EditText>(Resource.Id.editTextQte);
                textprix = FindViewById<EditText>(Resource.Id.editTextPrix);
                texttaux = FindViewById<EditText>(Resource.Id.editTextRemisetaux);
                texttva = FindViewById<EditText>(Resource.Id.editTextTvaFact);
                texttotal = FindViewById<TextView>(Resource.Id.textViewTotal);
                spiner = FindViewById<Spinner>(Resource.Id.spinnerTypeRemise);

                var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.Produit, Android.Resource.Layout.SimpleSpinnerItem);

                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spiner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

                spiner.Adapter = adapter;


                textArticle.Text = sharedPreferences.GetString("NomArticle", "");
                textQuantite.Text = sharedPreferences.GetFloat("Qte", 0).ToString();
                textprix.Text = sharedPreferences.GetFloat("Prix", 0).ToString();
                texttaux.Text = sharedPreferences.GetFloat("Tauxremise", 0).ToString();
                texttva.Text = sharedPreferences.GetFloat("TVA", 0).ToString();

                string t = sharedPreferences.GetString("Type", "");


                int o = adapter.GetPosition(t);
                spiner.SetSelection(o);

                 var btnEnregister = FindViewById<Button>(Resource.Id.buttonEnregistrerArticle);

                btnEnregister.Visibility=Android.Views.ViewStates.Invisible;
               

                btnlistarticle.Click += delegate
                {

                    Intent intent = new Intent(this, typeof(ListProduits));


                    StartActivity(intent);



                };
                float Remise = 0, r = 0;

                if (toast == "Montant")
                {

                    Remise = Somme - float.Parse(texttaux.Text);


                }
                else if (toast == "Pourcentage")
                {

                    r = (Somme * float.Parse(texttaux.Text)) / 100;

                    Remise = Somme - r;

                }
                //btnEnregister.Click += delegate
                //{
                //    //List<Articleitem> articleitems = new List<Articleitem>();

                //    //articleitems.Add(new Articleitem(textArticle.Text, float.Parse(textQuantite.Text),float.Parse(textprix.Text)));

                //    // string Itemarticle= JsonConvert.SerializeObject(articleitems);

                //    InsertionArticleItem(textArticle.Text, float.Parse(textprix.Text), int.Parse(textQuantite.Text), int.Parse(texttva.Text), toast, float.Parse(texttaux.Text), idfacture);

                //    Intent intent = new Intent(this, typeof());


                //    //editorfacture.PutString("Article", textArticle.Text);
                //    //editorfacture.PutFloat("Qte", float.Parse(textQuantite.Text));
                //    //editorfacture.PutFloat("Prix", float.Parse(textprix.Text));
                //    //editorfacture.Apply();
                //    //intent.PutExtra("NomArticle", textArticle.Text);
                //    //intent.PutExtra("", float.Parse(textQuantite.Text));
                //    //intent.PutExtra("prix", float.Parse(textprix.Text));
                //    editorfacture.PutFloat("Tauxremise", Remise);
                //    editorfacture.Apply();


                //    StartActivity(intent);

                //};

               


                //Somme = int.Parse(textQuantite.Text) * decimal.Parse(textprix.Text);




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

            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
            }


            int id = item.ItemId;
            if (id == Resource.Id.Update)
            {
                UpdateItemFacture(textArticle.Text, float.Parse(textprix.Text), int.Parse(textQuantite.Text), int.Parse(texttva.Text), toast, float.Parse(texttaux.Text), idItemfacture);

                Intent intent = new Intent(this, typeof(Updatefacture));
                StartActivity(intent);

            }
            if (id == Resource.Id.Delete)
            {
                DeleteItemFacture(idItemfacture);
                Toast.MakeText(this, "Your Items inovice has been deleted", ToastLength.Long).Show();
                Intent intent = new Intent(this, typeof(Updatefacture));
                StartActivity(intent);



            }

            return base.OnOptionsItemSelected(item);
        }

    }
}