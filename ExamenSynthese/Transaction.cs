using System;
using System.IO;


namespace ExamenSynthese
{
   /// <summary>
   /// Classe abstraite servant de base à tous les types de transactions possibles sur un compte
   /// </summary>
   abstract class Transaction
   {
      /// <summary>
      /// Constructuer pour effectuer une nouvelle transaction
      /// </summary>
      /// <param name="compte">Le compte sur lequel la transaction est effectuée</param>
      /// <param name="montant">Le montant de la transaction</param>
      /// <exception cref="Exception">Si le montant est invalide</exception>
      public Transaction(Compte compte, double montant)
      {
         if (montant <= 0)
         {
            throw new Exception("montant invalide");
         }

         _compte = compte;
         _montant = montant;
         // Utilise la date actuelle comme date de la transaction
         _date = DateTime.Now;
      }


      /// <summary>
      /// Constructeur pour recréer un transaction à partir de l'information provenant du fichier des transactions
      /// </summary>
      /// <param name="compte">Le compte sur lequel la transaction est effectuée</param>
      /// <param name="valeurs">Les valeurs lues d'une ligne du fichier</param>
      /// <exception cref="Exception">Si les données sont corrompues</exception>
      public Transaction(Compte compte, string[] valeurs)
      {
         _compte = compte;

         // valeurs[0] contient le #compte, valeurs[1] contient le type de transaction
         _montant = Convert.ToDouble(valeurs[2]);  // valeurs[2] contient le montant
         if (_montant <= 0)
         {
            throw new Exception("montant invalide");
         }
         _date = Convert.ToDateTime(valeurs[3]);  // valeurs[3] contient la date
      }


      /// <summary>
      /// Sauvegarde l'information de la transaction dans le fichier donné
      /// </summary>
      /// <param name="fichier">Le fichier dans lequel écrire l'information</param>
      public void Sauvegarder(StreamWriter fichier)
      {
         fichier.WriteLine("{0};{1};{2};{3}", _compte.Numero, Identificateur(), _montant, _date.ToShortDateString());
      }


      /// <summary>
      /// Effectue la transaction sur le compte
      /// </summary>
      public abstract void Effectuer();


      /// <summary>
      /// Indique le type spécialisé de la transaction
      /// </summary>
      /// <returns>La chaine de caractère utilisée pour représenter le type de transaction</returns>
      protected abstract string Identificateur();

      /// <summary>
      /// Indique le nom de la transaction
      /// </summary>
      /// <returns>La nom décrivant le type de transaction</returns>
      protected abstract string NomTransaction();


      protected Compte _compte;
      protected readonly double _montant;
      protected double _soldeFinal;
      private readonly DateTime _date;
   }
}
