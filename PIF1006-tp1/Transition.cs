namespace PIF1006_tp1
{
    //Classe représentant les transitions entre les états
    public class Transition
    {
        public char Input { get; }
        public State TransiteTo { get; }

        public Transition(char input, State transiteTo)
        {
            Input = input;
            TransiteTo = transiteTo;
        }

        //Retourne les informations de la transition dépandamment si l'état est final ou pas
        public override string ToString()
        {

            if (TransiteTo != null)
            {
                return ", " + TransiteTo.Name + "(" + Input+")";
            } else
            {
                return " ";
            }

        }
    }
}