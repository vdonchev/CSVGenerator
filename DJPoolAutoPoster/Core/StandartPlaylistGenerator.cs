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

            var files = Directory.GetFiles(releasePath);
            foreach (var file in files)
            {
                var cleanFileName = Path.GetFileNameWithoutExtension(file);
                var duration = this.audioFileReader.GetMp3Duration(file);

                if (duration != string.Empty)
                {
                    result.AppendLine($"{cleanFileName} ({duration})");
                }
                else
                {
                    result.AppendLine($"{cleanFileName}");
                }
            }

            return result.ToString().Trim();
        }
    }
}