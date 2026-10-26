using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_Joyce.Models
{
    public class Client
    {
        public int Id_client { get; set; }
        public string Nom { get; set; }
        public string Telephone { get; set; }
        public string Adresse_ligne1 { get; set; }
        public string Adresse_ligne2 { get; set; }
        public string Appartement { get; set; }
        public string Ville { get; set; }
        public string Code_postal { get; set; }
        public string Province { get; set; }
        public string Remarque { get; set; }
        public decimal Solde { get; set; }
        public Nullable<decimal> HistoTotalCom { get; set; } // si plante mettre nullable<decimal>
        public Nullable<decimal> HistoTotalExpedie { get; set; }
        public Nullable<decimal> HistoTotalFacture { get; set; }
        public Nullable<decimal> HistoTotalPaiement { get; set; }
       

    }
}
