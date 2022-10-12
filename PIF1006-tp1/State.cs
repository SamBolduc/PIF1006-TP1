using System;
using System.Collections.Generic;

namespace PIF1006_tp1
{
    //Classe représentant les états d'un automate
    public class State
    {
        public bool IsFinal { get; }
        public string Name { get; }
        public List<Transition> Transitions { get; }

        public State(string name, bool isFinal)
        {
            Name = name;
            IsFinal = isFinal;
            Transitions = new List<Transition>();
        }

        //Retourne les informations de l'état et de ses transitions
        public override string ToString()
        {
            
            var fullString = "";
            var transactions = "";


            fullString += Name + " ->";

            foreach (var elementInit in Transitions)
            {
                transactions += elementInit.ToString();
            }

            transactions += "\n";
            fullString += transactions.Remove(0, 1);

            return fullString;
        }
    }
}