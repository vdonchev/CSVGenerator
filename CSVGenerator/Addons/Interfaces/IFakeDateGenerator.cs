namespace CsvGenerator.Addons.Interfaces
{
    using System;

    public interface IFakeDateGenerator
    {
        int NumberOfReleases { get; set; }

        DateTime Next();
    }
}