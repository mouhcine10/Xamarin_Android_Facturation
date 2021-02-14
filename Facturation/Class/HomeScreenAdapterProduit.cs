using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Facturation.Class
{
    class HomeScreenAdapterProduit:BaseAdapter<ProduitClass>
    {
        public readonly List<ProduitClass> items;
        public readonly Activity context;

        public HomeScreenAdapterProduit(Activity context, List<ProduitClass> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return position;

        }
        public override Class.ProduitClass this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.tikitProduit, null);

            view.FindViewById<TextView>(Resource.Id.textViewTypeproduit).Text =item.Type.ToString();
            view.FindViewById<TextView>(Resource.Id.textViewNomProduit).Text = item.Nomproduit.ToString();
            view.FindViewById<TextView>(Resource.Id.textViewPrixProduit).Text =item.Prix.ToString();
        
           

            return view;
        }

    }
}