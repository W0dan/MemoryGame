namespace MemoryGame.Server.Core
{
    public class Card
    {
        readonly int _number;

        public Card(int number)
        {
            _number = number;
        }

        public int Number
        {
            get { return _number; }
        }

        //public string FileName
        //{
        //    get { return _number.ToString("000") + ".png"; }
        //}

        //public string ResourceName
        //{
        //    get { return "Client/Resources/Images/" + FileName; }
        //}
    }
}