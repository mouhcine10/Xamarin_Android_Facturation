using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace Facturation.Class
{
    class HomeScreenAdapterArticleFact : BaseAdapter<Articleitem>
    {
        public readonly List<Articleitem> items;
        public readonly Activity context;
        public float Mtt;
        public HomeScreenAdapterArticleFact(Activity context, List<Articleitem> items) : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Class.Articleitem this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.tikitarticle, null);

            view.FindViewById<TextView>(Resource.Id.textViewArticletq).Text = item.NomArticle.ToString();
            view.FindViewById<TextView>(Resource.Id.textViewQteArt).Text = item.Qte.ToString()+" x";
            view.FindViewById<TextView>(Resource.Id.textViewPrixart).Text = item.Prix.ToString();
            //view.FindViewById<TextView>(Resource.Id.textViewTVA).Text = item.Tva.ToString();
            //view.FindViewById<TextView>(Resource.Id.textViewRemise).Text = item.Tauxremise.ToString();
            ////view.FindViewById<Button>(Resource.Id.button1).Click+=delegate {

            //    AlertDialog.Builder t = new AlertDialog.Builder(context);


            //};

            Mtt = item.Qte * item.Prix;
            view.FindViewById<TextView>(Resource.Id.textViewMtt).Text = "Montant: "+Mtt.ToString();

            return view;
        }
    }
}
