using System;
using System.Text;


namespace ExamenSynthese
{
   /// <summary>
   /// Une option disponible dans un menu
   /// </summary>
   class OptionMenu : IEquatable<OptionMenu>
   {
      /// <summary>
      /// Constructeur
      /// </summary>
      /// <param name="choix">Le caractère associé à l'option que l'utilitateur doit indiquer pour choisir l'option.</param>
      /// <param name="description">La description de l'option. Optionnelle pour l'utilisation lors de la validation par le menu.</param>
      public OptionMenu(char choix, string description = "")
      {
         _choix = choix;
         _description = description;
      }

      /// <summary>
      /// Redéfinition de Object.ToString
      /// </summary>
      /// <returns>La représentation textuelle de l'option telle qu'affichée dans le menu</returns>
      public override string ToString()
      {
         StringBuilder builder = new StringBuilder();
         builder.AppendFormat(" {0}) {1}", _choix, _description);
         return builder.ToString();
      }

      /// <summary>
      /// Redéfinition de IEquatable.Equals
      /// </summary>
      /// <param name="that">L'autre option avec laquelle se comparer</param>
      /// <returns>True si this est équivalente à that, false sinon</returns>
      public bool Equals(OptionMenu that)
      {
         // Deux options non nulles sont égales si les choix sont égaux
         return that == null ? false : _choix == that._choix;
      }


      private readonly char _choix;
      private readonly string _description;
   }





   /// <summary>
   /// Base de tous les menus de l'application
   /// </summary>
   class Menu
   {

      /// <summary>
      /// Affiche le menu et demande le choix de l'utilisateur.
      /// Réaffiche le menu en boucle tant qu'une option valide n'est pas donnée.
      /// </summary>
      /// <returns>Le caractère valide donné par l'utilisateur</returns>
      public char Afficher()
      {
      }

   }





   /// <summary>
   /// Menu principal de l'application
   /// </summary>
   class MenuPrincipal : Menu
   {
      /// <summary>
      /// Constructeur
      /// </summary>
      public MenuPrincipal() :
         base("Banque")
      {
         AjouterOption(new OptionMenu('O', "Ouvrir un nouveau compte"));
         AjouterOption(new OptionMenu('L', "Lister les comptes"));
         AjouterOption(new OptionMenu('A', "Accéder à un compte"));
         AjouterOption(new OptionMenu('Q', "Quitter"));
      }
   }





   /// <summary>
   /// Menu des comptes
   /// </summary>
   class MenuCompte : Menu
   {
      /// <summary>
      /// Constructeur
      /// </summary>
      /// <param name="numeroCompte">Le numéro du compte courrant, à afficher dans le titre</param>
      public MenuCompte(int numeroCompte) :
         base("Compte " + numeroCompte)
      {
         AjouterOption(new OptionMenu('S', "Afficher solde"));
         AjouterOption(new OptionMenu('T', "Effectuer des transactions"));
         AjouterOption(new OptionMenu('R', "Relevé de transactions"));
         AjouterOption(new OptionMenu('Q', "Retour au menu principal"));
      }
   }





   /// <summary>
   /// Menu des transactions
   /// </summary>
   class MenuTransaction : Menu
   {
      /// <summary>
      /// Constructeur
      /// </summary>
      /// <param name="numeroCompte">Le numéro du compte courrant, à afficher dans le titre</param>
      public MenuTransaction(int numeroCompte) : base("Transactions sur le compte " + numeroCompte)
      {
         AjouterOption(new OptionMenu('D', "Effectuer un dépôt"));
         AjouterOption(new OptionMenu('R', "Effectuer un retrait"));
         AjouterOption(new OptionMenu('I', "Calculer les intérêts"));
         AjouterOption(new OptionMenu('Q', "Retour au menu principal"));
      }
   }
}
