using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace L_Joyce.Models
{
    public class Produits
    {
        public virtual ObservableCollection<ProduitCaracteristique> ProduitCaracteristiques { get; set; }

        // Valeur initiale à -1 (utile pour savoir si l'objet est nouveau ou déjà existant en BD)
        public int Id_Produit { get; set; } = -1;
       
        private string nom;
        public string Nom
        {
            get { return nom; }
            set
            {
                nom = value;
                
            }
        }

        private int qte_Stock;
        public int Qte_Stock
        {
            get { return qte_Stock; }
            set
            {
                qte_Stock = value;
               
            }
        }

        private string id_Unité_Mesure;
        public string Id_Unité_Mesure
        {
            get { return id_Unité_Mesure; }
            set
            {
                id_Unité_Mesure = value;
               
            }

        }
        //Id_Categorie est la clé étrangère 
        public int Id_Categorie { get; set; } = 1;
        
        public virtual Categorie Categorie { get; set; }

        private Nullable<decimal> prix;
        public Nullable<decimal> Prix
        {
            get { return prix; }
            set
            {
                prix = value;
               
            }
        }
        //Permet de créer facilement un nouveau produit avec des valeurs par défaut.
        public Produits(string nom = "", int stock = 12, string Mesure = "cm", int categorie = 1, int prix = 50)
        {

            this.Nom = nom;
            this.Qte_Stock = stock;
            this.Id_Unité_Mesure = Mesure;
            this.Id_Categorie = categorie;
            this.Prix = prix;
            //Initialise aussi la liste ProduitCaracteristiques
            ProduitCaracteristiques = new ObservableCollection<ProduitCaracteristique>();
        }

        //Permet d’informer l’interface graphique quand une propriété change
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));



        public override string ToString()
        {
            return $"{Nom} {Qte_Stock} {Id_Unité_Mesure}  {Id_Categorie} {Prix}";
        }

    }
}
