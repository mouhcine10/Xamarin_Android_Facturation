
using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.Graphics.Pdf;
using Android.OS;
using Android.Print;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Facturation.Class;
using PDFViewAndroid;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Facturation
{
    [Activity(Label = "PDFlayout", Theme = "@style/AppTheme.NoActionBar")]
    public class PDFlayoutDevis : AppCompatActivity
    {
        AdView ads1, ads2;
        string a = "";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {

                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.lyoutpdf);


                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

                SetSupportActionBar(toolbar);

                ads1 = FindViewById<AdView>(Resource.Id.adView1Lyoutpdf);
                ads2 = FindViewById<AdView>(Resource.Id.adView2Lyoutpdf);
                var adRequest1 = new AdRequest.Builder().Build();
                ads1.LoadAd(adRequest1);
                var adRequest2 = new AdRequest.Builder().Build();
                ads2.LoadAd(adRequest2);

                // Back Home
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                toolbar.SetTitleTextColor(Android.Graphics.Color.Rgb(0, 250, 154));
                toolbar.SetBackgroundColor(Android.Graphics.Color.Rgb(27, 49, 71));
                SupportActionBar.Title = "PDF";

                var pdf = FindViewById<PDFView>(Resource.Id.pDFView1);

                a = Intent.GetStringExtra("a");

                Java.IO.File fileStream = new Java.IO.File(a);

                pdf.FromFile(a).Show();

            }
            catch (Exception ex)
            {

                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }

        }



        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.MenuPDF, menu);
            // menu.GetItem(2).SetEnabled(false);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
            }

            if (item.ItemId == Resource.Id.Envoyer)
            {
                //Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);

                //LayoutInflater layoutInflater = LayoutInflater.From(this);
                //View view = layoutInflater.Inflate(Resource.Layout.AUthentification, null);
                //var email = view.FindViewById<EditText>(Resource.Id.editTextEMailauth);
                //var password = view.FindViewById<EditText>(Resource.Id.editTextpassword);
                //builder.SetView(view);
                //builder.SetTitle("Information").SetMessage("Authentifier sur votre Email pour pouvoir envoyer votre Devis à vos Client ").SetPositiveButton("Yes", (senderAlert, args) =>
                //{
                   

                //}).SetNegativeButton("Cancel", (senderAlert, args) => { builder.Dispose(); });

                //builder.Show();

                string Emailentreprise, nomentreptise, Emailclient, nomclient;
                Emailclient = Intent.GetStringExtra("Emailclient");
                nomclient = Intent.GetStringExtra("Nomclient");
                nomentreptise = Intent.GetStringExtra("NomEnt");
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                string text = "Bonjour " + nomclient + " ,\n" + "Vous trouvez ci -joint le devis que vous nous avez demandé. \nJe reste sur votre disposition \n Bien cordilement ";
                mail.From = new MailAddress("quickfact75@gmail.com", nomentreptise);
                mail.To.Add(Emailclient);
                mail.Subject = "Devis de " + nomentreptise;
                mail.Body = text;
                mail.Attachments.Add(new Attachment(a));
                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("quickfact75@gmail.com", "Entreprise2020Quick");

                SmtpServer.Send(mail);



            }
            //// Specify the file to be attached and sent.
            //// This example assumes that a file named Data.xls exists in the
            //// current working directory.
            //string file = "data.xls";
            //// Create a message and set up the recipients.
            //MailMessage message = new MailMessage(
            //    "jane@contoso.com",
            //    "ben@contoso.com",
            //    "Quarterly data report.",
            //    "See the attached spreadsheet.");

            //// Create  the file attachment for this email message.
            //Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
            //// Add time stamp information for the file.
            //ContentDisposition disposition = data.ContentDisposition;
            //disposition.CreationDate = System.IO.File.GetCreationTime(file);
            //disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
            //disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
            //// Add the file attachment to this email message.
            //message.Attachments.Add(data);

            ////Send the message.
            //SmtpClient client = new SmtpClient(server);
            //// Add credentials if the SMTP server requires them.
            //client.Credentials = CredentialCache.DefaultNetworkCredentials;


            // Display the values in the ContentDisposition for the attachment.
            //ContentDisposition cd = data.ContentDisposition;
            //Console.WriteLine("Content disposition");
            //Console.WriteLine(cd.ToString());
            //Console.WriteLine("File {0}", cd.FileName);
            //Console.WriteLine("Size {0}", cd.Size);
            //Console.WriteLine("Creation {0}", cd.CreationDate);
            //Console.WriteLine("Modification {0}", cd.ModificationDate);
            //Console.WriteLine("Read {0}", cd.ReadDate);
            //Console.WriteLine("Inline {0}", cd.Inline);
            //Console.WriteLine("Parameters: {0}", cd.Parameters.Count);
            //foreach (DictionaryEntry d in cd.Parameters)
            //{
            //    Console.WriteLine("{0} = {1}", d.Key, d.Value);
            //}
            //data.Dispose();

            if (item.ItemId == Resource.Id.Imprimer10)
            {


                try
                {
                    PrintManager printManager = (PrintManager)GetSystemService(Context.PrintService);

                    PrintFile printFile = new PrintFile(this, a);

                    printManager.Print("Doc", printFile, new PrintAttributes.Builder().Build());

                }
                catch (Exception ex)
                {

                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

                }
            }




            return base.OnOptionsItemSelected(item);
        }
    }
}