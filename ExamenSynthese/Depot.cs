

namespace ExamenSynthese
{
   /// <summary>
   /// Type spécialisé de transaction
   /// </summary>
   class Depot : Transaction
   {
      /// <summary>
      /// Chaine de caractère utilisée pour représenter le type
      /// </summary>
      public const string IdentificateurType = "D";


      /// <summary>
      /// Constructuer pour effectuer un nouveau dépôt
      /// </summary>
      /// <param name="compte">Le compte sur lequel le dépôt est effectué</param>
      /// <param name="montant">Le montant du dépôt</param>
      /// <exception cref="Exception">Voir Transaction</exception>
      public Depot(Compte compte, double montant) :
          base(compte, montant)
      {
      }


      /// <summary>
      /// Constructeur pour recréer un dépôt à partir de l'information provenant du fichier des transactions
      /// </summary>
      /// <param name="compte">Le compte sur lequel le dépôt est effectué</param>
      /// <param name="valeurs">Les valeurs lues d'une ligne du fichier</param>
      /// <exception cref="Exception">Voir Transaction</exception>
      public Depot(Compte compte, string[] valeurs) :
          base(compte, valeurs)
      { }


      /// <summary>
      /// Redéfinition de Transaction.Effectuer
      /// </summary>
      public override void Effectuer()
      {
         // Effectue le dépôt dans le compte en donnant une référence sur nous-même
         _soldeFinal = _compte.Deposer(_montant, this);
      }


      /// <summary>
      /// Redéfinition de Transaction.Identificateur
      /// </summary>
      protected override string Identificateur()
      {
         return IdentificateurType;
      }


      /// <summary>
      /// Redéfinition de Transaction.NomTransaction
      /// </summary>
      protected override string NomTransaction()
      {
         return "Dépôt";
      }
   }
}
