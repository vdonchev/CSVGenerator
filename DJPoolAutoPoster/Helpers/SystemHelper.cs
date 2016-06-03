namespace CsvGenerator.Helpers
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows.Forms;
    using Exceptions;

    public static class SystemHelper
    {
        public static string SelectFolder()
        {
            var targetFolder = new FolderBrowserDialog();
            if (targetFolder.ShowDialog() != DialogResult.OK)
            {
                throw new CsvGeneratorException(Constants.InvalidFolderPath);
            }

            return targetFolder.SelectedPath;
        }

        public static string SelectFile()
        {
            var targetFolder = new OpenFileDialog();
            if (targetFolder.ShowDialog() != DialogResult.OK)
            {
                throw new CsvGeneratorException(Constants.InvalidFilePath);
            }

            return targetFolder.FileName;
        }

        public static string NameToMd5(string dirName)
        {
            var encodedPassword = new UTF8Encoding().GetBytes(dirName);
            var hash = ((HashAlgorithm)CryptoConfig
                .CreateFromName("MD5"))
                .ComputeHash(encodedPassword);

            var md5 = BitConverter.ToString(hash)
               .Replace("-", string.Empty)
               .ToLower();

            return md5;
        }

        public static string ExtractFolderName(string path)
        {
            var dirName = new DirectoryInfo(path).Name;

            return dirName;
        }

        public static string EscapeText(string text)
        {
            return text.Replace(@"""", @"\""");
        }

        public static string FormatDate(DateTime date)
        {
            return date.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
        }
    }
}