using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.OleDb;
using Microsoft.VisualBasic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Automation;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Runtime.InteropServices;

namespace L_Joyce.Models
{

    //Une classe DataService dans une application WPF MVVM sert à isoler et centraliser la logique d’accès aux données, permettant ainsi au ViewModel de se concentrer sur la logique de présentation, tout en facilitant la maintenance, l’évolution et les tests de l’application
    // elle  évite la duplication du code d’accès aux données dans les Modelsà


    public class DataService
    {
        private ObservableCollection<Produits> prdList;
        // toujours une propriété public get set avec notification
        // LA connectionString
        // string connectionString = @"Data Source =Environment.Machine.Name\sqlexpress; Initial Catalog = demoA7B; Integrated Security = True";
        string nomserveur = "LAPTOP-0LB0U5KN'\'SQLEXPRESS";
         //string connectionString = @"Data Source = LAPTOP-0LB0U5KN\SQLEXPRESS; Initial Catalog = bditemcom0ss3; Integrated Security = True";
        string connectionString1 = @"Data Source = LAPTOP-0LB0U5KN\SQLEXPRESS; Initial Catalog = BdH2025; Integrated Security = True";

        public bool EnModeSuppression { get; set; } = false;

        public ObservableCollection<Produits> PrdList
        {
            get => prdList;
            set
            {
                prdList = value;
                //OnPropertyChanged();
            }
        }
        private ObservableCollection<Commande> cmdList;
        public ObservableCollection<Commande> CmdList
        {
            get => cmdList;
            set => cmdList = value;
        }
      

        private ObservableCollection<Client> clients;
        public ObservableCollection<Client> Clients
        {
            get => clients;
            set => clients = value;
        }

        private ObservableCollection<Vendeur> vendeurs;
        public ObservableCollection<Vendeur> Vendeurs
        {
            get => vendeurs;
            set => vendeurs = value;
        }

        private ObservableCollection<Categorie> categories;
        // toujours une propriété public get set avec notification
        public ObservableCollection<Categorie> Categories
        {
            get => categories;
            set
            {
                categories = value;


            }
        }
        private ObservableCollection<Caracteristique> caracteristiques;
        public ObservableCollection<Caracteristique> Caracteristiques
        {
            get => caracteristiques;
            set => caracteristiques = value;
        }
        public ObservableCollection<ProduitCaracteristique> produitCaracteristiques;

        public ObservableCollection<ComProd> comProds;
        //Le service utilise un pattern singleton pour garantir qu'une seule instance de DataService existe
        private static DataService instance = null;
        // propriété en lecture seule get
        public static DataService Instance()
        {
            if (instance == null)
            { instance = new DataService(); }

            return instance;
        }
        public DataService()
        {
            LoadAll();

        }
       
        public void LoadAll()
        {
          //  LoadFromBdItemcom();
            LoadFromBdH2025();

           
        }
      /*  private void LoadFromBdItemcom()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                LoadCategories(connection);
               
                LoadProduits(connection);
            }
        }*/
       
        private void LoadFromBdH2025()
        {
            using (SqlConnection connection = new SqlConnection(connectionString1))
            {
                //Ouvre une connexion à la base de données.
                connection.Open();
                //* MessageBox.Show("Connexion ouverte à : " + connection.Database); // Doit afficher BdH2025*/
              //  Charge les catégories, produits , caractéristiques clientset commandes.
                LoadCategories(connection);
                LoadProduits(connection);
                LoadClients(connection);
                LoadVendeurs(connection);
                LoadCaracteristiques(connection);
                LoadCommandes(connection);
               

                // Charger les caractéristiques des produits maintenant que la base est correcte
                foreach (var p in PrdList)
                {
                    LoadProduitCaracteristiques(connection, p);
                }
                foreach (var c in cmdList)
                {
                    LoadComProds(connection, c);
                }
                //faire la meme chose pour charger es commandes des produits 
            }
        }


       //Récupère tous les clients depuis la table Client et les ajoute à la collection Clients.


