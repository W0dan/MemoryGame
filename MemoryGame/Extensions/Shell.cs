using System;

namespace MemoryGame.Extensions
{
    public class Shell
    {
        public void WriteLine(string format = "", params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public int ReadInt(int defaultInt)
        {
            var numberString = Console.ReadLine();

            int number;
            if (!int.TryParse(numberString, out number))
            {
                number = defaultInt;
            }
            return number;
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }
    }
}