namespace Facturation.Class
{
    class Facture
    {
        private int numerofacture;

        private int idclient;

        private string clientname;

        private float montant;

        private string datefacturation;

        private string datepaiement;
      
        private string modepaiement;

        private string dateechence;

        private string observation;

        private string devise;
        private string etat;

        public Facture() { }
        public Facture(int n, string datefacturation, string dateechence)
        {
            this.Numerofacture = n;
            this.Datefacturation = datefacturation;
            this.Dateechence = dateechence;


        }
        public Facture(int n, string cl, float m, string dp, string e, int idclient)
        {
            this.idclient = idclient;
            this.numerofacture = n;
            this.clientname = cl;
            this.montant = m;
            this.datepaiement = dp;
            this.etat = e;

        }
        public Facture(int n, string cl, float m, string datefacturation, string dateechence, string e, string devise)
        {
            this.Numerofacture = n;
            this.clientname = cl;
            this.Datefacturation = datefacturation;
            this.Dateechence = dateechence;

            this.Montant = m;
            this.Devise = devise;
            this.Etat = e;

        }
        public Facture(int n, string datecreation, string datevalisation, string modepaiement, string obs)
        {
            this.Numerofacture = n;

            this.datefacturation = datecreation;
            this.Dateechence = datevalisation;
            this.Modepaiement = modepaiement;
            this.observation= obs;
        }
        public Facture(int n, string cl, float m, string datefacturation, string dateechence, string e, string devise,string dp)
        {
            this.Numerofacture = n;
            this.clientname = cl;
            this.Datefacturation = datefacturation;
            this.Dateechence = dateechence;
            this.datepaiement = dp;
            this.Montant = m;
            this.Devise = devise;
            this.Etat = e;

        }
        //public Facture(int n, string cl, float m, string datefacturation, string dateechence, string e, string devise, string dp)
        //{
        //    this.Numerofacture = n;
        //    this.clientname = cl;
        //    this.Datefacturation = datefacturation;
        //    this.Dateechence = dateechence;
        //    this.Datepaiement = dp;
        //    this.Montant = m;
        //    this.Devise = devise;
        //    this.Etat = e;

        //}
        public int Numerofacture { get => numerofacture; set => numerofacture = value; }
        public string Clientname { get => clientname; set => clientname = value; }
        public float Montant { get => montant; set => montant = value; }
        public string Datepaiement { get => datepaiement; set => datepaiement = value; }
        public string Etat { get => etat; set => etat = value; }
        public int Idclient { get => idclient; set => idclient = value; }
        public string Datefacturation { get => datefacturation; set => datefacturation = value; }
        public string Dateechence { get => dateechence; set => dateechence = value; }
        public string Devise { get => devise; set => devise = value; }
        public string Modepaiement { get => modepaiement; set => modepaiement = value; }
        public string Observation { get => observation; set => observation = value; }
    }
}