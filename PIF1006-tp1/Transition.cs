namespace PIF1006_tp1
{
    public class Transition
    {
        public char Input { get; }
        public State TransiteTo { get; }

        public Transition(char input, State transiteTo)
        {
            Input = input;
            TransiteTo = transiteTo;
        }

        public override string ToString()
        {
            if (TransiteTo == null)
            {
                return ", " + Input;
            }

            return ", " + Input + TransiteTo.Name;
        }
    }
}