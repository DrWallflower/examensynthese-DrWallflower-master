using System.IO;


namespace ExamenSynthese
{
   /// <summary>
   /// Type spécialisé de compte bancaire
   /// </summary>
   class CompteEpargne : Compte
   {
      /// <summary>
      /// Chaine de caractère utilisée pour représenter le type
      /// </summary>
      public const string IdentificateurType = "E";


      /// <summary>
      /// Constructeur pour ouvrir un nouveau compte
      /// </summary>
      /// <param name="prenom">Le prénom du propriétaire</param>
      /// <param name="nom">Le nom du propriétaire</param>
      public CompteEpargne(string prenom, string nom) :
          base(prenom, nom)
      {
      }


      /// <summary>
      /// Constructeur pour recréer un compte à partir de l'information provenant du fichier des comptes
      /// </summary>
      /// <param name="valeurs">Les valeurs lues d'une ligne du fichier</param>
      /// <exception cref="Exception">Voir Compte</exception>
      public CompteEpargne(string[] valeurs) :
          base(valeurs)
      {
      }


      /// <summary>
      /// Redéfinition de Compte.CalculerInterets
      /// </summary>
      public override double CalculerInterets()
      {
         // Pour un compte d'épargne, les intérets sont de 1% du solde actuel
         return Solde * 0.01;
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
         fichier.WriteLine();
      }


      /// <summary>
      /// Redéfinition de Compte.TypeCompte
      /// </summary>
      protected override string TypeCompte()
      {
         return "Épargne";
      }
   }
}
