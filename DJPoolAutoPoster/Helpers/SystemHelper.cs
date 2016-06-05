namespace CsvGenerator.Helpers
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
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
               .Replace("-", String.Empty)
               .ToLower();

            return md5;
        }

        public static string ToAlphaNumberc(string text)
        {
            var res = new string(
                text
                    .Where(ch => Char.IsLetterOrDigit(ch) ||
                                 Char.IsWhiteSpace(ch) ||
                                 ch == '_' ||
                                 ch == '-').ToArray());

            res = res.Replace(' ', '_').ToLower();

            return res;
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

        public static string ExtractCategoryFromRelease(string releasePath)
        {
            var releaseFolderName = SystemHelper.ExtractFolderName(releasePath);

            var indexOfNameEnd = releaseFolderName
                .ToLower()
                .Replace(" vol", "<")
                .Replace(" pack ", "<")
                .IndexOfAny(new[] { '<', '(', '[' });

            indexOfNameEnd = indexOfNameEnd < 0
                ? releaseFolderName.Length - 1
                : indexOfNameEnd;

            var category = releaseFolderName.Substring(0, indexOfNameEnd);
            if (category == String.Empty)
            {
                category = "Other";
            }

            return category;
        }
    }
}