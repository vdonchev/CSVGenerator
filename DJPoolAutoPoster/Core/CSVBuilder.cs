namespace CsvGenerator.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using Helpers;
    using Interfaces;
    using Models.Interfaces;

    public class CsvBuilder : ICsvBuilder
    {
        private const string PosterName = "autodj";
        private const string PostType = "post";
        private const string PostStatus = "publish";
        private const string Item = ",{0}";
        private const string ItemStr = ",\"{0}\"";
        private readonly string outputFilename = 
            "Export-" + $"text-{DateTime.UtcNow:yyyy-MM-dd_hh-mm-ss}";

        private readonly StringBuilder output = new StringBuilder();

        public CsvBuilder(IPlaylistGenerator playlistGenerator)
        {
            this.PlaylistGenerator = playlistGenerator;
        }

        private IPlaylistGenerator PlaylistGenerator { get; }

        public void BuildAndSave(Dictionary<string, IRelease> releases)
        {
            this.output.AppendLine(Messages.DefaultCsvHeader);

            foreach (var release in releases)
            {
                var releaseName = SystemHelper.EscapeText(release.Value.Name);
                var releasePath = release.Value.Path;
                var releaseLinks = release.Value.DownloadLinks;
                var releaseCategory = release.Value.Category;
                var releaseTags = string.Join(",", release.Value.Tags);
                var releasePlayList = 
                    SystemHelper.EscapeText(this.PlaylistGenerator.Generate(releasePath));

                // Id
                // this.output.Append(string.Format(Item, string.Empty));

                // Name
                this.output.Append(string.Format(Item, string.Empty));

                // Author
                this.output.Append(string.Format(ItemStr, PosterName));

                // Date
                this.output.Append(string.Format(Item, SystemHelper.FormatDate(DateTime.UtcNow)));

                // Type
                this.output.Append(string.Format(ItemStr, PostType));

                // Status
                this.output.Append(string.Format(ItemStr, PostStatus));

                // Title
                this.output.Append(string.Format(ItemStr, releaseName));

                // Content
                this.output.Append(string.Format(ItemStr, releasePlayList));

                // Category
                this.output.Append(string.Format(ItemStr, releaseCategory));

                // Tags
                this.output.Append(string.Format(ItemStr, releaseTags));

                if (releaseLinks.Any())
                {
                    // Download
                    this.output.Append(string.Format(ItemStr, releaseLinks.First()));

                    if (releaseLinks.Count > 1)
                    {
                        // Mirror
                        this.output.Append(string.Format(ItemStr, releaseLinks[1]));
                    }
                    else
                    {
                        this.output.Append(string.Format(Item, string.Empty));
                    }
                }
                else
                {
                    this.output.Append(string.Format(Item, string.Empty));
                }

                this.output.AppendLine();
            }

            this.SaveFile();
        }

        private void SaveFile()
        {
            var saveDialog = new SaveFileDialog
            {
                FileName = this.outputFilename,
                DefaultExt = Messages.Csv,
                Filter = Messages.CsvFileSignature
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveDialog.FileName, this.output.ToString().Trim());
            }
        }
    }
}