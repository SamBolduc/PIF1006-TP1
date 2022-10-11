using System.Collections.Generic;
using System.Linq;

namespace PIF1006_tp1
{
    public class Automate
    {
        private State InitialState { get; }
        private State CurrentState { get; set; }

        public Automate(State initialState)
        {
            InitialState = initialState;
            Reset();
        }

        public bool Validate(string input)
        {
            Reset();

            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];

                var nextTrans = CurrentState.Transitions.FirstOrDefault(trans => trans.Input == c);

                if (nextTrans != null)
                    CurrentState = nextTrans.TransiteTo ?? CurrentState;
                else
                    return false;

                if (i == input.Length - 1 && !CurrentState.IsFinal)
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            return InitialState + AllState(InitialState.Transitions, InitialState.Name);
        }

        private string AllState(List<Transition> transitions, string alreadyPrint)
        {
            var allInfoState = "";

            foreach (var element in transitions)
            {
                if (element.TransiteTo == null || alreadyPrint.Contains(element.TransiteTo.Name)) continue;

                allInfoState += element.TransiteTo.ToString();
                alreadyPrint += element.TransiteTo.Name;
                allInfoState += AllState(element.TransiteTo.Transitions, alreadyPrint);
            }

            return allInfoState;
        }

        private void Reset() => CurrentState = InitialState;
    }
}