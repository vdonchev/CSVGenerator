namespace CsvGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Core.Interfaces;
    using Exceptions;
    using Helpers;
    using IO;
    using IO.Interfaces;
    using Models;
    using Models.Interfaces;

    public class App
    {
        private Dictionary<string, IRelease> releases;
        private Dictionary<string, List<string>> extractedLinks;

        private string releasesDirectory;
        private string linksFile;

        public App(
            IInputOutput io, 
            ILinkExtractor linkExtractor, 
            ICsvBuilder csvBuilder)
        {
            this.Io = io;
            this.LinkExtractor = linkExtractor;
            this.CsvBuilder = csvBuilder;
        }

        public IInputOutput Io { get; private set; }

        public ILinkExtractor LinkExtractor { get; private set; }

        public ICsvBuilder CsvBuilder { get; private set; }

        public void Run()
        {
            try
            {
                this.ReadInputs();

                this.extractedLinks = this.LinkExtractor.ExtractLinks(this.linksFile);

                this.ProcessReleaseFolders();

                this.ProcessLinks();

                this.CsvBuilder.BuildAndSave(this.releases);
            }
            catch (CsvGeneratorException ex)
            {
                this.Io.WriteLine(ex.Message, IoColors.Red);
            }
            catch (Exception ex)
            {
                this.Io.WriteLine("System error: " + ex.Message, IoColors.Red);
            }

            this.Io.WriteLine(Constants.AllDone, IoColors.Green);
        }

        private void ProcessLinks()
        {
            foreach (var extractedLink in this.extractedLinks)
            {
                var linkName = extractedLink.Key;
                var linkUrls = extractedLink.Value;
                if (this.releases.ContainsKey(linkName))
                {
                    this.releases[linkName].AddLinks(linkUrls);
                }
            }
        }

        private void ProcessReleaseFolders()
        {
            this.releases = new Dictionary<string, IRelease>();
            var allReleasesPaths = Directory.GetDirectories(this.releasesDirectory);
            foreach (var releasePath in allReleasesPaths)
            {
                var releaseName = SystemHelper.ExtractFolderName(releasePath);
                var releaseMd5Name = SystemHelper.NameToMd5(releaseName);
                if (this.releases.ContainsKey(releaseMd5Name))
                {
                    throw new InvalidOperationException(Constants.DupeFolder);
                }

                this.releases[releaseMd5Name] = 
                    new Release(releaseName, releasePath, SystemHelper.ExtractFolderName(this.releasesDirectory));
            }
        }

        private void ReadInputs()
        {
            this.Io.WriteLine(Constants.SelectReleasesFolder);
            this.releasesDirectory = SystemHelper.SelectFolder();

            this.Io.WriteLine(Constants.SelectFileWithLinks);
            this.linksFile = SystemHelper.SelectFile();
        }
    }
}