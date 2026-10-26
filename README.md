# Application de gestion commerciale (C# / WPF)

Application de bureau qui gère tout le cycle des commandes clients : choix du client et du vendeur, ajout de produits, calcul des totaux et enregistrement dans une base de données relationnelle.

## Technologies

C#, .NET Framework, WPF, XAML, architecture MVVM avec un DataService, MariaDB, SQL

## Fonctionnalités

- Création, modification et suppression de commandes
- Ajout de produits depuis une liste, avec récupération automatique du prix
- Modification des quantités et des prix directement dans le tableau (DataGrid)
- Relation parent/enfant entre les commandes et les produits commandés, synchronisée avec la base de données
- Validation : une commande exige un client, un vendeur et au moins un produit

## Structure

- `Models/` : les classes de données et `DataService.cs`, qui centralise les requêtes SQL
- `ViewModels/` : la logique des écrans (MVVM)
- `Views/` : les écrans en XAML
- `Commands/` : `RelayCommand` pour relier les boutons aux ViewModels
- `scripts/` : les scripts SQL de création de la base de données

## Lancer le projet

1. Exécuter les scripts SQL du dossier `scripts` dans MariaDB pour créer la base de données.
2. Vérifier la chaîne de connexion dans `App.config`.
3. Ouvrir `L_Joyce.sln` dans Visual Studio et appuyer sur F5.

## Limites connues

- La suppression en cascade dans la base de données reste à finaliser.
- Le calcul automatique des totaux est instable au moment de la sauvegarde.

---
Projet final réalisé en mai 2025 dans le cours Développement d'applications d'entreprise I, Cégep de Shawinigan.
