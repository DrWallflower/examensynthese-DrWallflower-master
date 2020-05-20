using System;
using System.Text;
using System.IO;


namespace ExamenSynthese
{
   /// <summary>
   /// Type spécialisé de compte bancaire
   /// </summary>
   class CompteCredit : Compte
   {
      /// <summary>
      /// Chaine de caractère utilisée pour représenter le type
      /// </summary>
      public const string IdentificateurType = "R";


      /// <summary>
      /// Constructeur pour ouvrir un nouveau compte
      /// </summary>
      /// <param name="prenom">Le prénom du propriétaire</param>
      /// <param name="nom">Le nom du propriétaire</param>
      /// <param name="limiteCredit">La limite de crédit</param>
      public CompteCredit(string prenom, string nom, double limiteCredit) :
          base(prenom, nom)
      {
         _limiteCredit = limiteCredit;
      }


      /// <summary>
      /// Constructeur pour recréer un compte à partir de l'information provenant du fichier des comptes
      /// </summary>
      /// <param name="valeurs">Les valeurs lues d'une ligne du fichier</param>
      /// <exception cref="Exception">Voir Compte</exception>
      public CompteCredit(string[] valeurs) :
          base(valeurs)
      {
         // _indexValeurs contient l'index de la prochaine valeur à lire
         _limiteCredit = Convert.ToDouble(valeurs[_indexValeurs++]);
      }


      /// <summary>
      /// Redéfinition de Compte.Retirer
      /// </summary>
      public override double Retirer(double montant, Transaction transaction)
      {
         // Le résultat du retrait ne doit pas être moins que la valeur négative de la limite de crédit
         if ((Solde - montant) >= -_limiteCredit)
         {
            Solde -= montant;
            _transactions.Add(transaction);
            return Solde;
         }

         throw new Exception("limite de crédit insufisante");
      }


      /// <summary>
      /// Redéfinition de Compte.CalculerInterets
      /// </summary>
      public override double CalculerInterets()
      {
         if (Solde < 0)
         {
            // Pour un compte de crédit, les intérêts sont de 4,5% de la valeur absolue d'un solde négatif
            return Solde * -0.045;
         }
         return 0; // Aucun intérêt sur solde nul ou positif
      }


      /// <summary>
      /// Redéfinition de Compte.Sauvegarder
      /// </summary>
      public override void Sauvegarder(StreamWriter fichier)
      {
         // Type de compte
         fichier.Write("{0}", IdentificateurType);
         // Information de base
         SauvegarderBase(fichier);
         // Limite de crédit
         fichier.Write(";{0}", _limiteCredit);
         fichier.WriteLine();
      }


      /// <summary>
      /// Redéfinition de Compte.InfoSpecialisee
      /// </summary>
      protected override string InfoSpecialisee()
      {
         // Il faut afficher la limite de crédit
         StringBuilder sb = new StringBuilder();
         sb.AppendFormat("Limite de crédit:  {0,10:C}", _limiteCredit);
         return sb.ToString();
      }


      /// <summary>
      /// Redéfinition de Compte.TypeCompte
      /// </summary>
      protected override string TypeCompte()
      {
         return "Crédit";
      }


      private readonly double _limiteCredit;
   }
}
