using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Facturation
{
    [Activity(Label = "Headernavbar", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class Headernavbar : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.nav_header_main);

            //var image = FindViewById<ImageView>(Resource.Id.imageViewnavheaderlogo);

            //var text = FindViewById<TextView>(Resource.Id.textViewadara);
            //var tyara = FindViewById<TextView>(Resource.Id.hajra);

            //text.Text = "mouhcine585@gmail.com";
            //tyara.Text = "MOyhciue";
            // Create your application here
        }
    }
}