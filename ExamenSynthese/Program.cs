using System;
using System.Collections.Generic;


namespace ExamenSynthese
{
   /// <summary>
   /// Classe principale du système de gestion bancaire.
   /// Gère les menus et l'exécution générale du programme.
   /// </summary>
   class Program
   {
      /// <summary>
      /// Méthode principale du programme
      /// </summary>
      static void Main()
      {
         try
         {
            // Instantiation d'un objet de la classe courrante.
            Program p = new Program();
            // Appel de la méthode principale de l'objet Program.
            // En ayant un objet, les méthodes et attributs de la classe n'ont pas à être static
            p.Executer();
            Pause("Fin du programme");
         }
         catch (Exception e)
         {
            // On termine proprement le programme en cas d'erreur non gérée
            Pause("Fin du programme dûe à une exception: " + e.Message);
         }
      }


      /// <summary>
      /// Arrête l'exécution du programme pour permttre à l'utilisateur de lire la console.
      /// Attend une action de l'utilisateur avant de continuer.
      /// Doit être static car utilisée par la méthode <c>Main</c>
      /// </summary>
      /// <param name="s">Chaine de caractères optionnelle. Si elle est fournie, elle est affichée dans la console.</param>
      public static void Pause(string s = null)
      {
         Console.WriteLine();
         if (s != null)
         {
            Console.WriteLine(s);
         }

         Console.WriteLine("Appuyez sur une touche pour continuer");
         Console.ReadKey(true);
      }


      /// <summary>
      /// Constructeur
      /// </summary>
      private Program()
      {
         _laBanque = new Banque();
      }


      /// <summary>
      /// Boucle principale du programme
      /// </summary>
      private void Executer()
      {
         MenuPrincipal mp = new MenuPrincipal();
         while (true)
         {
            char choix = mp.Afficher();
            switch (choix)
            {
               case 'O': OuvrirCompte(); break;
               case 'L': ListerComptes(); break;
               case 'A': DemanderCompte(); break;
               case 'Q': return;
            }
         }
      }


      /// <summary>
      /// Option "O" du menu principal
      /// Crée un nouveau compte dans la banque
      /// </summary>
      private void OuvrirCompte()
      {
         Menu.AfficherTitre("Ouverture de compte");

         string type = DemanderType();

         // Les prénom et nom sont de simples chaines de caractères. Aucune validation n'est faite.
         Console.Write("Indiquez le prénom du propriétaire: ");
         string prenom = Console.ReadLine();
         Console.Write("Indiquez le nom du propriétaire: ");
         string nom = Console.ReadLine();

         double montant = 0;
         if (type != CompteCredit.IdentificateurType)
         {
            // Les comptes de crédit sont créés avec un solde initial de zéro
            montant = DemanderMontant("initial");
         }

         // 'AjouterCompte' retourne le numéro du nouveau compte créé
         int numero = _laBanque.AjouterCompte(type, prenom, nom, montant);
         Pause("Le compte " + numero + " a été ajouté");
      }


      /// <summary>
      /// Demande un type de compte voulu à l'utilisateur.
      /// Boucle et redemande tant que le type donné est invalide
      /// </summary>
      /// <returns>La chaine représentant le type choisi: "C", "E" ou "R"</returns>
      private string DemanderType()
      {
         while (true)
         {
            Console.Write("Indiquer le type de compte voulu, (C) Chèques, (E) Épargne, (R) cRédit: ");
            string ligne = Console.ReadLine().ToUpper();
            if (Banque.IdentificateursDeComptes.Contains(ligne))
            {
               return ligne;
            }
            Console.WriteLine("Choix invalide");
         }
      }


      /// <summary>
      /// Demande un montant à l'utilisateur
      /// Boucle et redemande tant que le montant donné est invalide
      /// </summary>
      /// <param name="titre">Une indication sur l'utilité du montant affiché dans la console</param>
      /// <returns></returns>
      private double DemanderMontant(string titre)
      {
         while (true)
         {
            try
            {
               Console.Write("Indiquer le montant {0}: ", titre);
               double montant = Convert.ToDouble(Console.ReadLine());
               if (montant <= 0)
               {
                  throw new Exception();
               }
               return montant;
            }
            catch
            {
               Console.WriteLine("Montant invalide");
            }
         }
      }


      /// <summary>
      /// Option "L" du menu principal
      /// Affiche la liste de tous les comptes de la banque
      /// </summary>
      private void ListerComptes()
      {
         Menu.AfficherTitre("Liste des comptes");

         // Obtient la liste à afficher sous forme d'une liste de chaine de caractère
         List<string> liste = _laBanque.ListeDeComptes();

         if (liste.Count == 0)
         {
            Pause("Aucun compte n'a encore été créé!");
            return;
         }

         foreach (var ligne in liste)
         {
            Console.WriteLine(ligne);
         }

         Pause();
      }


