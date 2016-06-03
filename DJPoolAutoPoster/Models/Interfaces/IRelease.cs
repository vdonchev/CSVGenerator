namespace CsvGenerator.Models.Interfaces
{
    using System.Collections.Generic;

    public interface IRelease
    {
        string Name { get; }

        string Path { get; }

        string Category { get; }

        List<string> Tags { get; }

        List<string> DownloadLinks { get; }

        void AddLink(string link);

        void AddLinks(List<string> links);
    }
}