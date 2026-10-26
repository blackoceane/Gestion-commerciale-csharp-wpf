using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_Joyce.Models
{
    public class ProduitCaracteristique 
    {
        public int Id_ProduitCaracteristique { get; set; }
        public int Id_Produit { get; set; }
        public int Id_Caracteristique { get; set; }

        public string Description { get; set; }

        // a loader dans chaussure dataservice// pour afficher description dans le datagrid et non dans un combobox

        public virtual Caracteristique Caracteristique { get; set; }
    }
}
