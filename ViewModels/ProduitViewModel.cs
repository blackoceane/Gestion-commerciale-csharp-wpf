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

namespace L_Joyce.ViewModels
{
    public class ProduitViewModel:BaseViewModel
    {
        
        #region Command
        public RelayCommand NextCommand { get; private set; }
        public RelayCommand FirstCommand { get; private set; }
        public RelayCommand LastCommand { get; private set; }
        public RelayCommand PreviousCommand { get; private set; }

        public RelayCommand AddPersonneCommand { get; private set; }
        public RelayCommand DeletePersonneCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }
        public RelayCommand AddEnfant { get; private set; }
        public RelayCommand DeleteEnfant { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        
        #endregion
        private DataService ds = new DataService();
        // Vue sur la collection de produits, pour la navigation et l'affichage dans la vue.
        //ICollectionView est une interface WPF qui fournit une vue sur une collection de données (par exemple, une liste de produits).
        ICollectionView produitsViewSource;

        public ICollectionView ProduitsViewSource
        {
            get => produitsViewSource;
            set
            {
                produitsViewSource = value;
                OnPropertyChanged();

            }
        }

        ICollectionView categoriesViewSource;
        public ICollectionView CategoriesViewSource
        {
            get => categoriesViewSource;
            set
            {
                categoriesViewSource = value;
                OnPropertyChanged();
            }
        }


        //Le produit actuellement sélectionné ou en cours d’édition.
        private Produits produit = new Produits();
        public Produits Produit
        {
            get { return produit; }
            set { produit = value; OnPropertyChanged(); }

        }
       
       // La liste observable des produits(source de données principale pour l’UI).
        private ObservableCollection<Produits> prdList;
        public ObservableCollection<Produits> PrdList
        {
            get { return prdList; }
            set { prdList = value; OnPropertyChanged(); }
        }
        private Produits temp;

        /*CONSTRUCTEUR QUI : -Initialise les vues (CollectionViewSource) à partir des listes du DataService.

-Initialise la liste des produits et sélectionne le premier produit par défaut.

-Initialise toutes les commandes avec les méthodes associées (ex : NextPersonne, AddP, etc.)*/
        public ProduitViewModel()
        {
            ProduitsViewSource = CollectionViewSource.GetDefaultView(ds.PrdList);
            CategoriesViewSource = CollectionViewSource.GetDefaultView(ds.Categories);

            PrdList = ds.PrdList;
            Produit = PrdList.FirstOrDefault();

            NextCommand = new RelayCommand(NextPersonne, CanBeginEdit);
            AddPersonneCommand = new RelayCommand(AddP, CanBeginEdit);
            DeletePersonneCommand = new RelayCommand(RemoveP, CanBeginEdit);
            FirstCommand = new RelayCommand(FirstP, CanBeginEdit);
            LastCommand = new RelayCommand(LastP, CanBeginEdit);
            PreviousCommand = new RelayCommand(PreviousP, CanBeginEdit);
            SaveCommand=new RelayCommand(Save, CanEndEdit);
            CancelCommand=new RelayCommand(Cancel, CanEndEdit);
            EditCommand=new RelayCommand(Edit, CanBeginEdit);  
            AddEnfant=new RelayCommand(AddEN, CanEndEdit);
            DeleteEnfant = new RelayCommand(DeleteEN, CanEndEdit);

        }
        // Méthodes pour naviguer dans la liste :

        private void PreviousP(object obj)
        {
            int index = 0;
            if (Produit != null)
            {
                index = PrdList.IndexOf(Produit);
                index--;
                if (index > PrdList.Count) { index++; }
                if (index >= 0) { Produit = PrdList[index]; }
            }
        }

        private void LastP(object obj)
        {
            if (Produit != null)
            {
                Produit = PrdList.LastOrDefault();
            }
        }

        private void FirstP(object obj)
        {
            if (Produit != null)
            {
                Produit = PrdList.FirstOrDefault();
            }
        }

        private void NextPersonne(object obj)
        {
            int index = 0;
            if (Produit != null)
            {
                index = PrdList.IndexOf(Produit);
                index++;
                if (index > PrdList.Count) { index--; }
                if (index > 0) { Produit = PrdList[index]; }
            }
        }

        //Passe en mode ajout, crée un nouveau produit, l’ajoute à la liste, puis le sélectionne.
        private void AddP(object obj)
        {
            ActionModeActuel = ACTIONMODE.ADD;
            Produit = new Produits("téléphone");
            PrdList.Add(Produit);
            //PrdList = null;
            //PrdList = tempprdList;
            Produit = PrdList.LastOrDefault();

        }
        //Cette méthode permet d’ajouter une nouvelle caractéristique à un produit sélectionné dans ton application
        private void AddEN(object obj)
        {
            // 1. Récupérer le produit actuellement sélectionné dans la vue
            Produits produit = (Produits)ProduitsViewSource.CurrentItem;

            // 2. Créer un nouvel objet caractéristique produit
            ProduitCaracteristique pc = new ProduitCaracteristique();

            // 3. Associer l'ID du produit sélectionné à la caractéristique
            pc.Id_Produit = produit.Id_Produit;

            // 4. Récupérer la première caractéristique disponible dans la liste des caractéristiques (à améliorer)
            Caracteristique caracteristique = ds.Caracteristiques.FirstOrDefault();

            // 5. Associer l'ID de la caractéristique à la nouvelle caractéristique produit
            pc.Id_Caracteristique = caracteristique.Id_Caracteristique;

            // 6. Indiquer que cette caractéristique n'est pas encore enregistrée en base (ID temporaire)
            pc.Id_ProduitCaracteristique = -1;

            // 7. Associer l'objet caractéristique complet (pour affichage ou édition)
            pc.Caracteristique = caracteristique;

            // 8. Copier la description de la caractéristique (pour affichage ou édition)
            pc.Description = caracteristique.Description;

            // 9. Ajouter cette nouvelle caractéristique à la liste des caractéristiques du produit
            produit.ProduitCaracteristiques.Add(pc);

            // 10. Notifier la vue que la liste a changé (mise à jour de l'UI)
            OnPropertyChanged("ProduitsViewSource");
            ProduitsViewSource.Refresh();
        }



        //Supprime le produit courant et sélectionne le précédent ou le premier.

        private void RemoveP(object obj)
        {
            int index = 0;
            if (Produit != null)
            {
                index = PrdList.IndexOf(Produit);
                PrdList.Remove(Produit);
                Produit = null;
                index--;
                if (index >= 0)
                { Produit = PrdList[index]; }

                else
                {
                    Produit = PrdList.FirstOrDefault();
                }
            }

            else
            { MessageBox.Show("veuillez sélectionner une personne à enlever"); }
        }
        //Supprimer une caractéristique (enfant) sélectionnée d’un produit courant.
        private void DeleteEN(object obj)
        {
            // 1. Récupérer le produit sélectionné
            Produits produit = (Produits)ProduitsViewSource.CurrentItem;
            if (produit == null)
                return;

            // 2. Vérifier que l'objet à supprimer est bien une caractéristique du produit
            var pc = obj as ProduitCaracteristique;
            if (pc == null)
                return;

            // 3. Supprimer la caractéristique de la liste
            produit.ProduitCaracteristiques.Remove(pc);

            // 4. Notifier la vue du changement
            OnPropertyChanged(nameof(ProduitsViewSource));
            ProduitsViewSource.Refresh();
        }

        //Passe en mode édition et sauvegarde l’état actuel du produit dans une variable temporaire (temp).
        private void Edit(object obj)
        {
            ActionModeActuel=ACTIONMODE.EDIT;
            temp = new Produits
            {
                Nom = produit.Nom,
                Qte_Stock = produit.Qte_Stock,
                Id_Categorie=produit.Id_Categorie,
                Id_Produit=produit.Id_Produit,
                Prix= produit.Prix
            };
    
        }
        //Sauvegarde le produit courant via le service de données, puis rafraîchit la vue.
        private void Save(object obj)
        {

            Produits prd = (Produits)ProduitsViewSource.CurrentItem;
           
                if (prd != null)
                {
                    ds.Save(prd);
                    // maj des controles autres que liste si nous modifions
                    // une valeur par code et non par saisie à l'écran
                    // simulation de clic sur un autre et revient dessus
                    ProduitsViewSource.MoveCurrentTo(null);
                    ProduitsViewSource.MoveCurrentTo(prd);
                    // maj de des listes ex lisxbox
                    ProduitsViewSource.Refresh();
                }
            

            
            ActionModeActuel =ACTIONMODE.DISPLAY;
        }
      /*  Si on était en ajout, retire le produit ajouté.

Sinon, recharge les données du produit depuis la base de données pour annuler les modifications.*/

        private void Cancel(object obj)
        {

            Produits prd = (Produits)ProduitsViewSource.CurrentItem;
           

                if (ActionModeActuel == ACTIONMODE.ADD)
                {
                    bool r = ds.PrdList.Remove(prd);

                    ProduitsViewSource.Refresh();
                }
                else
                {
                    ds.Reload(prd);

                    produitsViewSource.Refresh();
                    produitsViewSource.MoveCurrentTo(null);
                    produitsViewSource.MoveCurrentTo(prd);



                }
                ActionModeActuel = ACTIONMODE.DISPLAY;
            }
           /* if (ActionModeActuel == ACTIONMODE.ADD) {
                PrdList.Remove(produit);
            }
            else if(ActionModeActuel == ACTIONMODE.EDIT)
            {
                produit.Nom = temp.Nom;
                produit.Qte_Stock = temp.Qte_Stock;
                produit.Id_Categorie = temp.Id_Categorie;
                produit.Id_Produit = temp.Id_Produit;
                produit.Prix = temp.Prix;
            }
        }*/



    }
}