        private void LoadClients(SqlConnection connection)
        {
            Clients = new ObservableCollection<Client>();
            string queryString = "SELECT * FROM dbo.Client";
            SqlCommand command = new SqlCommand(queryString, connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Client client = new Client
                    {
                        Id_client = (int)reader["Id_client"],
                        Nom = (string)reader["Nom"],
                        Telephone = (string)reader["Telephone"],
                        Adresse_ligne1 = (string)reader["Adresse_ligne1"],
                        Adresse_ligne2 = reader["Adresse_ligne2"] as string,
                        Appartement = reader["Appartement"] as string,
                        Ville = (string)reader["Ville"],
                        Code_postal = (string)reader["CodePostal"],
                        Province = (string)reader["Code_Province"],
                        Remarque = reader["Remarque"] as string,
                        Solde = (decimal)reader["Solde"],
                        HistoTotalCom = (decimal)reader["HistoTotalCom"],
                        HistoTotalExpedie = (decimal)reader["HistoTotalExpedie"],
                        HistoTotalFacture = (decimal)reader["HistoTotalFacture"],
                        HistoTotalPaiement = (decimal)reader["HistoTotalPaiement"]
                    };

                    Clients.Add(client);
                }
            }
        }
        private void LoadVendeurs(SqlConnection connection)
        {
            Vendeurs = new ObservableCollection<Vendeur>();
            string queryString = "SELECT * FROM dbo.Vendeur";
            SqlCommand command = new SqlCommand(queryString, connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Vendeur vendeur = new Vendeur
                    {
                        Id_vendeur = (int)reader["Id_Vendeur"],
                        Nom = (string)reader["Nom"]
                    };

                    Vendeurs.Add(vendeur);
                }
            }
        }

        //Récupère tous les produits depuis la table Produit,
        //crée un objet Produits pour chaque ligne,
        //et lie chaque produit à sa catégorie correspondante.
        private void LoadProduits(SqlConnection connection)
        {

            PrdList = new ObservableCollection<Produits>();

            string queryString = "SELECT * FROM dbo.Produit";
            SqlCommand command = new SqlCommand(queryString, connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Produits produit = new Produits();
                    ReaderToOneProduit(reader, produit);
                    produit.Categorie = Categories.FirstOrDefault(c => c.Id_Categorie == produit.Id_Categorie);

                    PrdList.Add(produit);
                }
            }

          
        }

        private void ReaderToOneProduit(SqlDataReader reader, Produits produit)
        {
            produit.Id_Produit = (Int32)reader["Id_Produit"];
            produit.Nom= (string)reader["Nom"];
            produit.Qte_Stock= (Int32)reader["Qte_Stock"];
            produit.Id_Categorie= (Int32)reader["Id_Categorie"];
           /* produit.Prix= (Int32)reader["Prix"];*/
            produit.Prix = Convert.ToDecimal(reader["Prix"]);
        }

        private void LoadCommandes(SqlConnection connection)
        {

            CmdList = new ObservableCollection<Commande>();

            string queryString = "SELECT * FROM dbo.Commande";
            SqlCommand command = new SqlCommand(queryString, connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                   Commande commande = new Commande();
                    ReaderToOneCommande(reader, commande);
                   
                    if (Vendeurs == null)
                    {
                        throw new Exception("La liste Vendeurs est null.");
                    }

                    commande.Client = Clients.FirstOrDefault(c => c.Id_client == commande.Id_Client);
                    commande.Vendeur = Vendeurs.FirstOrDefault(c => c.Id_vendeur == commande.Id_Vendeur);
                    CmdList.Add(commande);
                }
            }


        }

        private void ReaderToOneCommande(SqlDataReader reader, Commande commande)
        {
            commande.Id_Commande = (Int32)reader["Id_Commande"];
            commande.Date_Commande = (DateTime)reader["Date_Commande"];
            commande.Date_Req = (DateTime)reader["Date_Req"];
            commande.Commentaire = (string)reader["Commentaire"];
             commande.Id_Client= (Int32)reader["Id_Client"];
            commande.Id_Vendeur = (Int32)reader["Id_Vendeur"];
            commande.Total_Commande = Convert.ToDecimal(reader["Total_Commande"]);
            commande.Total_Expedie = Convert.ToDecimal(reader["Total_Expedie"]);
            commande.Total_Facture = Convert.ToDecimal(reader["Total_Facture"]);
            commande.Total_Paye = Convert.ToDecimal(reader["Total_Paye"]);
        }
        //Pour un produit donné, récupère toutes ses caractéristiques depuis la table ProduitCaracteristique,
        //les instancie et les ajoute à la collection du produit.
        private void LoadProduitCaracteristiques(SqlConnection connection, Produits p)
        {
            // Efface les caractéristiques existantes du produit (pour le rechargement)
            p.ProduitCaracteristiques.Clear();
            // Requête SQL pour récupérer les caractéristiques du produit
            string queryString = "SELECT * FROM dbo.ProduitCaracteristique WHERE Id_Produit = @Id_Produit";
            
            // Crée la commande SQL
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@Id_Produit", p.Id_Produit);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ProduitCaracteristique pc = new ProduitCaracteristique();

                    if (p.ProduitCaracteristiques == null)
                p.ProduitCaracteristiques = new ObservableCollection<ProduitCaracteristique>();

                    ReaderToOneProduitCaracteristique(reader, pc);

                    pc.Caracteristique = Caracteristiques.FirstOrDefault(c => c.Id_Caracteristique == pc.Id_Caracteristique);

                    p.ProduitCaracteristiques.Add(pc);
                    pc.Caracteristique = Caracteristiques.FirstOrDefault(c => c.Id_Caracteristique == pc.Id_Caracteristique);
                }
            }

          
        }

        
        private void ReaderToOneProduitCaracteristique(SqlDataReader reader, ProduitCaracteristique pc)
        {
            pc.Id_Produit = (int)reader["Id_Produit"];
            pc.Id_Caracteristique = (int)reader["Id_Caracteristique"];
           
        }
        private void LoadComProds(SqlConnection connection, Commande c)
        {
            // Efface les caractéristiques existantes du produit (pour le rechargement)
            c.ComProds.Clear();
            // Requête SQL pour récupérer les caractéristiques du produit
            string queryString = "SELECT * FROM dbo.ComProd WHERE Id_Commande = @Id_Commande";

            // Crée la commande SQL
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@Id_Commande", c.Id_Commande);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ComProd pc = new ComProd();

                    if (c.ComProds == null)
                        c.ComProds = new ObservableCollection<ComProd>();

                    ReaderToOneComProds(reader, pc);

                    pc.Produit = PrdList.FirstOrDefault(p => p.Id_Produit == pc.Id_Produit);

                    c.ComProds.Add(pc);
                    pc.Produit = PrdList.FirstOrDefault(p => p.Id_Produit == pc.Id_Produit);
                }
            }


        }


        private void ReaderToOneComProds(SqlDataReader reader, ComProd pc)
        {
            pc.Id_Commande = (int)reader["Id_Commande"];
            pc.Id_Produit = (int)reader["Id_Produit"];

        }

        //Récupère toutes les catégories depuis la table Categorie et les ajoute à la collection Categories.
        private void LoadCategories(SqlConnection connection)
        {
            Categories = new ObservableCollection<Categorie>();
            string queryString = "SELECT * FROM dbo.Categorie"; // Requête SQL pour récupérer toutes les catégories
            SqlCommand command = new SqlCommand(queryString, connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Categorie categorie = new Categorie();
                    // Mapper les colonnes de la base de données aux propriétés de l'objet Categorie
                    categorie.Id_Categorie = (int)reader["Id_Categorie"];
                    categorie.Nom = (string)reader["Description"];

                    Categories.Add(categorie);
                }
            }
        }



        //Récupère toutes les caractéristiques depuis la table Caracteristique et les ajoute à la collection Caracteristiques.
        private void LoadCaracteristiques(SqlConnection connection)
        {
            Caracteristiques = new ObservableCollection<Caracteristique>();
            string query = "SELECT * FROM dbo.Caracteristique";
            SqlCommand cmd = new SqlCommand(query, connection);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Caracteristique carac = new Caracteristique();

                    carac.Id_Caracteristique = (int)reader["Id_Caracteristique"];
                    carac.Description = (string)reader["Description"];
                    
                    Caracteristiques.Add(carac);
                }
            }
        }
        //Si le produit n'existe pas encore (Id_Produit == -1),
        //il est inséré avec InsertSansProcS().
        //Sinon, il est mis à jour avec Update().
        public void Save(Produits produits)
        {// a mettre dans le viewmodel
         // insert ou update?????


            if (produits.Id_Produit == -1)
            {
                InsertSansProcS(produits);
                // Insert( personne);
            }
            else
            {
                Update(produits);
            }


        }
        public void SaveC(Commande commande)

        {
            if (EnModeSuppression==true)
            {
                EnModeSuppression = false; // on réinitialise
                commande.Calcul_Total_Commande();
                UpdateCommande(commande);
                return;
            }


            // 1. Validation du client
            if (commande.Id_Client == 0)
            {
                MessageBox.Show("Entrez un client valide");
                return;
            }
            // 2. Validation du vendeur
            if (commande.Id_Vendeur == 0)
            {
                MessageBox.Show("Entrez un vendeur valide");
                return;
            }
            // 3. Validation des dates
            if (commande.Date_Req < commande.Date_Commande)
            {
                MessageBox.Show("La date requise doit être supérieure ou égale à la date de commande");
                return;
            }
            // 4. Validation des produits
            if (commande.ComProds == null || commande.ComProds.Count <= 0)
            {
                MessageBox.Show("Ajoutez au moins un produit à la commande");
                MessageBox.Show($"Nombre de produits dans la commande : {commande.ComProds.Count}");

                return;
            }

            // 5. Calcul du total
            commande.Calcul_Total_Commande();

            // 6. Insertion ou mise à jour
            if (commande.Id_Commande == 1)

            {

                MessageBox.Show("insertCommande");
                InsertCommande(commande);
            }
            else
            {

                MessageBox.Show($"updateCommande  {commande.Id_Commande} ");
                
                UpdateCommande(commande);
            }
        }

        //Insère un nouveau produit dans la base de données, puis insère ses caractéristiques associées
        private void InsertSansProcS(Produits p)
        {
            using (SqlConnection connection = new SqlConnection())
            {

                connection.ConnectionString = connectionString1;
                connection.Open();

                string queryString = "INSERT INTO dbo.Produit ( Nom, Qte_Stock,Id_Categorie,Prix)" +
                    "VALUES(@Nom , @Qte_Stock, @Id_Categorie,@Prix)";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@Nom", p.Nom);
                    command.Parameters.AddWithValue("@Qte_Stock", p.Qte_Stock);
                    command.Parameters.AddWithValue("@Id_Categorie", p.Id_Categorie);
                    command.Parameters.AddWithValue("@Prix", p.Prix);
                    command.ExecuteNonQuery();
                    // non!!!! auto command.Parameters.AddWithValue("@Id_Personne", p.Id_Personne);
                    command.ExecuteNonQuery(); // result :nb lignes affectée
                }
                queryString = "SELECT IDENT_CURRENT('Produit')";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    p.Id_Produit = Convert.ToInt32(command.ExecuteScalar());//////
                }
                // save enfants
                foreach (var perch in p.ProduitCaracteristiques)
                {
                    perch.Id_ProduitCaracteristique = p.Id_Produit; ///*****
                    InsertProduitCaracteristiquesPrd(connection, p, perch);

                }
            }
        }
        //Met à jour un produit existant dans la base de données.
        public void Update(Produits p)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString1;
                connection.Open();

                string queryString = "UPDATE dbo.Produit" +
               " SET Nom = @Nom," +
                  "Qte_Stock = @Qte_Stock," +
                  "Id_Categorie = @Id_Categorie," +
                  "Prix = @Prix" +
             "  WHERE Id_Produit=@Id_Produit";

                SqlCommand command = new SqlCommand(queryString, connection);
                //  moins compliqué que le tableau mais je voulais vous montrer les tableaux aussi
                command.Parameters.AddWithValue("@Nom", p.Nom);
                command.Parameters.AddWithValue("@Qte_Stock", p.Qte_Stock);
                command.Parameters.AddWithValue("@Id_Categorie", p.Id_Categorie);
                command.Parameters.AddWithValue("@Prix", p.Prix);
                command.Parameters.AddWithValue("@Id_Produit", p.Id_Produit);
                command.ExecuteNonQuery();
            }
        }

        private void UpdateCommande(Commande c)
        {
            using (SqlConnection connection = new SqlConnection(connectionString1))
            {
                connection.Open();

                // Mise à jour de la commande principale
                string queryString = @"UPDATE dbo.Commande
                               SET Date_Commande = @Date_Commande,
                                   Date_Req = @Date_Req,
                                   Commentaire = @Commentaire,
                                   Id_Client = @Id_Client,
                                   Id_Vendeur = @Id_Vendeur,
                                   Total_Commande = @Total_Commande,
                                   Total_Expedie = @Total_Expedie,
                                   Total_Facture = @Total_Facture,
                                   Total_Paye = @Total_Paye
                               WHERE Id_Commande = @Id_Commande";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@Id_Client", c.Id_Client);
                    command.Parameters.AddWithValue("@Id_Vendeur", c.Id_Vendeur);
                    command.Parameters.AddWithValue("@Date_Commande", c.Date_Commande);
                    command.Parameters.AddWithValue("@Date_Req", c.Date_Req);
                    command.Parameters.AddWithValue("@Commentaire", c.Commentaire);
                    command.Parameters.AddWithValue("@Total_Expedie", c.Total_Expedie);
                    command.Parameters.AddWithValue("@Total_Facture", c.Total_Facture);
                    command.Parameters.AddWithValue("@Total_Paye", c.Total_Paye);
                    command.Parameters.AddWithValue("@Total_Commande", c.Total_Commande);
                    command.Parameters.AddWithValue("@Id_Commande", c.Id_Commande);
                    command.ExecuteNonQuery();
                }

                // Ici tu peux gérer la mise à jour des lignes de commande
                // (par exemple, supprimer puis réinsérer, ou faire un update intelligent)
            }
        }

        // Sert à insérer les caractéristiques d’un produit, mais le code semble dupliqué ou incorrect 
        private void InsertProduitCaracteristiquesPrd(SqlConnection connection, Produits p, ProduitCaracteristique pc)
        {
            string queryString = "INSERT INTO dbo.ProduitCaracteristique (Id_Produit, Id_Caracteristique) VALUES (@Id_Produit, @Id_Caracteristique)";
            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@Id_Produit", p.Id_Produit);
                command.Parameters.AddWithValue("@Id_Caracteristique", pc.Id_Caracteristique);
                command.ExecuteNonQuery();
            }

           
        }
        //Mettre à jour un objet Produits (produit) en rechargeant ses données depuis la base de données, y compris ses caractéristiques associées.
        public void Reload(Produits p)
        {
            // vider les enfants
            string queryString = "SELECT * FROM dbo.Produit " +
                                "WHERE(Produit.Id_Produit = @Id_Produit)";

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString1;



                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);

                command.Parameters.AddWithValue("@Id_Produit", p.Id_Produit);
                //command.ExecuteNonQuery();


                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {

                        ReaderToOneProduit(reader, p);
                        p.Categorie = Categories.FirstOrDefault(pr => pr.Id_Categorie == p.Id_Categorie);
                        //p.ProduitCaracteristiques = ProduitCaracteristiques.FirstOrDefault(p => p.Id_ProduitCarac == p.Id_Profession);


                    }
                }   // fin using reader

                // load les  objets 1-1
                // load les enfants s'il y a lieu
                // ici
                LoadProduitCaracteristiques(connection, p);
            }

        }

        public void ReloadC(Commande c)
        {
            string queryString = "SELECT * FROM dbo.Commande WHERE Id_Commande = @Id_Commande";

            using (SqlConnection connection = new SqlConnection(connectionString1))
            {
                connection.Open();

                // 1. Recharger les propriétés principales de la commande
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@Id_Commande", c.Id_Commande);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ReaderToOneCommande(reader, c);
                        // Recharger les objets liés (client, vendeur)
                        c.Client = Clients.FirstOrDefault(cl => cl.Id_client == c.Id_Client);
                        c.Vendeur = Vendeurs.FirstOrDefault(vd => vd.Id_vendeur == c.Id_Vendeur);
                    }
                }

                // 2. Recharger les enfants (lignes de commande)
                LoadComProds(connection, c);
            }
        }

      


        private void InsertCommande(Commande c)
        {
            using (SqlConnection connection = new SqlConnection(connectionString1))
            {
                connection.Open();

                // Insertion de la commande principale
                string queryString = @"INSERT INTO dbo.Commande (Date_Commande, Date_Req,Commentaire,Id_Client, Id_Vendeur,  Total_Commande,Total_Expedie,Total_Facture,Total_Paye)
                               VALUES ( @Date_Commande, @Date_Req,@Commentaire, @Id_Client, @Id_Vendeur,@Total_Commande,@Total_Expedie,@Total_Facture,@Total_Paye)";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@Id_Client", c.Id_Client);
                    command.Parameters.AddWithValue("@Id_Vendeur", c.Id_Vendeur);
                    command.Parameters.AddWithValue("@Date_Commande", c.Date_Commande);
                    command.Parameters.AddWithValue("@Date_Req", c.Date_Req);
                    command.Parameters.AddWithValue("@Total_Commande", c.Total_Commande);
                    command.Parameters.AddWithValue("@Commentaire", c.Commentaire);
                    command.Parameters.AddWithValue("@Total_Expedie", c.Total_Expedie);
                    command.Parameters.AddWithValue("@Total_Facture", c.Total_Facture);
                    command.Parameters.AddWithValue("@Total_Paye", c.Total_Paye);
                    command.ExecuteNonQuery();
                }

                // Récupérer l'ID généré de la commande
                string getIdQuery = "SELECT IDENT_CURRENT('Commande')";
                using (SqlCommand command = new SqlCommand(getIdQuery, connection))
                {
                    c.Id_Commande = Convert.ToInt32(command.ExecuteScalar());
                }

                // Insertion des lignes de produits si elles existent
                if (c.ComProds != null && c.ComProds.Count > 0)
                {
                    foreach (var ligne in c.ComProds)
                    {
                        InsertLigneCommande(connection, c, ligne);
                    }
                }
                else
                {
                    // Aucun produit associé à cette commande — rien à faire ici
                }
            }

           
        }

        private void InsertLigneCommande(SqlConnection connection, Commande c, ComProd cp)
        {
            string queryString = @"INSERT INTO dbo.ComProd (Id_Commande, Id_Produit, Prix_Vente , Qte_Com, Qte_Exp)
                           VALUES (@Id_Commande, @Id_Produit, @Prix_Vente ,@Qte_Com ,@Qte_Exp)";
            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@Id_Commande", c.Id_Commande);
                command.Parameters.AddWithValue("@Id_Produit", cp.Id_Produit);
                command.Parameters.AddWithValue("@Qte_Com", cp.Qte_Com);
                
                command.Parameters.AddWithValue("@Prix_Vente", cp.Prix_Vente);
                command.Parameters.AddWithValue("@Qte_Exp", cp.Qte_Exp);
                command.ExecuteNonQuery();
            }
        }

      

        public void SaveComProd(ComProd comProd)
        {
            using (SqlConnection conn = new SqlConnection(connectionString1))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO dbo.ComProd (Id_Commande, Id_Produit, Prix_Vente, Qte_Com, Qte_Exp )
            VALUES (@Id_Commande, @Id_Produit, @Prix_Vente , @Qte_Com, @Qte_Exp)", conn);

                cmd.Parameters.AddWithValue("@Id_Commande", comProd.Id_Commande);
                cmd.Parameters.AddWithValue("@Id_Produit", comProd.Id_Produit);
                cmd.Parameters.AddWithValue("@Qte_Com", comProd.Qte_Com);
                cmd.Parameters.AddWithValue("@Qte_Exp", comProd.Qte_Exp);
                cmd.Parameters.AddWithValue("@Prix_Vente", comProd.Prix_Vente);

                cmd.ExecuteNonQuery();
            }
        }
       
       



    }
}
