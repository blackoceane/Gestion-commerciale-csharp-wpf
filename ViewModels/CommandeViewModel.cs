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
   public class CommandeViewModel: BaseViewModel
    {
        public RelayCommand AddPersonneCommand { get; private set; }
        public RelayCommand DeletePersonneCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }
        public RelayCommand AddEnfant { get; private set; }
        public RelayCommand DeleteEnfant { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        private DataService ds = new DataService();
      
        // Vue sur la collection de produits, pour la navigation et l'affichage dans la vue.
        //ICollectionView est une interface WPF qui fournit une vue sur une collection de données (par exemple, une liste de produits).
        ICollectionView commandesViewSource;

        public ICollectionView CommandesViewSource
        {
            get => commandesViewSource;
            set
            {
                commandesViewSource = value;
                OnPropertyChanged();

            }
        }
        ICollectionView clientsViewSource;
        public ICollectionView ClientsViewSource
        {
            get => clientsViewSource;
            set
            {
                clientsViewSource = value;
                OnPropertyChanged();
            }
        }
        ICollectionView vendeursViewSource;
        public ICollectionView VendeursViewSource
        {
            get => vendeursViewSource;
            set
            {
                vendeursViewSource = value;
                OnPropertyChanged();
            }
        }
        //La commande  actuellement sélectionné ou en cours d’édition.
        private Commande commande  = new Commande();
        public Commande Commande
        {
            get { return commande; }
            set { commande = value; OnPropertyChanged(); }

        }
        // La liste observable des commandes(source de données principale pour l’UI).
        private ObservableCollection<Commande> cmdList;
        public ObservableCollection<Commande> CmdList
        {
            get { return cmdList; }
            set { cmdList = value; OnPropertyChanged(); }
        }
        private Commande temp;
        private ComProd selectedComProd;
        public ComProd SelectedComProd
        {
            get => selectedComProd;
            set { selectedComProd = value; OnPropertyChanged(); }
        }


        /*CONSTRUCTEUR QUI : -Initialise les vues (CollectionViewSource) à partir des listes du DataService.

-Initialise la liste des commandes et sélectionne la premiere commandes par défaut.

-Initialise toutes les commandes avec les méthodes associées (ex : NextPersonne, AddP, etc.)*/
        public CommandeViewModel()
        {
            CommandesViewSource = CollectionViewSource.GetDefaultView(ds.CmdList);
            ClientsViewSource = CollectionViewSource.GetDefaultView(ds.Clients);
            VendeursViewSource = CollectionViewSource.GetDefaultView(ds.Vendeurs);

            CmdList = ds.CmdList;
            Commande = CmdList.FirstOrDefault();

          
            AddPersonneCommand = new RelayCommand(AddP, CanBeginEdit);
            DeletePersonneCommand = new RelayCommand(RemoveP, CanBeginEdit);
             SaveCommand = new RelayCommand(Save, CanEndEdit);
            CancelCommand = new RelayCommand(Cancel, CanEndEdit);
            EditCommand = new RelayCommand(Edit, CanBeginEdit);
            AddEnfant = new RelayCommand(AddEN, CanEndEdit);
            DeleteEnfant = new RelayCommand(DeleteEN, CanEndEdit);

        }
        //Passe en mode ajout, crée un nouveau produit, l’ajoute à la liste, puis le sélectionne.
        private void AddP(object obj)
        {
            ActionModeActuel = ACTIONMODE.ADD;
            Commande = new Commande();
            CmdList.Add(Commande);

            Commande = CmdList.LastOrDefault();
           
        }
        private void AddEN(object obj)
        {
            Commande commande = (Commande)CommandesViewSource.CurrentItem;

            if (commande == null)
            {
                MessageBox.Show("Aucune commande sélectionnée.");
                return;
            }

            var ajoutWindow = new AjoutEnfView(); // fenêtre WPF
            ajoutWindow.DataContext = new AjoutEnfViewModel(commande);
            ajoutWindow.ShowDialog(); // modal pour bloquer l’interaction
            
        }







       

        //Supprime la commande courante et sélectionne le précédent ou le premier.
        private void RemoveP(object obj)
        {
            // 1. Vérifier qu’une commande est sélectionnée
            if (Commande == null)
            {
                MessageBox.Show("Veuillez sélectionner une commande à enlever");
                return;
            }

            // 2. Demander confirmation
            MessageBoxResult answer = MessageBox.Show(
                "Êtes-vous sûr de vouloir supprimer cette commande ?",
                "Confirmation de suppression",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (answer != MessageBoxResult.Yes)
                return;     // Annulé par l’utilisateur

            // 3. Retirer la commande de la liste en mémoire
            int index = CmdList.IndexOf(Commande);
            Commande cmdASupprimer = Commande;          // on la garde sous la main
            CmdList.Remove(cmdASupprimer);
            
            // 4. Déterminer la nouvelle sélection
            if (CmdList.Count > 0)
            {
                Commande = index > 0 ? CmdList[index - 1] : CmdList.FirstOrDefault();
            }
            else
            {
                Commande = null;
            }

            // 5. Sauvegarder la suppression dans la BD
            
            ds.EnModeSuppression = false;   
            ds.SaveC(cmdASupprimer);       
            

            // 6. Rafraîchir la vue pour refléter la suppression
            CommandesViewSource.MoveCurrentTo(null);
            CommandesViewSource.MoveCurrentTo(Commande);
            CommandesViewSource.Refresh();
            
            // 7. Revenir au mode affichage
            ActionModeActuel = ACTIONMODE.DISPLAY;
        }


        

        //Supprimer un produit (enfant) sélectionnée d’une commande courant.
        private void DeleteEN(object obj)
        {
            ds.EnModeSuppression = false;

            // 1. Récupérer la commande sélectionné
            Commande commande = (Commande)CommandesViewSource.CurrentItem;
            if (commande == null)
            {
                MessageBox.Show("veuillez sélectionner unproduit  commande");
                return;
            }
                
            // 2. Vérifier que l'objet à supprimer est bien un produit de la commande
            var pc = obj as ComProd;
            if (pc == null)
            {
                MessageBox.Show("Commande n est pas un produit de la commande");
                return;
            }
                

            // 3. Supprimer le produit  de la liste
            commande.ComProds.Remove(pc);
            MessageBox.Show("une commande enlever");
            
            // 4. Notifier la vue du changement
            OnPropertyChanged(nameof(CommandesViewSource));
            CommandesViewSource.Refresh();
        }

        //Passe en mode édition et sauvegarde l’état actuel de la commande dans une variable temporaire (temp).
        private void Edit(object obj)
        {
            ActionModeActuel = ACTIONMODE.EDIT;
            temp = new Commande
            {
                Date_Commande = commande.Date_Commande,
                Date_Req = commande.Date_Req,
                Commentaire = commande.Commentaire,
                Id_Commande = commande.Id_Commande,
                Id_Client = commande.Id_Client,
                Id_Vendeur = commande.Id_Vendeur,
                Total_Commande = commande.Total_Commande,
                Total_Expedie = commande.Total_Expedie,
                Total_Facture = commande.Total_Facture,
                Total_Paye = commande.Total_Paye
                
        };
            
        }
        //Sauvegarde le produit courant via le service de données, puis rafraîchit la vue.
        private void Save(object obj)
        {

            Commande cmd = (Commande)CommandesViewSource.CurrentItem;

            if (cmd != null)
            {
               
               

                ds.SaveC(cmd);
                
                CommandesViewSource.MoveCurrentTo(null);
                CommandesViewSource.MoveCurrentTo(cmd);
                
                CommandesViewSource.Refresh();  
            }



            ActionModeActuel = ACTIONMODE.DISPLAY;
        }
        /*  Si on était en ajout, retire le produit ajouté.

Sinon, recharge les données du produit depuis la base de données pour annuler les modifications.*/

        private void Cancel(object obj)
        {
            Commande cmd = (Commande)CommandesViewSource.CurrentItem;
            if (cmd == null)
            {
                MessageBox.Show("Aucune commande sélectionnée.");
                return;
            }

            if (ActionModeActuel == ACTIONMODE.ADD)
            {
                // Retirer la commande de la bonne liste !
                bool r = ds.CmdList.Remove(cmd);
                CommandesViewSource.Refresh();
            }
            else
            {
                ds.ReloadC(cmd);
                CommandesViewSource.Refresh();
                CommandesViewSource.MoveCurrentTo(null);
                CommandesViewSource.MoveCurrentTo(cmd);
            }
            ActionModeActuel = ACTIONMODE.DISPLAY;
        }
        private void MettreAJourHistoriqueClient(Client client)
        {
            if (client == null) return;

            // Calcul du total des montants de commandes pour ce client
            client.HistoTotalCom = CmdList
                .Where(c => c.Id_Client == client.Id_client)
                .Sum(c => c.Total_Commande);
        }


    }
}












