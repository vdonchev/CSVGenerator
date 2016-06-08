namespace CsvGenerator.Helpers
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
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

        public static string ToAlphaNumberc(string text)
        {
            var res = new string(
                text
                    .Where(ch => char.IsLetterOrDigit(ch) ||
                                 char.IsWhiteSpace(ch) ||
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

            var work = releaseFolderName.ToLower();

            work = Regex.Replace(work, @"va|v.a.", "", RegexOptions.IgnoreCase);
            work = Regex.Replace(work, @"vol(\s|-|_|\.)|pack(\s|-|_)|\d+|\<|\(|\[", "<");
            work = work.Trim('-', '_', ' ');
            work = work.Replace("-", "<");

            var indexOfNameEnd = work
                .IndexOfAny(new[] { '<', '(', '[', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });

            indexOfNameEnd = indexOfNameEnd < 0
                ? releaseFolderName.Length - 1
                : indexOfNameEnd;

            var category = work.Substring(0, indexOfNameEnd);
            if (category == string.Empty)
            {
                category = "Other";
            }

            var textInfo = new CultureInfo("en-US", false).TextInfo;
            
            return textInfo.ToTitleCase(category.Trim('-', '_', ' '));

        }
    }
}