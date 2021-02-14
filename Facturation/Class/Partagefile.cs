using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace Facturation.Class
{
    class Partagefile
    {
        public Java.IO.File dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/FolderDevis");


        public async void Partager(string fle)
        {
            var file = Path.Combine(FileSystem.CacheDirectory, fle);
          
            File.WriteAllText(file, "Hello World");

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "",
                File = new ShareFile(file)
            });



        }

        



    }
}