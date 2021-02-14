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
    class Banque
    {
        private int idbanque;

        private string nombanque;

        private string bic;

        private string iban;

        public int Idbanque { get => idbanque; set => idbanque = value; }
        public string Nombanque { get => nombanque; set => nombanque = value; }
        public string Bic { get => bic; set => bic = value; }
        public string Iban { get => iban; set => iban = value; }

        public Banque(int id, string nomb, string bic, string iban)
        {
            this.Idbanque = id;
            this.Nombanque = nomb;
            this.Bic = bic;
            this.Iban = iban;



        }

        public Banque(string nomb, string bic, string iban)
        {
            this.Nombanque = nomb;
            this.Bic = bic;
            this.Iban = iban;

        }
        public Banque() { }

    }
}