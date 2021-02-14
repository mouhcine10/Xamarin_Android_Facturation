using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Print;
using Android.Runtime;
using Android.Telecom;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;

namespace Facturation.Class
{
    class PrintFile : PrintDocumentAdapter
    {
        Context Context;

        string path;


        public PrintFile(Context context, string path)
        {

            this.Context = context;
            this.path = path;


        }


        public override void OnLayout(PrintAttributes oldAttributes, PrintAttributes newAttributes, CancellationSignal cancellationSignal, LayoutResultCallback callback, Bundle extras)
        {
            if (cancellationSignal.IsCanceled)
            {

                callback.OnLayoutCancelled();


            }
            else
            {
                PrintDocumentInfo.Builder builder = new PrintDocumentInfo.Builder(path);

                builder.SetContentType(PrintContentType.Document).SetPageCount(PrintDocumentInfo.PageCountUnknown).Build();


                callback.OnLayoutFinished(builder.Build(), !newAttributes.Equals(oldAttributes));

            }
        }

        public override void OnWrite(PageRange[] pages, ParcelFileDescriptor destination, CancellationSignal cancellationSignal, WriteResultCallback callback)
        {
            InputStream input = null;

            OutputStream output = null;

            try
            {

                File file = new File(path);

                input = new FileInputStream(file);
                output = new FileOutputStream(destination.FileDescriptor);

                byte[] buf = new byte[8 * 1024];

                int ligne;

                while ((ligne = input.Read(buf)) >= 0 && !cancellationSignal.IsCanceled)
                output.Write(buf, 0, ligne);    
               
                if (cancellationSignal.IsCanceled)
                    callback.OnWriteCancelled();
                else
                    callback.OnWriteFinished(new PageRange[] { PageRange.AllPages });

                



            }
            catch (Exception ex)
            {

                callback.OnWriteFailed(ex.Message);

            }
            finally
            {
                try
                {
                    input.Close();

                    output.Close();

                }
                catch (IOException ex)
                {

                    Log.Error("e", "", ex.Message);

                }
            }


        }

    }
}