      /// <summary>
      /// Option "A" du menu principal
      /// Demande le numéro du compte à l'utilisateur.
      /// Si le numéro est valide et correspond à un compte existant, continue à la méthode <c>AccederCompte</c>
      /// Sinon, se termine pour retourner au menu principal
      /// </summary>
      private void DemanderCompte()
      {
         Console.Write("\n\nNuméro du compte: ");
         string ligne = Console.ReadLine();

         try
         {
            int numeroCompte = Convert.ToInt32(ligne);
            // ValiderExistence va lancer un ArgumentException si le compte donné n'existe pas dans la banque
            _laBanque.ValiderExistence(numeroCompte);
            // Si aucune exception n'a été lancée, le compte existe.
            AccederCompte(numeroCompte);
         }
         catch (FormatException)
         {
            Pause("Numéro invalide");
         }
         catch (ArgumentException)
         {
            Pause("Ce compte n'existe pas");
         }
      }


      /// <summary>
      /// Suite de l'option "A" du menu principal
      /// Appellée par <c>DemanderCompte</c> une fois la validation faite
      /// Boucle dans le sous-menu tant que l'utilisateur ne choisi pas de retourner au menu principal
      /// </summary>
      /// <param name="numeroCompte">Le numéro du compte à accéder</param>
      private void AccederCompte(int numeroCompte)
      {
         MenuCompte mc = new MenuCompte(numeroCompte);
         while (true)
         {
            char choix = mc.Afficher();
            switch (choix)
            {
               case 'S': AfficherSolde(numeroCompte); break;
               case 'R': ReleveDeTransactions(numeroCompte); break;
               case 'T': EffectuerTransaction(numeroCompte); break;
               case 'Q': return;
            }
         }
      }


      /// <summary>
      /// Option "S" du menu compte
      /// Affiche le solde du compte donné
      /// </summary>
      /// <param name="numeroCompte">Le numéro du compte duquel afficher le solde</param>
      private void AfficherSolde(int numeroCompte)
      {
         Console.WriteLine("\n\nSolde du compte: {0,12:C}", _laBanque.Solde(numeroCompte));
         Pause();
      }


      /// <summary>
      /// Option "R" du menu compte
      /// Affiche la liste des transactions du compte
      /// </summary>
      /// <param name="numeroCompte">Le numéro du compte duquel afficher les transactions</param>
      private void ReleveDeTransactions(int numeroCompte)
      {
         Menu.AfficherTitre("Relevé de transactions du compte " + numeroCompte);

         // Obtient la liste à afficher sous forme d'une liste de chaine de caractère
         List<string> liste = _laBanque.ReleveDeTransactions(numeroCompte);

         if (liste.Count == 0)
         {
            Pause("Aucune transaction n'a été effectuée!");
            return;
         }

         foreach (var ligne in liste)
         {
            Console.WriteLine(ligne);
         }
         Pause();
      }


      /// <summary>
      /// Option "T" du menu compte
      /// Boucle dans le sous-menu tant que l'utilisateur ne choisi pas de retourner au menu du compte
      /// </summary>
      /// <param name="numeroCompte">Le numéro du compte sur lequel effectuer une transaction</param>
      private void EffectuerTransaction(int numeroCompte)
      {
         MenuTransaction mt = new MenuTransaction(numeroCompte);
         while (true)
         {
            char choix = mt.Afficher();
            switch (choix)
            {
               case 'D': Deposer(numeroCompte); break;
               case 'R': Retirer(numeroCompte); break;
               case 'I': CalculerInterets(numeroCompte); break;
               case 'Q': return;
            }
         }
      }


      /// <summary>
      /// Option "D" du menu transaction
      /// Effectue un dépôt dans le compte donné
      /// </summary>
      /// <param name="numeroCompte">Le numéro du compte dans lequel effectuer un dépôt</param>
      private void Deposer(int numeroCompte)
      {
         double montant = DemanderMontant("du dépôt");

         // 'Deposer' retourne le nouveau solde après le dépôt
         double solde = _laBanque.Deposer(numeroCompte, montant);

         Console.WriteLine("\nDépôt effectué, nouveau solde du compte: {0,12:C}", solde);
         Pause();
      }


      /// <summary>
      /// Option "R" du menu transaction
      /// Tente d'effectuer un retrait du le compte donné
      /// </summary>
      /// <param name="numeroCompte">Le numéro du compte duquel effectuer un retrait</param>
      private void Retirer(int numeroCompte)
      {
         double montant = DemanderMontant("du retrait");
         // 'Retirer' dans la banque va lancer une exception si les fonds sont insuffisant pour le retrait demandé
         try
         {
            // 'Retirer' retourne le nouveau solde après le retrait
            double solde = _laBanque.Retirer(numeroCompte, montant);

            Console.WriteLine("\nRetrait effectué, nouveau solde du compte: {0,12:C}", solde);
            Pause();
         }
         catch (Exception e)
         {
            Pause("Retrait impossible, " + e.Message);
         }
      }


      /// <summary>
      /// Option "I" du menu transaction
      /// Affiche les intérêts calculés sur le compte donné
      /// </summary>
      /// <param name="numeroCompte">Le numéro du compte duquel effectuer un retrait</param>
      private void CalculerInterets(int numeroCompte)
      {
         // 'CalculerInterets' retourne le montant d'intérêts calculé pour le compte
         Console.WriteLine("\nIntérêts sur le compte: {0,12:C}", _laBanque.CalculerInterets(numeroCompte));
         Pause();
      }


      /// <summary>
      /// La banque qui contient tous les comptes et dans laquelle toute les opérations sont effectuées
      /// </summary>
      private Banque _laBanque;
   }
}