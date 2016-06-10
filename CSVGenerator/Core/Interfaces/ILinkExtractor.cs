namespace CsvGenerator.Core.Interfaces
{
    using System.Collections.Generic;

    public interface ILinkExtractor
    {
        Dictionary<string, List<string>> ExtractLinks(string filePath);
    }
}