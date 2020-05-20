using System;
using System.Collections.Generic;
using System.IO;


namespace ExamenSynthese
{
   /// <summary>
   /// Représentaion de la banque.
   /// Contient tous les comptes créés
   /// </summary>
   class Banque
   {
      #region public

      /// <summary>
      /// Liste des identificateurs de tous les types de compte disponibles
      /// </summary>
      public static readonly List<string> IdentificateursDeComptes = new List<string> { CompteCheques.IdentificateurType, CompteEpargne.IdentificateurType, CompteCredit.IdentificateurType };


      /// <summary>
      /// Constructeur
      /// </summary>
      public Banque()
      {
         ChargerComptes();
         ChargerTransactions();
      }


      /// <summary>
      /// Ouvre un nouveau compte
      /// </summary>
      /// <param name="type">Le type de compte à ouvrir</param>
      /// <param name="prenom">Le prénom du propriétaire</param>
      /// <param name="nom">Le nom du propriétaire</param>
      /// <param name="montantInitial">Le montant initial déposé dans le compte</param>
      /// <returns>Le numéro du compte créé</returns>
      public int AjouterCompte(string type, string prenom, string nom, double montantInitial)
      {
         Compte nouveauCompte;
         switch (type)
         {
            case CompteCheques.IdentificateurType:  nouveauCompte = new CompteCheques(prenom, nom);  break;
            case CompteEpargne.IdentificateurType:  nouveauCompte = new CompteEpargne(prenom, nom);  break;
            // La limite de crédit est générée aléatoirement en multiple de 100$ entre 500$ et 3000$
            case CompteCredit.IdentificateurType:   nouveauCompte = new CompteCredit(prenom, nom, _generateurAleatoire.Next(5, 31) * 100);  break;
            default: throw new Exception("Type de compte invalide");
         }

         _comptes.Add(nouveauCompte);
         Sauvegarder(nouveauCompte);

         // Si un montant initial est donné, on effectue une transaction de dépôt dans le compte
         if (montantInitial > 0)
         {
            Deposer(nouveauCompte, montantInitial);
         }

         return nouveauCompte.Numero;
      }


      /// <summary>
      /// Retourne la liste de tous les comptes pour fins d'affichage
      /// </summary>
      /// <returns>
      /// Une liste de chaine de caractères.
      /// Chaque item de la liste correspond à la description d'un compte.
      /// Les comtes sont triés premièrement par nom, puis par prénom, et finalement par numéro.
      ///</returns>
      public List<string> ListeDeComptes()
      {
         // Les comptes vont se trier correctement
         _comptes.Sort();

         List<string> liste = new List<string>();
         foreach (var compte in _comptes)
         {
            liste.Add(compte.ToString());
         }

         return liste;
      }


      /// <summary>
      /// Vérifie que le compte portant le numéro donné existe
      /// </summary>
      /// <param name="numeroCompte">Le numéro de compte à valider</param>
      /// <exception cref="ArgumentException">Si le compte n'existe pas</exception>
      public void ValiderExistence(int numeroCompte)
      {
         if (IndexCompte(numeroCompte) == -1)
         {
            throw new ArgumentException();
         }
      }


      /// <summary>
      /// Indique le sole du compte donné
      /// </summary>
      /// <param name="numeroCompte">Le numéro du compte duquel indiquer le solde</param>
      /// <returns>Le sole de compte donné</returns>
      /// <exception cref="ArgumentException">Si le compte n'existe pas</exception>
      public double Solde(int numeroCompte)
      {
         return TrouverCompte(numeroCompte).Solde;
      }


      /// <summary>
      /// Retourne la liste des transactions d'un compte pour fins d'affichage
      /// </summary>
      /// <param name="numeroCompte">Le numéro du compte duquel lister les transactions</param>
      /// <returns>
      /// Une liste de chaine de caractères.
      /// Chaque item de la liste correspond à la description d'une transaction.
      ///</returns>
      /// <exception cref="ArgumentException">Si le compte n'existe pas</exception>
      public List<string> ReleveDeTransactions(int numeroCompte)
      {
         return TrouverCompte(numeroCompte).ReleveDeTransactions();
      }


      /// <summary>
      /// Effectue un dépôt du montant donné dans le compte portant le numéro donné
      /// </summary>
      /// <param name="numeroCompte">Le numéro du compte dans lequel effectuer le dépôt</param>
      /// <param name="montant">Le montant à déposer</param>
      /// <returns>Le nouveau solde du compte après le dépôt</returns>
      /// <exception cref="ArgumentException">Si le compte n'existe pas, ou si le montant est négatif ou 0</exception>
      public double Deposer(int numeroCompte, double montant)
      {
         Compte compte = TrouverCompte(numeroCompte);
         // Appelle la version privé pour effectuer le dépôt
         Deposer(compte, montant);
         return compte.Solde;
      }


      /// <summary>
      /// Effectue un retrait du montant donné du le compte donné
      /// </summary>
      /// <param name="numeroCompte">Le numéro du compte dans lequel effectuer le retrait</param>
      /// <param name="montant">Le montant à retirer</param>
      /// <returns>Le nouveau solde du compte après le retrait</returns>
      /// <exception cref="ArgumentException">Si le compte n'existe pas, ou si le montant est négatif ou 0</exception>
      public double Retirer(int numeroCompte, double montant)
      {
         Compte compte = TrouverCompte(numeroCompte);
         Transaction t = new Retrait(compte, montant);
         t.Effectuer();
         Sauvegarder(t);
         return compte.Solde;
      }


