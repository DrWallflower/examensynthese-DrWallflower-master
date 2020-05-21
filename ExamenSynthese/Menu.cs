using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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
        /// Titre du menu
        /// </summary>
        /// <param name="titreMenu"></param>
        public Menu(string titreMenu) // ToCheck
        {
            _titreMenu = titreMenu;
             
        }



        /// <summary>
        /// Affiche le menu et demande le choix de l'utilisateur.
        /// Réaffiche le menu en boucle tant qu'une option valide n'est pas donnée.
        /// </summary>
        /// <returns>Le caractère valide donné par l'utilisateur</returns>
        public char Afficher() // ToCheck
        {

            string infos01 = _listeOptions[0];
            string[] infosDivises01 = infos01.Split(')');
            char infoOption01 = Convert.ToChar(infosDivises01[0].Substring(1, 1));

            string infos02 = _listeOptions[1];
            string[] infosDivises02 = infos02.Split(')');
            char infoOption02 = Convert.ToChar(infosDivises02[0].Substring(1,1));

            string infos03 = _listeOptions[2];
            string[] infosDivises03 = infos03.Split(')');
            char infoOption03 = Convert.ToChar(infosDivises03[0].Substring(1, 1));

            string infos04 = _listeOptions[3];
            string[] infosDivises04 = infos04.Split(')');
            char infoOption04 = Convert.ToChar(infosDivises04[0].Substring(1, 1));
            

            // Afficher le menu voulu

            Console.WriteLine("================================================================================");
            Console.WriteLine("= " + _titreMenu + "                                                                       =");
            Console.WriteLine("================================================================================");
            Console.WriteLine();
            foreach (var option in _listeOptions)
            {
                Console.WriteLine(option);
            }


            string entree = Console.ReadLine();

            entree = entree.ToUpper();

            char lettre = Convert.ToChar(entree);

            while (true)
            {
                if (!(lettre == infoOption01 || lettre == infoOption02 || lettre == infoOption03 || lettre == infoOption04))
                {

                    Console.WriteLine("Option invalide.");
                    Console.ReadKey();
                    Console.Clear();

                    // Afficher le menu voulu

                    Console.WriteLine("================================================================================");
                    Console.WriteLine("= " + _titreMenu + "                                                                       =");
                    Console.WriteLine("================================================================================");

                    entree = Console.ReadLine();

                    lettre = Convert.ToChar(entree);
                }

                else
                {
                    break;
                }

            }

            return lettre;
        }

        public static void AfficherTitre(string message) // ToCheck --- Done
        {
            Console.WriteLine(message);
        }

        public List<string> _listeOptions = new List<string>();
        private readonly string _titreMenu;
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

        /// <summary>
        /// Ajoute les informations dans des listes
        /// </summary>
        /// <param name="optionMenu"></param>
        public void AjouterOption(OptionMenu optionMenu)
        {
            _listeOptions.Add(optionMenu.ToString());
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

        private void AjouterOption(OptionMenu optionMenu) // ToCheck
        {
            throw new NotImplementedException();
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

        private void AjouterOption(OptionMenu optionMenu) // ToCheck
        {
            throw new NotImplementedException();
        }
    }

}
