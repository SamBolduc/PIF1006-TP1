using System.Collections.Generic;
using System.Linq;

namespace PIF1006_tp1
{
    //Classe représentant un automate composé de plusieurs états ayant chacun leurs transitions
    public class Automate
    {
        private State InitialState { get; }
        private State CurrentState { get; set; }

        public Automate(State initialState)
        {
            InitialState = initialState;
            Reset();
        }

        //Retourne si la chaine est valide ou non
        public bool Validate(string input)
        {
            Reset();

            // Si le premier état n'est pas final et qu'il n'y a aucun input(entrée vide 'ε'), alors la validation est fausse
            if (input.Length == 0 && !CurrentState.IsFinal)
            {
                return false;
            }

            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];

                // Trouver la prochaine(première, si non-déterministe) transition qui correspond au input (c) courant
                var nextTrans = CurrentState.Transitions.FirstOrDefault(trans => trans.Input == c);

                // Si on trouve une prochaine transition, alors on met à jour le CurrentState.
                if (nextTrans != null)
                    // Si la transition ne contient pas de TransiteTo, c'est que l'on transite sur le même état (boucle)
                    CurrentState = nextTrans.TransiteTo ?? CurrentState;
                else
                    return false;

                // Si nous avons épuiser l'input et que le CurrentState n'est pas final, alors l'input est invalide
                if (i == input.Length - 1 && !CurrentState.IsFinal)
                    return false;
            }

            return true;
        }

        //Retourne les informations complète de l'automate. Elle renvoie une grammaire régulière représentant l'automate
        public override string ToString()
        {

            if (InitialState.IsFinal)
            {
                return InitialState.Name + "(F)" + AllStateName(InitialState.Transitions, InitialState.Name) + "\n\n" + InitialState.ToString().Insert(5, " ε, ") + AllState(InitialState.Transitions, InitialState.Name);
            }
            else {
                return InitialState.Name + AllStateName(InitialState.Transitions, InitialState.Name) + "\n\n" + InitialState + AllState(InitialState.Transitions, InitialState.Name);
            }

        }

        //Fonction récursive servant à aller chercher tous les états et leurs transitions
        private string AllState(List<Transition> transitions, string alreadyPrint)
        {
            
            var allInfoState = "";

            foreach (var element in transitions)
            {
                // Vérifie si l'état et ses transitions n'ont pas déjà été ajouté dans la variable à print
                if (!alreadyPrint.Contains(element.TransiteTo.Name + " ->") && element.TransiteTo.Name != "s0")
                {
                    allInfoState += element.TransiteTo.ToString();
                    alreadyPrint += allInfoState;
                    allInfoState += AllState(element.TransiteTo.Transitions, alreadyPrint);
                    alreadyPrint += allInfoState;
                }
            }

            return allInfoState;
        }

        //Fonction récursive servant à aller chercher tous les états et s'ils sont finaux ou pas
        private string AllStateName(List<Transition> transitions, string alreadyPrint)
        {

            var allInfoState = "";

            foreach (var element in transitions)
            {
                // Vérifie si l'état n'a pas déjà été ajouté dans la variable à print
                if (!alreadyPrint.Contains(element.TransiteTo.Name) && element.TransiteTo.Name != "s0")
                {
                    if (element.TransiteTo.IsFinal)
                    {
                        allInfoState += ", " + element.TransiteTo.Name+"(F)";
                    }
                    else {
                        allInfoState += ", " + element.TransiteTo.Name;
                    }

                    alreadyPrint += allInfoState;
                    allInfoState += AllStateName(element.TransiteTo.Transitions, alreadyPrint);
                    alreadyPrint += allInfoState;
                }
            }

            return allInfoState;
        }

        private void Reset() => CurrentState = InitialState;
    }
}