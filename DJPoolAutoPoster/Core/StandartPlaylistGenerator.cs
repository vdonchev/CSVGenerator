namespace CsvGenerator.Core
{
    using System.IO;
    using System.Text;
    using Interfaces;

    public class StandartPlaylistGenerator : IPlaylistGenerator
    {
        private readonly IAudioFileReader audioFileReader;

        public StandartPlaylistGenerator(IAudioFileReader audioFileReader)
        {
            this.audioFileReader = audioFileReader;
        }

        public string Generate(string releasePath)
        {
            var result = new StringBuilder();
            this.ReadPath(releasePath, result);
            return result.ToString().Trim();
        }

        private void ReadPath(string path, StringBuilder result)
        {
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var cleanFileName = Path.GetFileNameWithoutExtension(file);
                var duration = this.audioFileReader.GetMp3Duration(file);

                if (duration != string.Empty)
                {
                    result.AppendLine($"{cleanFileName} ({duration})");
                }
            }

            var subFolders = Directory.GetDirectories(path);
            foreach (var subFolder in subFolders)
            {
                this.ReadPath(subFolder, result);
            }
        }
    }
}