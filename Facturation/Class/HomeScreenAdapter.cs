using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace Facturation.Class
{
    class HomeScreenAdapter : BaseAdapter<Class.Clients>
    {
        public readonly List<Class.Clients> items;
        public readonly Activity context;
        public HomeScreenAdapter(Activity context, List<Class.Clients> items) : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Class.Clients this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.TikitCLient, null);

            view.FindViewById<TextView>(Resource.Id.textViewNomClient).Text = item.NomClinet.ToString();
            view.FindViewById<TextView>(Resource.Id.textViewType).Text = item.TypeClinet.ToString();


            return view;
        }
    }
}