using L_Joyce.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace L_Joyce.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // Cette propriété représente le ViewModel actuellement actif, donc la vue affichée dans l'application.
        //Quand elle change, l'interface graphique est mise à jour pour refléter le nouveau contenu.
        private BaseViewModel currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                currentViewModel = value;
                OnPropertyChanged();
            }

        }
        //Ces commandes sont reliées à des boutons dans la Vue.
        //Elles utilisent la classe RelayCommand, qui permet d'exécuter une méthode quand le bouton est cliqué.
        public RelayCommand CmdAcceuil { get; private set; }
        public RelayCommand CmdCommande { get; private set; }
        public RelayCommand CmdProduit { get; private set; }
        public RelayCommand CmdClient { get; private set; }

        public RelayCommand CmdAjout { get; private set; }
        public MainViewModel()
        {
            CurrentViewModel = new AcceuilViewModel();//// vue par défaut au lancement
            //On lie chaque commande à une méthode de navigation
            CmdAcceuil = new RelayCommand(GotoAcceuil, null);
            CmdProduit = new RelayCommand(GotoProduit, null);
            CmdClient=new RelayCommand(GotoClient,null);
            CmdCommande=new RelayCommand(GotoCommand,null);
            
        }
        //Ces méthodes changent simplement le ViewModel actif
        //Quand CurrentViewModel change, la Vue réagit (grâce à OnPropertyChanged)
        private void GotoClient(object obj)
        {
            CurrentViewModel=new ClientViewModel();
        }

        private void GotoProduit(object obj)
        {
            CurrentViewModel = new ProduitViewModel();

        }
        private void GotoCommand(object obj)
        {
            CurrentViewModel = new CommandeViewModel();
        }
     

        private void GotoAcceuil(object obj)
        {
            CurrentViewModel = new AcceuilViewModel();
            
        }
      
       

    }
}
