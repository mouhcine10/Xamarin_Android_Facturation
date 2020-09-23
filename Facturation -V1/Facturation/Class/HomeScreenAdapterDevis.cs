using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace Facturation.Class
{
    class HomeScreenAdapterDevis : BaseAdapter<Devis>
    {
        public readonly List<Devis> items;
        public readonly Activity context;

        public HomeScreenAdapterDevis(Activity context, List<Devis> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return position;

        }
        public override Class.Devis this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.TikitListDevise, null);

            view.FindViewById<TextView>(Resource.Id.textViewNumDevis).Text =  "Devis N°:"+item.Nemrodevis.ToString();
            view.FindViewById<TextView>(Resource.Id.textViewClient).Text ="Client : "+item.Clientnom.ToString();
            view.FindViewById<TextView>(Resource.Id.textViewMontantDevis).Text = item.Montant.ToString()+" "+item.Devise;
            view.FindViewById<TextView>(Resource.Id.textViewDatedevis).Text = item.Datefin.ToString();
            view.FindViewById<TextView>(Resource.Id.textViewEtat).Text = item.Etat;

            if (item.Etat == "Encours")
            {
                view.FindViewById<TextView>(Resource.Id.textViewEtat).SetBackgroundResource(Resource.Drawable.BtnEtat);
                view.FindViewById<TextView>(Resource.Id.textViewEtat).SetTextColor(Android.Graphics.Color.White);

            }
            if (item.Etat == "Validé")
            {
                view.FindViewById<TextView>(Resource.Id.textViewEtat).SetBackgroundResource(Resource.Drawable.BtnEtatvalide);
                view.FindViewById<TextView>(Resource.Id.textViewEtat).SetTextColor(Android.Graphics.Color.White);


            }
                
                return view;
        }



    }
}