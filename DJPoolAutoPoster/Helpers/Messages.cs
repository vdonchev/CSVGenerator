namespace CsvGenerator.Helpers
{
    public static class Messages
    {
        // Messages
        public const string SelectReleasesFolder = "Select releases folder";
        public const string SelectFileWithLinks = "Select links file";
        public const string AllDone = "All done!";

        // App specific
        public const string CsvFileSignature = "Comma separated values file (.csv)|*.csv";
        public const string Csv = ".csv";
        public const string DefaultCsvHeader = @"""post_id"",""post_name"",""post_author"",""post_date"",""post_type"",""post_status"",""post_title"",""post_content"",""post_category"",""post_tags"",""download"",""mirror""";

        // Errors
        public const string DupeFolder = "Duplicate fodler name";
        public const string InvalidFolderPath = "Invalid folder path";
        public const string InvalidFilePath = "Invalid file path";
        public const string FakeDateNotSet = "Fake date start is not set.";
    }
}