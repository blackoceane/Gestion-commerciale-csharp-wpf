USE bditemcom0ss3 
GO
DROP TABLE IF EXISTS dbo.Categorie;   
 DROP TABLE IF EXISTS dbo.Client;
 DROP TABLE IF EXISTS dbo.Produits;
 Go
CREATE TABLE Client ( 
Id_client INT IDENTITY(1,1) PRIMARY KEY, 
Nom VARCHAR(55) NOT NULL, 
Telephone VARCHAR(10) NOT NULL, 
Adresse_ligne1  VARCHAR(50) NOT NULL, 
Adresse_ligne2  VARCHAR(50), 
Appartement   VARCHAR(10), 
Ville VARCHAR(100) NOT NULL, 
Code_postal  VARCHAR(10) NOT NULL, 
Province VARCHAR(3) NOT NULL, 
Remarque  VARCHAR(20), 
Solde DECIMAL(15,2) NOT NULL, 
HistoTotalCom DECIMAL(15,2) NOT NULL, 
HistoTotalExpedie DECIMAL(15,2) NOT NULL, 
HistoTotalFacture DECIMAL(15,2) NOT NULL, 
HistoTotalPaiement DECIMAL(15,2) NOT NULL, 

);
INSERT INTO dbo.Client (
    Nom, Telephone, Adresse_ligne1, Adresse_ligne2, Appartement, Ville, Code_postal, Province, Remarque,
    Solde, HistoTotalCom, HistoTotalExpedie, HistoTotalFacture, HistoTotalPaiement
) VALUES 
(
    'Jean Tremblay', '5141234567', '123 Rue Principale', NULL, NULL, 'Montréal', 'H2X1Y4', 'QC', 'Client fidèle',
    200.00, 500.00, 450.00, 480.00, 300.00
),
(
    'Sophie Gagnon', '4189876543', '456 Boulevard René-Lévesque', 'Bureau 102', NULL, 'Québec', 'G1R2K4', 'QC', 'Rien à signaler',
    150.75, 300.00, 290.00, 310.00, 250.00
),
(
    'Marc-André Lavoie', '4507654321', '789 Avenue des Pins', NULL, '2A', 'Laval', 'H7N3Y6', 'QC', 'Aucun retour',
    50.00, 120.00, 100.00, 110.00, 70.00
),
(
    'Isabelle Roy', '6131237890', '12 King Street', NULL, NULL, 'Ottawa', 'K1A0B1', 'ON', 'Nouveau client',
    0.00, 75.00, 70.00, 80.00, 75.00
),
(
    'David Nguyen', '5143216549', '1599 Chemin Sainte-Foy', NULL, NULL, 'Québec', 'G1S2L5', 'QC', 'Réduction appliquée',
    25.50, 250.00, 200.00, 240.00, 220.00
);
GO
CREATE TABLE dbo.Categorie (
  Id_Categorie  INT  IDENTITY(1,1) PRIMARY KEY,
  Nom VARCHAR(15 ) NOT NULL,
  
  );
  INSERT INTO Categorie( Nom)  VALUES ('Vetement')
  INSERT INTO Categorie( Nom)  VALUES ('Electronique')
  Go
  CREATE TABLE dbo.Produits (
    Id_Produit INT IDENTITY(1,1) PRIMARY KEY,
    Nom VARCHAR(100) NOT NULL,
    Qte_Stock INT NOT NULL,
    Id_Unité_Mesure VARCHAR(10) NOT NULL,
    Id_Categorie INT NOT NULL,
    Prix DECIMAL(10, 2) NOT NULL
);
GO

-- Insertion de 5 produits
INSERT INTO Produits (Nom, Qte_Stock, Id_Unité_Mesure, Id_Categorie, Prix) VALUES 
('Stylo', 12, 'cm', 1, 50),
('Cahier', 20, 'cm', 1, 75),
('Montre', 5, 'cm', 2, 120),
('Clé USB', 50, 'cm', 3, 30),
('Sac à dos', 10, 'cm', 2, 100);
GO
