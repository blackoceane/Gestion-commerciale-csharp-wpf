using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_Joyce.Models
{
   
    public class ComProd
    {
        public int Id_ComProd { get; set; }//PK 
        public int Id_Commande { get; set; } //fk 
        public int Id_Produit { get; set; } //FK 
        public string Nom { get; set; }
        public Nullable<decimal> Qte_Com { get; set; }
        public Nullable<decimal> Qte_Exp { get; set; }
        public Nullable<decimal> Prix_Vente { get; set; }
        virtual public Produits Produit { get; set; }// travail pour afficher decription dans le 
                                                    //datagrid 
    }

}
