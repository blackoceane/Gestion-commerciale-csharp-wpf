USE BdH2025
GO

DROP TABLE IF EXISTS dbo.ComProd
DROP TABLE IF EXISTS dbo.Commande
DROP TABLE IF EXISTS dbo.ProduitCaracteristique
DROP TABLE IF EXISTS dbo.Produits
DROP TABLE IF EXISTS dbo.Categorie
DROP TABLE IF EXISTS dbo.Caracteristique
DROP TABLE IF EXISTS dbo.Vendeur
DROP TABLE IF EXISTS dbo.Client

GO
CREATE TABLE Vendeur
(
	Id_Vendeur INT IDENTITY(1,1)  Not Null, 
	Nom VARCHAR(15)

);
INSERT INTO Vendeur VALUES ('Yvan Destruc')
INSERT INTO Vendeur VALUES ('Yan Passe')
CREATE TABLE Client ( 
Id_client INT IDENTITY(1,1) PRIMARY KEY  Not Null,  
Nom VARCHAR(55) , 
Telephone VARCHAR(10) , 
Adresse_ligne1  VARCHAR(50) , 
Adresse_ligne2  VARCHAR(50), 
Appartement   VARCHAR(10), 
Ville VARCHAR(100) , 
CodePostal  VARCHAR(10) , 
Code_Province VARCHAR(3) , 
Remarque  VARCHAR(20), 
Solde DECIMAL(15,2) , 
HistoTotalCom DECIMAL(15,2) , 
HistoTotalExpedie DECIMAL(15,2) , 
HistoTotalFacture DECIMAL(15,2) , 
HistoTotalPaiement DECIMAL(15,2) ,
);
INSERT INTO Client(Nom,Telephone,Adresse_ligne1,Ville,CodePostal,Code_Province,Solde,HistoTotalCom,HistoTotalExpedie,HistoTotalFacture,HistoTotalPaiement) 
VALUES ('Rose Lafleur','8191234567','2 rue Fleury','Shawinigan','G9X 1Y7','QC',0,72.0,0,0,0)
INSERT INTO Client(Nom,Telephone,Adresse_ligne1,Ville,CodePostal,Code_Province,Solde,HistoTotalCom,HistoTotalExpedie,HistoTotalFacture,HistoTotalPaiement) 
VALUES ('Pierre LaRoche','8191212123','5 rue DuRocher','Grand-Mère','G8VX 2B9','QC',0,0,0,0,0)

CREATE TABLE Categorie
(
	Id_Categorie INT IDENTITY(1,1) Not Null, 
	Description VARCHAR(25)

);
INSERT INTO Categorie VALUES ('Véhicules')
INSERT INTO Categorie VALUES ('Alimentaire')
INSERT INTO Categorie VALUES ('enfant')
GO

CREATE TABLE Caracteristique
(
	Id_Caracteristique INT IDENTITY(1,1) PRIMARY KEY Not Null, 
	Description VARCHAR(25)
);

INSERT INTO Caracteristique VALUES ('légers défauts')
INSERT INTO Caracteristique VALUES ('Vente en ligne seulement')
INSERT INTO Caracteristique VALUES ('retour impossible')
GO

CREATE TABLE Produits
(
	Id_Produit INT IDENTITY(1,1) PRIMARY KEY  Not Null, 
	Nom VARCHAR(15),
	Qte_Stock INT, 
	Id_Categorie INT,
	Prix decimal,
);
INSERT INTO Produits VALUES ('vélo', 2, 1, 10.0)
INSERT INTO Produits VALUES ('pomme', 15, 2, 20.00)
INSERT INTO Produits VALUES ('poupée', 81, 3, 30.00)
GO

CREATE TABLE ProduitCaracteristique
(
	Id_ProduitCaracteristique INT IDENTITY(1,1),
	Id_Produit INT,
	Id_Caracteristique INT,

	FOREIGN KEY (Id_Produit) REFERENCES dbo.Produit(Id_Produit),
	FOREIGN KEY (Id_Caracteristique) REFERENCES dbo.Caracteristique(Id_Caracteristique) 
);
INSERT INTO ProduitCaracteristique(Id_Produit, Id_Caracteristique) VALUES (1, 1)
GO
 --INSERT INTO Produit VALUES ('poupée', 81, 3, 15.99)'15-sep-2010','Bon de commande # 2','33333',1200.00)
--ALTER TABLE BON
--  ADD CONSTRAINT pk_bonnb PRIMARY KEY CLUSTERED  (NUM_BON)
--ALTER TABLE BON
--  ADD CONSTRAINT fk_bonna FOREIGN KEY (NUM_ACH) REFERENCES ACH(NUM_ACH)

CREATE TABLE Commande ( 
Id_Commande INT IDENTITY(1,1) PRIMARY KEY, 
Date_Commande datetime,
Date_Req	 datetime,
Commentaire VARCHAR(20) , 
Id_Client int,
Id_Vendeur int,
Total_Commande  DECIMAL(15,2), 
Total_Expedie DECIMAL(15,2), 
Total_Facture DECIMAL(15,2), 
Total_Paye DECIMAL(15,2), 

);

CREATE TABLE ComProd ( 
Id_ComProd INT IDENTITY(1,1) PRIMARY KEY, 
Id_Commande INT , 
Id_Produit INT , 
Prix_Vente DECIMAL(15,2), 
Qte_Com DECIMAL(15,2), 
Qte_Exp DECIMAL(15,2), 
);
ALTER TABLE Commande
  ADD CONSTRAINT fk_CommandeCli FOREIGN KEY (Id_Client) REFERENCES Client(Id_Client)
--ALTER TABLE Commande
--  ADD CONSTRAINT fk_CommandeVen FOREIGN KEY (Id_Vendeur) REFERENCES Vendeur(Id_Vendeur)


ALTER TABLE ComProd
  ADD CONSTRAINT fk_ComProdCom FOREIGN KEY (Id_Commande) REFERENCES Commande(Id_Commande)
ALTER TABLE ComProd
  ADD CONSTRAINT fk_ComProdProd FOREIGN KEY (Id_Produit) REFERENCES Produit(Id_Produit)

   

INSERT INTO Commande 
VALUES('10-jan-2025','01-sep-2025','verif total',1,1,72.00,0.0,0.0,0.0)


INSERT INTO ComProd VALUES (1,1,10.00,3.0,0)
INSERT INTO ComProd VALUES (1,2,21.00,2.0,0)
GO