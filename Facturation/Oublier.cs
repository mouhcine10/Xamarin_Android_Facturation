using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;
using Xamarin.Essentials;

namespace Facturation
{
    [Activity(Label = "Oublier", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class Oublier : Activity
    {
        AdView ads1, ads2;
        private bool checkmail(string Email)
        {
            bool ra = false;

            string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FactureDevis.db3");
            var connection = new SqliteConnection("Data Source=" + dbpath);
            connection.Open();
            var contenu = connection.CreateCommand();
            contenu.CommandText = "Select EmailClient from Client where role='Admin'";

            var r = contenu.ExecuteReader();

            while (r.Read())
            {
                if (r[0].ToString() == Email)
                {

                    ra = true;


                }



            }

            return ra;

        }

        EditText email, code;
        int a;
        Button envoyer, valide;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.oublier);

            email = FindViewById<EditText>(Resource.Id.textInputEditTextEmail);

            code = FindViewById<EditText>(Resource.Id.textInputEditTextcodevalidation);

            envoyer = FindViewById<Button>(Resource.Id.buttonEnvoyercode);

            valide = FindViewById<Button>(Resource.Id.buttonvalidercode);

            ads1 = FindViewById<AdView>(Resource.Id.adView1ob);
            ads2 = FindViewById<AdView>(Resource.Id.adView2ob);

            var adRequest1 = new AdRequest.Builder().Build();
            ads1.LoadAd(adRequest1);
            var adRequest2 = new AdRequest.Builder().Build();
            ads2.LoadAd(adRequest2);



            envoyer.Click += delegate
            {

                if (checkmail(email.Text) == true)
                {
                    Random random = new Random();

                    a = random.Next(5000000);
                    List<string> vs = new List<string>();

                    string text = "Bonjour,\n votre code de vérification Est :" + a;



                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("quickfact75@gmail.com", "QuickFact");
                    mail.To.Add(email.Text);
                    mail.Subject = " code validation ";
                    mail.Body = text;

                    SmtpServer.Port = 587;
                    SmtpServer.Host = "smtp.gmail.com";
                    SmtpServer.EnableSsl = true;
                    SmtpServer.UseDefaultCredentials = false;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("quickfact75@gmail.com", "Entreprise2020Quick");

                    SmtpServer.Send(mail);




                    Toast.MakeText(this, "Mail Send Sucessufully", ToastLength.Short).Show();


                }








            };

            valide.Click += delegate
            {

                if (a == int.Parse(code.Text))
                {









                    Intent intent = new Intent(this, typeof(Changemotdepasse));


                    StartActivity(intent);


                }
                else
                {

                    Toast.MakeText(this, "votre code non valide ", ToastLength.Long).Show();


                }
            };


            // Create your application here
        }

        public async Task SendEmail(string subject, string body, List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }

    }
}