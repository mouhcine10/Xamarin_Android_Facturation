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
    class ProduitClass
    {
        private int idproduit;

        private string type;
        private string nomproduit;

        private float prix;

        private float tva;


        public ProduitClass()
        {

        }
        public ProduitClass(int id, string type, string nomproduit, float prix)
        {
            this.Idproduit = id;
            this.Type = type;
            this.Nomproduit = nomproduit;
            this.Prix = prix;
        }
        public ProduitClass(int id, string type, string nomproduit, float prix,float tva)
        {
            this.Idproduit = id;
            this.Type = type;
            this.Nomproduit = nomproduit;
            this.Prix = prix;
            this.Tva = tva;
        }
        public ProduitClass(string type, string nomproduit, float prix)
        {

            this.Type = type;
            this.Nomproduit = nomproduit;
            this.Prix = prix;
        }

        public int Idproduit { get => idproduit; set => idproduit = value; }
        public string Type { get => type; set => type = value; }
        public string Nomproduit { get => nomproduit; set => nomproduit = value; }
        public float Prix { get => prix; set => prix = value; }
        public float Tva { get => tva; set => tva = value; }
    }
}