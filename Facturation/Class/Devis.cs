using Android.Hardware.Camera2.Params;
using Java.Sql;
using System;

namespace Facturation.Class
{
    class Devis
    {
        private int nemrodevis;

        private string clientnom;

        private int idclient;

        private string etat;

        private string devise;

        private string datecreation;

        private string modepaiement;
        private string datefin;
        private string observation;
        private float montant;

        public Devis() { }

        public Devis(int n, string c, string et, string dc, string df, float mtt)
        {
            this.Nemrodevis = n;
            this.Clientnom = c;
            this.Etat = et;
            this.Datecreation = dc;
            this.Datefin = df;
            this.Montant = mtt;

        }

        public Devis(int iddev, string datecreation,string datevalise)
        {
            this.nemrodevis = iddev;
            this.datecreation = datecreation;
            this.datefin = datevalise;


        }
        public Devis(int n, string datecreation, string datevalisation, string modepaiement, string obs)
        {
            this.Nemrodevis = n;

            this.Datecreation = datecreation;
            this.Datefin = datevalisation;
            this.modepaiement = modepaiement;
            this.observation = obs;
        }

        //idDevis,Devise, idClient, modepaiement, observation

        public Devis(int id, string devise, int idclient, string modepaiement, string observatio)
        {
            this.nemrodevis = id;
            this.devise = devise;
            this.modepaiement = modepaiement;
            this.observation = observatio;
            this.idclient = idclient;




        }
        public Devis(int iddevis, string datevalidation, string nomclient, string Etat, float mtt, string devise)
        {
            this.nemrodevis = iddevis;
            this.datefin = datevalidation;
            this.clientnom = nomclient;
            this.Etat = Etat;
            this.Montant = mtt;
            this.Devise = devise;
        }
        public int Nemrodevis { get => nemrodevis; set => nemrodevis = value; }
        public string Clientnom { get => clientnom; set => clientnom = value; }
        public string Etat { get => etat; set => etat = value; }
        public string Datecreation { get => datecreation; set => datecreation = value; }
        public string Datefin { get => datefin; set => datefin = value; }
        public float Montant { get => montant; set => montant = value; }
        public string Devise { get => devise; set => devise = value; }
        public string Modepaiement { get => modepaiement; set => modepaiement = value; }
        public string Observation { get => observation; set => observation = value; }
        public int Idclient { get => idclient; set => idclient = value; }
    }
}