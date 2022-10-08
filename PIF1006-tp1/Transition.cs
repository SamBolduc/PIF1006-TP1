namespace PIF1006_tp1
{
    public class Transition
    {
        public char Input { get; set; }
        public State TransiteTo { get; set; }

        public Transition(char input, State transiteTo)
        {
            Input = input;
            TransiteTo = transiteTo;
        }

        public Transition()
        {
        }

        public override string ToString()
        {
            if (TransiteTo == null)
            {
                return ", " + Input;
            }
            else
            {
                return ", " + Input + TransiteTo.Name;
            }
        }
    }
}