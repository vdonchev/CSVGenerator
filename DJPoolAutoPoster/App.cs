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

        private string sourceDirectory;
        private string linksFile;
        private readonly IInputOutput io;
        private readonly ILinkExtractor linkExtractor;
        private readonly ICsvBuilder csvBuilder;

        public App(
            IInputOutput io,
            ILinkExtractor linkExtractor,
            ICsvBuilder csvBuilder)
        {
            this.io = io;
            this.linkExtractor = linkExtractor;
            this.csvBuilder = csvBuilder;
        }

        public void Run()
        {
            try
            {
                this.ReadInputs();

                this.extractedLinks = this.linkExtractor.ExtractLinks(this.linksFile);

                this.ProcessReleaseFolders();

                this.ProcessLinks();

                this.io.WriteLine(Constants.Building, IoColors.Blue);
                this.csvBuilder.BuildAndSave(this.releases);
            }
            catch (CsvGeneratorException ex)
            {
                this.io.WriteLine(ex.Message, IoColors.Red);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                this.io.WriteLine("System error: " + ex.Message, IoColors.Red);
                Environment.Exit(0);
            }

            this.io.WriteLine(Constants.AllDone, IoColors.Green);
            this.io.ReadLine();
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
            var allReleasesPaths = Directory.GetDirectories(this.sourceDirectory);
            foreach (var releasePath in allReleasesPaths)
            {
                var releaseName = SystemHelper.ExtractFolderName(releasePath);
                var releaseMd5Name = SystemHelper.ToAlphaNumberc(releaseName);
                if (this.releases.ContainsKey(releaseMd5Name))
                {
                    throw new InvalidOperationException(Constants.DupeFolder);
                }


                var categoryName = SystemHelper.ExtractFolderName(this.sourceDirectory);
                if (bool.Parse(CsvGenerator.Settings["ExtractCatFromRelease"]))
                {
                    categoryName = SystemHelper.ExtractCategoryFromRelease(releasePath);
                }

                this.releases[releaseMd5Name] =
                    new Release(
                        releaseName,
                        releasePath,
                        categoryName);
            }
        }

        

        private void ReadInputs()
        {
            this.io.WriteLine(Constants.SelectReleasesFolder);
            this.sourceDirectory = SystemHelper.SelectFolder();

            this.io.WriteLine(Constants.SelectFileWithLinks);
            this.linksFile = SystemHelper.SelectFile();
        }
    }
}