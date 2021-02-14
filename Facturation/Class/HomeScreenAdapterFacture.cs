using Android.App;
using Android.Views;
using Android.Widget;
using Java.Sql;
using System;
using System.Collections.Generic;

namespace Facturation.Class
{

    class HomeScreenAdapterFacture : BaseAdapter<Facture>
    {
        public readonly List<Facture> items;
        public readonly Activity context;


        public HomeScreenAdapterFacture(Activity context, List<Facture> items) : base()
        {
            this.items = items;
            this.context = context;


        }
        public override long GetItemId(int position)
        {
            return position;

        }
        public override Class.Facture this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.TikitListFacture, null);

            view.FindViewById<TextView>(Resource.Id.textViewIDfacture).Text = "Facture N°: "+item.Numerofacture.ToString();
            //SELECT * FROM Facture INNER JOIN commandeFacture on Facture.idFacture=commandeFacture.Idcomfact INNER JOIN Client on Client.idClient=Facture.idClient WHERE Facture.idFacture=2 
            view.FindViewById<TextView>(Resource.Id.textViewCLientfacture).Text = "Client: "+item.Clientname;
            view.FindViewById<TextView>(Resource.Id.textViewmttfacture).Text = ""+item.Montant.ToString() +" "+ item.Devise;
            view.FindViewById<TextView>(Resource.Id.textViewDateFacture).Text = item.Dateechence.ToString();
            view.FindViewById<TextView>(Resource.Id.textViewEtatFacture).SetTextColor(Android.Graphics.Color.White);
            view.FindViewById<TextView>(Resource.Id.textViewEtatFacture).Text = item.Etat;
            if (item.Etat == "Non Payée")
            {
                view.FindViewById<TextView>(Resource.Id.textViewEtatFacture).SetBackgroundResource(Resource.Drawable.BtnEtatRetard) ;
                view.FindViewById<TextView>(Resource.Id.textViewEtatFacture).SetTextColor(Android.Graphics.Color.White);
                             
            
            }
            if (item.Etat == "Payée")
            {
                view.FindViewById<TextView>(Resource.Id.textViewEtatFacture).SetBackgroundResource(Resource.Drawable.BtnEtatvalide);
                view.FindViewById<TextView>(Resource.Id.textViewEtatFacture).SetTextColor(Android.Graphics.Color.White);
                view.FindViewById<TextView>(Resource.Id.textViewEtatFacture).Text=item.Etat+" le: "+item.Datepaiement;

            }
            //DateTime dateec = DateTime.Parse(item.Dateechence);

            //if (dateec.Day>DateTime.Now.Day&& dateec.Month>DateTime.Now.Month&& dateec.Year>DateTime.Now.Year)
            //{
            //    item.Etat = "Enrtard";

            //    view.FindViewById<TextView>(Resource.Id.textViewEtatFacture).Text = item.Etat;
            //    view.FindViewById<TextView>(Resource.Id.textViewEtatFacture).SetBackgroundResource(Resource.Drawable.BtnEtatRetard);
            //    view.FindViewById<TextView>(Resource.Id.textViewEtatFacture).SetTextColor(Android.Graphics.Color.White);



            //}

            return view;
        }




    }
}