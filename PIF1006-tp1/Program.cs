﻿//Membres de l'équipe: Samuel Bolduc, Alexis Michaud & Benoit Légaré

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PIF1006_tp1
{
    public static class Program
    {
        private static Automate _automate;

        private static void Main(string[] args)
        {
            LoadFromFile("./automate.txt");
            var showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }

            // //-- Ceci est un exemple manuel de ce qui devrait fonctionner --
            // var s0 = new State("s0", false);
            // var s1 = new State("s1", false);
            // var s2 = new State("s2", true);
            // var s3 = new State("s3", false);
            // s0.Transitions.Add(new Transition('0', s1));
            // s1.Transitions.Add(new Transition('0', s0));
            // s1.Transitions.Add(new Transition('1', s2));
            // s2.Transitions.Add(new Transition('1', s2));
            // s2.Transitions.Add(new Transition('0', s3));
            // s3.Transitions.Add(new Transition('1', s1));
            //
            // // Dans cet exemple uniquement, on permet au constructuer d'accueilir un état initial
            // // (qui par référence "transporte" tout l'automate en soi)
            //
            //
            // // Décommenter pour avoir le json d'un automate en output.
            // var automate = new Automate(s0);
            // Console.WriteLine(JsonConvert.SerializeObject(_automate, new JsonSerializerSettings()
            // {
            //     ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            //     PreserveReferencesHandling = PreserveReferencesHandling.Objects
            // }));
            //
            // // On doit pouvoir ensuite appeler une méthode qui permet de valider un input ou non
            // var isValid = automate.Validate("011000");
            //
            // // Et ainsi de suite...
            //
            // //---------------------------------------------------------------------------------------------------------------------------
            // // Ci-haut est un exemple.  Vous devez plutôt faire un menu avec des options/interactions utilisateurs pour:
            // //      (1) Charger un fichier en spécifiant le chemin (relatif) du fichier.  Vous pouvez charger un fichier par défaut au démarrage
            // //      (2) La liste des états et la liste des transitions doivent pouvoir être affichées proprement;
            // //      (3) Soumettre un input en tant que chaîne de 0 ou de 1 -> Assurez-vous que la chaine passée ne contient QUE ces caractères
            // //          avant d'envoyer n'est pas obligatoire, mais cela ne doit pas faire planter de l'autre coté;  un message doit indiquer si
            // //          c'est accepté ou rejeté;
            // //      (4) Quitter l'application.
        }

        private static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Choisi une option:");
            Console.WriteLine("1) Charger un automate");
            Console.WriteLine("2) Afficher l'automate");
            Console.WriteLine("3) Soumettre un input à l'automate");
            Console.WriteLine("4) Quitter l'application");
            Console.Write("\r\nSélectionner une option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine();
                    Console.WriteLine("Veuillez entrer le chemin vers votre fichier txt");
                    LoadFromFile(Console.ReadLine());
                    return true;
                case "2":
                    PrintAutomate();
                    return true;
                case "3":
                    Console.WriteLine();
                    Console.Write("Veuillez entrer une expression constituée uniquement de 0 et 1: ");
                    SendInput(Console.ReadLine());
                    return true;
                case "4":
                    Console.WriteLine("Bye !");
                    return false;
                default:
                    return true;
            }
        }

        private static void SendInput(string input)
        {
            if (!input.All(c => c == '0' || c == '1'))
            {
                SendMessageAndWait("L'entrée ne doit contenir que des 0 et des 1.");
                return;
            }

            if (_automate.Validate(input))
            {
                SendMessageAndWait("L'entrée est valide.");
            }
            else
            {
                SendMessageAndWait("L'entrée n'est pas valide.");
            }

            return;
        }

        private static void PrintAutomate()
        {
            if (!CheckAutomate())
            {
                return;
            }

            SendMessageAndWait(_automate.ToString());
        }

        private static void LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                SendMessageAndWait("Impossible de charger l'automate...");
            }

            var states = new List<State>();

            foreach (var line in File.ReadLines(filePath))
            {
                var trim = line.ToLower();
                var args = trim.Split(" ");
                if (trim.StartsWith("state"))
                {
                    var name = args[1];
                    var finalArg = args[2];
                    var isFinal = AsBool(finalArg);

                    if (args.Length != 3 || isFinal == null || string.IsNullOrWhiteSpace(name) ||
                        string.IsNullOrWhiteSpace(finalArg) ||
                        states.Any(x => x.Name.ToLower().Equals(name.ToLower())))
                    {
                        SendMessageAndWait($"La ligne '{line}' est invalide et a été ignorée.");
                        continue;
                    }

                    var state = new State(name, isFinal.Value);
                    states.Add(state);

                    if (states.Count == 1)
                    {
                        _automate = new Automate(state);
                    }
                }
                else if (trim.StartsWith("transition"))
                {
                    var sourceStateArg = args[1];
                    var input = args[2];
                    var targetStateArg = args[3];
                    var sourceState = states.FirstOrDefault(x => x.Name.ToLower().Equals(sourceStateArg.ToLower()));
                    var targetState = states.FirstOrDefault(x => x.Name.ToLower().Equals(targetStateArg.ToLower()));

                    if (args.Length != 4 || input.Length != 1 || (!input.StartsWith("1") && !input.StartsWith("0")) ||
                        sourceState == null || targetState == null)
                    {
                        SendMessageAndWait($"La ligne '{line}' est invalide et a été ignorée.");
                        continue;
                    }

                    var transition = new Transition(input[0], targetState);
                    sourceState.Transitions.Add(transition);
                }
                else
                {
                    SendMessageAndWait($"La ligne '{line}' est invalide et a été ignorée.");
                }
            }

            SendMessageAndWait("Fin du chargement de l'automate...");
        }

        private static bool? AsBool(string arg)
        {
            if (arg == null)
            {
                return null;
            }

            if (arg.Equals("1"))
            {
                return true;
            }

            if (arg.Equals("0"))
            {
                return false;
            }

            return null;
        }

        private static bool CheckAutomate()
        {
            if (_automate != null) return true;

            SendMessageAndWait("Aucun automate est chargé, appuyer sur une touche pour continuer...");
            return false;
        }

        private static void SendMessageAndWait() => SendMessageAndWait("");

        private static void SendMessageAndWait(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadLine();
        }
    }
}