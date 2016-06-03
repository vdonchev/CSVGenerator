namespace CsvGenerator.Core.Interfaces
{
    using System.Collections.Generic;
    using Models.Interfaces;

    public interface ICsvBuilder
    {
        void BuildAndSave(Dictionary<string, IRelease> releases);
    }
}