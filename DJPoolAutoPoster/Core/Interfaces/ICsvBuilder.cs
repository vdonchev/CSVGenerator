namespace CsvGenerator.Core.Interfaces
{
    using System.Collections.Generic;
    using Addons.Interfaces;
    using Models.Interfaces;

    public interface ICsvBuilder
    {
        IFakeDateGenerator FakeDateGenerator { get; set; }

        void BuildAndSave(Dictionary<string, IRelease> releases);
    }
}