
using L_Joyce.Commands;
using L_Joyce.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Documents;
using L_Joyce.Views;

namespace L_Joyce.ViewModels
{
    public class AjoutEnfViewModel : BaseViewModel
    {
        public RelayCommand AddProduitCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        private DataService ds = new DataService();
        private Commande commandeParent;

        public AjoutEnfViewModel(Commande commande)
        {
            commandeParent = commande ?? throw new ArgumentNullException(nameof(commande));
            PrdList = ds.PrdList;
            ProduitsViewSource = CollectionViewSource.GetDefaultView(PrdList);

            AddProduitCommand = new RelayCommand(AddProduitToCommande, CanBeginEdit);
            CancelCommand = new RelayCommand(CloseWindow);
        }

        public ObservableCollection<Produits> PrdList { get; set; }

        public ICollectionView ProduitsViewSource { get; set; }

        private Produits produitSelectionne;
        public Produits ProduitSelectionne
        {
            get => produitSelectionne;
            set { produitSelectionne = value; OnPropertyChanged(); }
        }

        private bool CanAddProduit(object obj)
        {
            return ProduitSelectionne != null && commandeParent != null;
        }
       

       private void AddProduitToCommande(object obj)
        {
            if (ProduitSelectionne == null)
            {
                MessageBox.Show("Veuillez sélectionner un produit.");
                return;
            }
            int prochainId = commandeParent.ComProds.Any() ? commandeParent.ComProds.Max(p => p.Id_ComProd) + 1 : 0;

            var nouveauComProd = new ComProd
            {
                Id_Commande = commandeParent.Id_Commande,
                Id_Produit = ProduitSelectionne.Id_Produit,
                Produit = ProduitSelectionne,
                Nom = ProduitSelectionne.Nom,
                Prix_Vente = ProduitSelectionne.Prix,
                Qte_Com = 1,
                Qte_Exp = 0,
                Id_ComProd = prochainId
            };



           ds.SaveComProd(nouveauComProd);
          
            commandeParent.ComProds.Add(nouveauComProd);
            // CmdList.Add(Commande);


            nouveauComProd = commandeParent.ComProds.LastOrDefault();

            MessageBox.Show("Produit ajouté à la commande.");
            CloseWindow(null);
        }

        private void CloseWindow(object obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.DataContext == this)
                {
                    w.Close();
                    break;
                }
            }
        }
    }
}
