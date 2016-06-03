namespace CsvGenerator
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using Addons;
    using Addons.Interfaces;
    using Core;
    using Core.Interfaces;
    using IO;
    using IO.Interfaces;

    public static class CsvGeneratorMain
    {
        public static NameValueCollection Settings;

        [STAThread]
        public static void Main()
        {
            Settings = ConfigurationManager.AppSettings;

            IInputOutput inputOutput = new ConsoleInOut();
            ILinkExtractor linkExtractor = new UniversalLinkExtractor();

            IAudioFileReader audioFileReader = new Mp3Reader();
            IPlaylistGenerator playlistGenerator = new StandartPlaylistGenerator(audioFileReader);
            ICsvBuilder csvBuilder = new CsvBuilder(playlistGenerator);

            if (bool.Parse(Settings["FakeDates"]))
            {
                IFakeDateGenerator fakeDateGenerator = new FakeDateGenerator();
                csvBuilder.FakeDateGenerator = fakeDateGenerator;
            }

            var engine = new App(
                inputOutput,
                linkExtractor,
                csvBuilder);

            engine.Run();
        }
    }
}
