namespace CsvGenerator.Exceptions
{
    using System;

    public class CsvGeneratorException : Exception
    {
        public CsvGeneratorException(string message)
            : base(message)
        {
        }
    }
}