namespace CsvGenerator.Core
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Interfaces;

    public class UniversalLinkExtractor : ILinkExtractor
    {
        public Dictionary<string, List<string>> ExtractLinks(string filePath)
        {
            var result = new Dictionary<string, List<string>>();

            var links = File.ReadLines(filePath);
            foreach (var link in links)
            {
                var linkName = this.GetLinkName(link);
                if (!result.ContainsKey(linkName))
                {
                    result[linkName] = new List<string>();
                }

                result[linkName].Add(link);
            }

            return result;
        }

        private string GetLinkName(string linkUrl)
        {
            var linkName = linkUrl
                .TrimEnd('/')
                .Split('/')
                .Last()
                .Split('.')
                .First();

            return linkName;
        }
    }
}