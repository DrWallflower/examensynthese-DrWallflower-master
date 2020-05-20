using System;
using System.Text;
using System.IO;
using System.Collections.Generic;


namespace ExamenSynthese
{
   /// <summary>
   /// Classe abstraite servant de base à tous les types de compte offerts par la banque
   /// Implémente IComparable pour permettre le tri d'une liste de comptes
   /// </summary>
   abstract class Compte : IComparable<Compte>
   {
      #region public

      /// <summary>
      /// Constructeur pour ouvrir un nouveau compte
      /// </summary>
      /// <param name="prenom">Le prénom du propriétaire</param>
      /// <param name="nom">Le nom du propriétaire</param>
      public Compte(string prenom, string nom)
      {
         _prenom = prenom;
         _nom = nom;

         // Incrémente de dernier numéro de compte utilisé et utilise la nouvelle valeur comme numéro de ce compte
         Numero = ++DernierNumero;

         // Solde initial
         Solde = 0;
      }


      /// <summary>
      /// Constructeur pour recréer un compte à partir de l'information provenant du fichier des comptes
      /// </summary>
      /// <param name="valeurs">Les valeurs lues d'une ligne du fichier</param>
      /// <exception cref="Exception">Si les données sont corrompues</exception>
      public Compte(string[] valeurs)
      {
         // valeurs[0] contient le type de compte, les données intéressantes débutens à 1
         _indexValeurs = 1;

         Numero = Convert.ToInt32(valeurs[_indexValeurs++]);  // valeurs[1] contient le numéro du compte
         _prenom = valeurs[_indexValeurs++];  // valeurs[2] contient le prenom
         _nom = valeurs[_indexValeurs++];  // valeurs[3] contient le nom

         // Solde initial
         Solde = 0;

         // Il faut mettre à jour le dernier numéro utilisé afin de continuer la numérotation des nouveaux comptes
         if (Numero > DernierNumero)
         {
            // Le nouveau numéro est plus grand que la valeur actuelle, il devient de dernier numéro
            DernierNumero = Numero;
         }
      }


      /// <summary>
      /// Le numéro unique identifiant le compte
      /// </summary>
      public int Numero { get; }

      /// <summary>
      /// Le solde du compte
      /// </summary>
      public double Solde { get; protected set; }


      /// <summary>
      /// Redéfinition de Object.ToString
      /// </summary>
      /// <returns>La représentation textuelle du compte</returns>
      public override string ToString()
      {
         StringBuilder sb = new StringBuilder();
         // TypeCompte est redéfini par tous les comptes spécialisés
         // InfoSpecialisee permet aux comptes spécialisés d'ajouter leur information
         sb.AppendFormat("{0}  {1,-8}  {2,-32} {3}", Numero, TypeCompte(), _nom.ToUpper() + ", " + _prenom, InfoSpecialisee());
         return sb.ToString();
      }


      /// <summary>
      /// Implémentation de IComparable
      /// Compare selon les noms, puis les prénoms, et finalement les numéros de compte
      /// </summary>
      /// <param name="that">Le compte à comparer avec l'objet courrant this</param>
      /// <returns>-1, 0 ou 1, selon les spécifications de IComparable</returns>
      public int CompareTo(Compte that)
      {
         // Un objet non nul est plus grand qu'un objet nul
         if (that == null)
         {
            return 1;
         }

         // Comparaison des noms
         int resultat = _nom.CompareTo(that._nom);
         if (resultat != 0)
         {
            // Les noms sont différents
            return resultat;
         }

         // Les noms sont identiques, comparaison des prénoms
         resultat = _prenom.CompareTo(that._prenom);
         if (resultat != 0)
         {
            // Les prénoms sont différents
            return resultat;
         }

         // Les prénoms sont identiques, comparaison des numéros de compte
         return Numero.CompareTo(that.Numero);
      }


      /// <summary>
      /// Retourne la liste des transactions du compte pour fins d'affichage
      /// </summary>
      /// <returns>
      /// Une liste de chaine de caractères.
      /// Chaque item de la liste correspond à la description d'une transaction.
      ///</returns>
      public List<string> ReleveDeTransactions()
      {
         List<string> liste = new List<string>();

         //
         // TODO - Compléter la méthode
         //

         return liste;
      }


      /// <summary>
      /// Effectue un dépôt dans le compte
      /// </summary>
      /// <param name="montant">Le montant à déposer</param>
      public virtual double Deposer(double montant, Transaction transaction)
      {
         // Fait simplement ajouter le montant au solde
         Solde += montant;
         _transactions.Add(transaction);
         return Solde;
      }


      /// <summary>
      /// Effectue un retrait du compte
      /// </summary>
      /// <param name="montant">Le montant à retirer</param>
      /// <exception cref="Exception">Si le solde est insufisant</exception>
      public virtual double Retirer(double montant, Transaction transaction)
      {
         if (montant <= Solde)
         {
            Solde -= montant;
            _transactions.Add(transaction);
            return Solde;
         }

         throw new Exception("solde insufisant");
      }


      /// <summary>
      /// Calcule les intérêts du compte
      /// </summary>
      /// <returns>Les intérêts calculés</returns>
      public abstract double CalculerInterets();


      /// <summary>
      /// Sauvegarde l'information du compte dans le fichier donné
      /// </summary>
      /// <param name="fichier">Le fichier dans lequel écrire l'information</param>
      public abstract void Sauvegarder(StreamWriter fichier);

      #endregion

      #region protected

      /// <summary>
      /// Indique le type spécialisé du compte
      /// </summary>
      /// <returns>Une chaine de caractère décrivant le type du compte</returns>
      protected abstract string TypeCompte();


      /// <summary>
      /// Permet aux comptes spécialisés d'afficher leur information
      /// </summary>
      /// <returns>L'information à ajouter à l'affichage du compte</returns>
      protected virtual string InfoSpecialisee()
      {
         return "";
      }


      /// <summary>
      /// Sauvegarde l'information de base commune du compte dans le fichier donné
      /// </summary>
      /// <param name="fichier">Le fichier dans lequel écrire l'information</param>
      protected void SauvegarderBase(StreamWriter fichier)
      {
         fichier.Write(";{0};{1};{2}", Numero, _prenom, _nom);
      }


      // Utilisé lors de la construction à partir des valeurs d'un fichier pour conserver l'index des valeurs lues
      protected int _indexValeurs = 0;

      // Toutes les transactions effectuées sur le compte
      protected List<Transaction> _transactions = new List<Transaction>();

      #endregion

      #region private 

      private readonly string _prenom;
      private readonly string _nom;

      // Attribut static pour conserver le dernier numéro utilisé
      // Tous les comptes utilisent et mettent à jour cette valeur
      private static int DernierNumero = 100;

      #endregion
   }
}
