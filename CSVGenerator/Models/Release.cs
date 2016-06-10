namespace CsvGenerator.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Interfaces;

    public class Release : IRelease
    {
        private readonly List<string> downloadLinks;
        private List<string> tags; 

        public Release(string name, string path, string category)
        {
            this.Name = name;
            this.Path = path;
            this.Category = category;

            this.downloadLinks = new List<string>();

            this.ExtractTags();
        }

        public string Name { get; private set; }

        public string Path { get; private set; }

        public string Category { get; private set; }

        public List<string> Tags
        {
            get
            {
                return this.tags;
            }
        }

        public List<string> DownloadLinks
        {
            get
            {
                return this.downloadLinks;
            }
        }

        public void AddLink(string link)
        {
            this.downloadLinks.Add(link);
        }

        public void AddLinks(List<string> links)
        {
            this.downloadLinks.AddRange(links);
        }

        private void ExtractTags()
        {
            var banned = new[] { "vol", "volume", "djc" };
            var separators =
                new[] { ',', '.', '!', '?', '-', '[', ']', '<', '>', '(', ')', '{', '}', '_', '\t', '\n', ' ', '*' };

            this.tags = this.Name
                .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Where(t => !Regex.IsMatch(t, @"^\d+$"))
                .Select(t => t.ToLower())
                .Where(lt => !banned.Contains(lt) && lt.Length > 2)
                .ToList();
        }
    }
}