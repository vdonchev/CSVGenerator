namespace CsvGenerator.IO
{
    using System;
    using Interfaces;

    public class ConsoleInOut : IInputOutput
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void WriteLine(object text, IoColors color = IoColors.Default)
        {
            switch (color)
            {
                case IoColors.Red:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case IoColors.Green:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case IoColors.Blue:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case IoColors.Default:
                    break;
                default:
                    break;
            }

            Console.WriteLine(text);

            Console.ResetColor();
        }
    }
}