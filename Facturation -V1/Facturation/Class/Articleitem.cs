namespace Facturation.Class
{
    class Articleitem
    {
        //private int idcomfact;
        //private string nomArticle;

        //private float qte;

        //private float prix;

        //private string typeremise;

        //private float tauxremise;

        //private float tva;
        public Articleitem() { }
        public Articleitem(string art, float q, float p)
        {
            this.NomArticle = art;
            this.Qte = q;
            this.Prix = p;


        }
        public Articleitem(int id, string art, float q, float p, string rt, float tr, float tv)
        {
            this.Idcomfact = id;
            this.NomArticle = art;
            this.Qte = q;
            this.Prix = p;
            this.Ttyperemise = rt;
            this.Tauxremise = tr;
            this.Tva = tv;

        }
        public Articleitem(string art, float q, float p, string rt, float tr, float tv)
        {

            this.NomArticle = art;
            this.Qte = q;
            this.Prix = p;
            this.Ttyperemise = rt;
            this.Tauxremise = tr;
            this.Tva = tv;

        }

        public float calculermtt(float qt, float p)
        {


            return qt * p;

        }

        public int Idcomfact { get; }
        public string NomArticle { get; set; }

        public float Qte { get; set; }

        public float Prix { get; set; }

        public string Ttyperemise { get; set; }

        public float Tauxremise { get; set; }

        public float Tva { get; set; }


    }
}