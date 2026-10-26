using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace L_Joyce.Models
{
    public class Commande
    {
        public int Id_Commande { get; set; } = 1;
        private DateTime? date_Commande;
        public DateTime? Date_Commande
        {
            
            get {return  date_Commande; }
            set
            {
                date_Commande = value;
                
            }
        }

        private DateTime? date_Req;
        public DateTime? Date_Req
        {
            get {  return date_Req; }
            
            set
            {
                date_Req = value;
                
            }
        }
        private string commentaire;
        public string Commentaire
        {
            get { return commentaire; }
            
            set
            {
                commentaire = value;
                
            }
        }
        public int Id_Client { get; set; } = 1;
        public int Id_Vendeur { get; set; } = 1;

        public virtual Client Client { get; set; }
        public virtual Vendeur Vendeur { get; set; }

        private decimal? total_Commande;
        public decimal? Total_Commande
        {
            get { return  total_Commande; }
            
            set
            {
                total_Commande = value;
                
            }
        }


        private decimal? total_Expedie;
        public decimal? Total_Expedie
        {
            get { return total_Expedie; }
            set { total_Expedie = value; }
        }

        private decimal? total_Facture;
        public decimal? Total_Facture
        {
            get { return total_Facture; }
            set { total_Facture = value; }
        }


        private decimal? total_Paye;
        public decimal? Total_Paye
        {
            get { return total_Paye; }
            
            set
            {
                total_Paye = value;
                
            }
        }

        public virtual ObservableCollection<ComProd> ComProds { get; set; } = new ObservableCollection<ComProd>();
        public List<ComProd> ComProdsToDelete { get; set; } = new List<ComProd>();

        
       
        public void Calcul_Total_Commande()
        {
            Total_Commande = (decimal)ComProds.Sum(cp => cp.Qte_Com * cp.Prix_Vente);

        }

        
        public Commande( int idClient = 1, int idVendeur = 1, string commentaire = "")
        {
            this.Date_Commande = DateTime.Now;
            this.Date_Req = DateTime.Now.AddDays(10);
          this.Id_Client = idClient;
           this.Id_Vendeur = idVendeur;
            this.Commentaire = commentaire;
            this.Total_Commande =0 ;
            this.Total_Expedie = 0;
            this.Total_Facture = 0;
            this.Total_Paye = 0;
            //Initialise aussi la liste ComProds et ComProdsToDelete 
            ComProds = new ObservableCollection<ComProd>();
            ComProdsToDelete = new List<ComProd>();
        }

        //Permet d’informer l’interface graphique quand une propriété change
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));




    }
}
