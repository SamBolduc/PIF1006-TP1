using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PIF1006_tp1
{
    public class Automate
    {
        public State InitialState { get; set; }
        public State CurrentState { get; set; }

        public Automate(State initialState)
        {
            InitialState = initialState;
            Reset();
        }

        public Automate()
        {
        }

        public bool Validate(string input)
        {
            var isValid = true;
            Reset();

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                Transition nextTrans = CurrentState.Transitions.FirstOrDefault(trans => trans.Input == c);

                if (nextTrans != null)
                    CurrentState = nextTrans.TransiteTo ?? CurrentState;
                else
                    return false;

                if (i == input.Length - 1 && !CurrentState.IsFinal)
                    return false;
            }

            // Vous devez transformer l'input en une liste / un tableau de caractères (char) et les lire un par un;
            // L'automate doit maintenant à jour son "CurrentState" en suivant les transitions et en respectant l'input.
            // Considérez que l'automate est déterministe et que même si dans les faits on aurait pu mettre plusieurs
            // transitions possibles pour un état et un input donné, le 1er trouvé dans la liste est le chemin emprunté.
            // Si aucune transition n'est trouvé pour un état courant et l'input donné, cela doit retourner faux;
            // Si tous les caractères ont été pris en compte, on vérifie si l'état courant est final ou non et on retourne
            // vrai ou faux selon.

            return isValid;
        }

        public override string ToString()
        {
            String fullInfo = "";
            fullInfo = InitialState.ToString() + allState(InitialState.Transitions, InitialState.Name);

            // Vous devez modifier cette partie de sorte à retourner un équivalent string qui décrit tous les états et
            // la table de transitions de l'automate.
            return fullInfo;
        }

        public String allState(List<Transition> Transitions, String alreadyPrint)
        {
            String allInfoState = "";

            foreach (var element in Transitions)
            {
                if (element.TransiteTo != null && !alreadyPrint.Contains(element.TransiteTo.Name))
                {
                    allInfoState += element.TransiteTo.ToString();
                    alreadyPrint += element.TransiteTo.Name;
                    allInfoState += allState(element.TransiteTo.Transitions, alreadyPrint);
                }
            }

            return allInfoState;
        }

        public void Reset() => CurrentState = InitialState;
    }
}