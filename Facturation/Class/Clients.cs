using Android.Util;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.IO;

namespace Facturation.Class
{
    class Clients
    {
        public int Idclient { get; set; }

        public string TypeClinet { get; set; }

        public string NomClinet { get; set; }
        public string Email { get; set; }

        public string Telephone { get; set; }

        public string Adresse { get; set; }

        public string Codepostal { get; set; }

        public string Ville { get; set; }

        public string Pays { get; set; }

        public byte[] Logo { get; set; }

        //public Clients(string email, string nom, string tele, string adresse, string codepostal, string villa, string pays)
        //{
        //    this.Email = email;
        //    this.NomClinet = nom;
        //    this.Telephone = tele;
        //    this.Adresse = adresse;
        //    this.Codepostal = codepostal;
        //    this.Ville = villa;
        //    this.Pays = pays;




        //}
        //Logo,NomClinet,adresse, CodePostal, ville ,pays,Emailclient,Telephone

        public Clients(int id, string nom)
        {
            this.Idclient = id;
            this.NomClinet = nom;

        }

        public Clients(byte[] logo, string nomclient, string adresse, string codepostal, string ville, string pays, string email, string telephone)
        {
            this.Logo = logo;
            this.Email = email;
            this.NomClinet = nomclient;
            this.Telephone = telephone;
            this.Adresse = adresse;
            this.Codepostal = codepostal;
            this.Ville = ville;
            this.Pays = pays;




        }
        public Clients(string nomclien, string adres, string codeposta, string vill, string pay, string email, string telephone)
        {
            this.Codepostal = codeposta;
            this.Ville = vill;
            this.Pays = pay;
            this.Email = email;
            this.NomClinet = nomclien;
            this.Telephone = telephone;
            this.Adresse = adres;


        }

        public Clients(string nom, string email, byte[] logo)
        {

            this.NomClinet = nom;
            this.Email = email;
            this.Logo = logo;

        }

        public Clients()
        {



        }
        public Clients(string n)
        {
            this.NomClinet = n;


        }
        public Clients(int id, string t, string n)
        {
            this.Idclient = id;
            this.TypeClinet = t;
            this.NomClinet = n;


        }


        public Clients(int id)
        {

            this.Idclient = id;

        }

        public Clients(string t, string n)
        {
            this.TypeClinet = t;
            this.NomClinet = n;


        }


    }


}