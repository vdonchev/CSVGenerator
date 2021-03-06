﻿namespace CsvGenerator.Helpers
{
    public static class Constants
    {
        // Messages
        public const string SelectReleasesFolder = "Select releases folder";
        public const string SelectFileWithLinks = "Select links file";
        public const string AllDone = "All done! - Press any key to continue!";
        public const string Building = "Building csv...";

        // App specific
        public const string CsvFileSignature = "Comma separated values file (.csv)|*.csv";
        public const string Csv = ".csv";
        public const string DefaultCsvHeader = @"""post_id"",""post_name"",""post_author"",""post_date"",""post_type"",""post_status"",""post_title"",""post_content"",""post_category"",""post_tags"",""torrent"",""download"",""mirror""";

        // Errors
        public const string DupeFolder = "Duplicate fodler name";
        public const string InvalidFolderPath = "Invalid folder path";
        public const string InvalidFilePath = "Invalid file path";
        public const string FakeDateNotSet = "Fake date start is not set.";

        // String templates
        public const string Item = ",{0}";
        public const string ItemStr = @",""{0}""";
        public const string DestinationFilename = @"Export-{0:yyyy-MM-dd_hh-mm-ss}";
    }
}