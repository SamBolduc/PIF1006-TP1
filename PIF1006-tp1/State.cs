using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIF1006_tp1
{
    public class State
    {
        public bool IsFinal {get; set;}
        public string Name { get; private set; }
        public List<Transition> Transitions { get; private set; }

        public State (string name, bool isFinal)
        {
            Name = name;
            IsFinal = isFinal;
            Transitions = new List<Transition>();
        }

        public override string ToString()
        {
            String FullString = "";
            var Transactions = "";

            if (IsFinal)
            {
                FullString = "Final ";
            }
            FullString += Name + " ->";

            foreach (var ElementInit in Transitions)
            {
                Transactions += ElementInit.ToString();
            }

            Transactions += "\n";
            FullString += Transactions.Remove(0, 1);

            return FullString;
        }
    }
}
