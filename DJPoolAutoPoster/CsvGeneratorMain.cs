namespace CsvGenerator
{
    using System;
    using Core;
    using Core.Interfaces;
    using IO;
    using IO.Interfaces;

    public static class CsvGeneratorMain
    {
        [STAThread]
        public static void Main()
        {
            IInputOutput inputOutput = new ConsoleInOut();
            ILinkExtractor linkExtractor = new UniversalLinkExtractor();

            IAudioFileReader audioFileReader = new Mp3Reader();
            IPlaylistGenerator playlistGenerator = new StandartPlaylistGenerator(audioFileReader);
            ICsvBuilder csvBuilder = new CsvBuilder(playlistGenerator);

            var engine = new App(
                inputOutput,
                linkExtractor,
                csvBuilder);

            engine.Run();
        }
    }
}
