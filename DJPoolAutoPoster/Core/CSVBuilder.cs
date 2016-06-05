namespace CsvGenerator.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using Addons.Interfaces;
    using Helpers;
    using Interfaces;
    using Models.Interfaces;

    public class CsvBuilder : ICsvBuilder
    {
        private readonly string posterName = CsvGenerator.Settings["PosterName"];
        private readonly string postType = CsvGenerator.Settings["PostType"];
        private readonly string postStatus = CsvGenerator.Settings["PostStatus"];
        private readonly string websiteUrl = CsvGenerator.Settings["WebsiteUrl"];
        private readonly int freeFrequency = int.Parse(CsvGenerator.Settings["FreeReleasesFrequency"]);

        private readonly StringBuilder output = new StringBuilder();

        public CsvBuilder(IPlaylistGenerator playlistGenerator)
        {
            this.PlaylistGenerator = playlistGenerator;
        }

        public IFakeDateGenerator FakeDateGenerator { get; set; }

        private IPlaylistGenerator PlaylistGenerator { get; }

        public void BuildAndSave(Dictionary<string, IRelease> releases)
        {
            if (this.FakeDateGenerator != null)
            {
                this.FakeDateGenerator.NumberOfReleases = releases.Count;
            }

            this.output.AppendLine(Constants.DefaultCsvHeader);

            var releaseIndex = 0;
            foreach (var release in releases)
            {
                releaseIndex++;

                var releaseName = SystemHelper.EscapeText(release.Value.Name);
                var releasePath = release.Value.Path;
                var releaseLinks = release.Value.DownloadLinks;
                var releaseCategory = release.Value.Category;

                if (this.freeFrequency > 0)
                {
                    if (releaseIndex % this.freeFrequency == 0)
                    {
                        release.Value.Tags.Add("free");
                    }
                }
                var releaseTags = string.Join(",", release.Value.Tags);

                var releasePlayList =
                    SystemHelper.EscapeText(this.PlaylistGenerator.Generate(releasePath));

                // Name
                this.output.Append(string.Format(Constants.Item, string.Empty));

                // Author
                this.output.Append(string.Format(Constants.ItemStr, this.posterName));

                // Date

                if (this.FakeDateGenerator != null)
                {
                    this.output.Append(
                        string.Format(
                            Constants.Item,
                            SystemHelper.FormatDate(
                                this.FakeDateGenerator.Next())));
                }
                else
                {
                    this.output.Append(string.Format(Constants.Item, SystemHelper.FormatDate(DateTime.UtcNow)));
                }

                // Type
                this.output.Append(string.Format(Constants.ItemStr, this.postType));

                // Status
                this.output.Append(string.Format(Constants.ItemStr, this.postStatus));

                // Title
                this.output.Append(string.Format(Constants.ItemStr, releaseName));

                // Content
                this.output.Append(string.Format(Constants.ItemStr, releasePlayList));

                // Category
                this.output.Append(string.Format(Constants.ItemStr, releaseCategory));

                // Tags
                this.output.Append(string.Format(Constants.ItemStr, releaseTags));

                this.BuildReleaseLinks(releaseLinks);

                this.output.AppendLine();
            }

            this.SaveFile();
        }

        private void BuildReleaseLinks(List<string> releaseLinks)
        {
            var orderedLinks = new string[3];
            for (int i = 0; i < releaseLinks.Count; i++)
            {
                if (releaseLinks[i].Contains(this.websiteUrl))
                {
                    orderedLinks[0] = string.Format(Constants.ItemStr, releaseLinks[i]);
                }
                else
                {
                    if (orderedLinks[1] == null)
                    {
                        orderedLinks[1] = string.Format(Constants.ItemStr, releaseLinks[i]);
                    }
                    else
                    {
                        orderedLinks[2] = string.Format(Constants.ItemStr, releaseLinks[i]);
                    }
                }
            }

            for (int i = 0; i < orderedLinks.Length; i++)
            {
                if (orderedLinks[i] != null)
                {
                    this.output.Append(orderedLinks[i]);
                }
                else
                {
                    this.output.Append(string.Format(Constants.Item, string.Empty));
                }
            }
        }

        private void SaveFile()
        {
            var saveDialog = new SaveFileDialog
            {
                FileName = string.Format(Constants.DestinationFilename, DateTime.UtcNow),
                DefaultExt = Constants.Csv,
                Filter = Constants.CsvFileSignature
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveDialog.FileName, this.output.ToString().Trim());
            }
        }
    }
}