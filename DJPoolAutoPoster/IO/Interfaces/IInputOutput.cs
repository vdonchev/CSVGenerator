namespace CsvGenerator.IO.Interfaces
{
    public interface IInputOutput
    {
        string ReadLine();

        void WriteLine(object text, IoColors color = IoColors.Default);
    }
}