      /// <summary>
      /// Calcule les intérêts du compte donné
      /// </summary>
      /// <param name="numeroCompte">Le numéro du compte duquel calculter les intérêts</param>
      /// <returns>Les intérêts du compte</returns>
      /// <exception cref="ArgumentException">Si le compte n'existe pas</exception>
      public double CalculerInterets(int numeroCompte)
      {
         Compte compte = TrouverCompte(numeroCompte);
         return compte.CalculerInterets();
      }

      #endregion

      #region private

      /// <summary>
      /// Trouve et retourne le compte portant le numéro donné
      /// </summary>
      /// <param name="numeroCompte">Le numéro de compte à trouver</param>
      /// <returns>Le compte portant le numéro donné</returns>
      /// <exception cref="ArgumentException">Si le compte n'existe pas</exception>
      private Compte TrouverCompte(int numeroCompte)
      {
         int index = IndexCompte(numeroCompte);
         if (index != -1)
         {
            return _comptes[index];
         }
         throw new ArgumentException();
      }


      /// <summary>
      /// Retourne l'index du compte portant le numéro donné dans la liste des comptes
      /// </summary>
      /// <param name="numeroCompte">Le numéro de compte à trouver</param>
      /// <returns>L'index du compte dans la liste des comptes, ou -1 si le compte est introuvable</returns>
      private int IndexCompte(int numeroCompte)
      {
         for (int i = 0; i < _comptes.Count; ++i)
         {
            if (_comptes[i].Numero == numeroCompte)
            {
               return i;
            }
         }

         return -1;
      }


      /// <summary>
      /// Lit le fichier des comptes et le charge en mémoire
      /// </summary>
      private void ChargerComptes()
      {
         try
         {
            using (StreamReader fichier = new StreamReader(NomFichierComptes))
            {
               string ligne = fichier.ReadLine();
               while (ligne != null)
               {
                  try
                  {
                     string[] valeurs = ligne.Split(';');

                     Compte nouveauCompte;
                     switch (valeurs[0])  // valeurs[0] contient le type du compte
                     {
                        case CompteCheques.IdentificateurType:  nouveauCompte = new CompteCheques(valeurs);  break;
                        case CompteEpargne.IdentificateurType:  nouveauCompte = new CompteEpargne(valeurs);  break;
                        case CompteCredit.IdentificateurType:   nouveauCompte = new CompteCredit(valeurs);   break;
                        default: throw new Exception();
                     }

                     // Doit vérifier qu'un compte portant le même numéro n'existe pas déjà
                     if (IndexCompte(nouveauCompte.Numero) == -1)
                     {
                        // Le compte n'existe pas, il peut être ajouté à la liste
                        _comptes.Add(nouveauCompte);
                     }
                  }
                  catch { }  // Ignore silencieusement les erreurs et passe à la prochaine ligne

                  ligne = fichier.ReadLine();
               }
            }
         }
         catch { }  // Rien à faire si le fichier n'existe pas
      }


      /// <summary>
      /// Sauvegarde un compte dans le fichier des comptes
      /// </summary>
      /// <param name="compte">Le compte à sauvegarder</param>
      private void Sauvegarder(Compte compte)
      {
         using (StreamWriter fichier = new StreamWriter(NomFichierComptes, true /* On ajoute à la fin du fichier */))
         {
            compte.Sauvegarder(fichier);
         }
      }


      /// <summary>
      /// Lit le fichier des transactions et les effectue sur les comptes
      /// </summary>
      private void ChargerTransactions()
      {
         try
         {
            using (StreamReader fichier = new StreamReader(NomFichierTransactions))
            {
               string ligne = fichier.ReadLine();
               while (ligne != null)
               {
                  try
                  {
                     string[] valeurs = ligne.Split(';');

                     int numeroCompte = Convert.ToInt32(valeurs[0]); // valeurs[0] contient le numéro du compte
                     Compte compte = TrouverCompte(numeroCompte);

                     Transaction t;
                     switch (valeurs[1]) // valeurs[1] contient le type de la transaction
                     {
                        case Depot.IdentificateurType:    t = new Depot(compte, valeurs);    break;
                        case Retrait.IdentificateurType:  t = new Retrait(compte, valeurs);  break;
                        default: throw new Exception();
                     }

                     t.Effectuer();
                  }
                  catch { }  // Ignore silencieusement les erreurs et passe à la prochaine ligne

                  ligne = fichier.ReadLine();
               }
            }
         }
         catch { }  // Rien à faire si le fichier n'existe pas
      }


      /// <summary>
      /// Sauvegarde une transaction dans le fichier des transactions
      /// </summary>
      /// <param name="compte">La transaction à sauvegarder</param>
      private void Sauvegarder(Transaction transaction)
      {
         using (StreamWriter fichier = new StreamWriter(NomFichierTransactions, true /* On ajoute à la fin du fichier */))
         {
            transaction.Sauvegarder(fichier);
         }
      }


      /// <summary>
      /// Effectue un dépôt du montant donné dans le compte donné
      /// </summary>
      /// <param name="compte">Le compte dans lequel effectuer le dépôt</param>
      /// <param name="montant">Le montant à déposer</param>
      /// <exception cref="ArgumentException">Si le montant est négatif ou 0</exception>
      public void Deposer(Compte compte, double montant)
      {
         Transaction t = new Depot(compte, montant);
         t.Effectuer();
         Sauvegarder(t);
      }



      // Pour générer la limite de crédit
      private static Random _generateurAleatoire = new Random();

      private const string NomFichierComptes = "comptes.txt";
      private const string NomFichierTransactions = "transactions.txt";

      // Tous les comptes de la banque
      private List<Compte> _comptes = new List<Compte>();

      #endregion
   }
}
