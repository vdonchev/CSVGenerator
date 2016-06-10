namespace CsvGenerator.Core
{
    using System;
    using System.IO;
    using Interfaces;
    using Microsoft.WindowsAPICodePack.Shell;

    public class Mp3Reader : IAudioFileReader
    {
        private const string Mp3 = ".mp3";

        public string GetMp3Duration(string filePath)
        {
            var fileExtension = Path.GetExtension(filePath);
            if (fileExtension != Mp3)
            {
                return string.Empty;
            }

            var fileInfo = ShellFile.FromFilePath(filePath);

            long ticks;
            long.TryParse(
                fileInfo.Properties.System.Media.Duration.Value.ToString(),
                out ticks);

            var duration = new TimeSpan(ticks);
            var fileDuration = $"{duration.Minutes:00}:{duration.Seconds:00}";

            return fileDuration;
        }
    }
}