using System;

namespace Playground
{
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    class Program
    {
        static void Main(string[] args)
        {
            
        }

        private static void GenerateMd5FromString()
        {
            var path = @"D:\MY WORKTABLE\ICONS\SOCIAL-ICONS";
            var dirName = new DirectoryInfo(path).Name;

            // byte array representation of that string
            byte[] encodedPassword = new UTF8Encoding().GetBytes(dirName);

            // need MD5 to calculate the hash
            var hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            // string representation (similar to UNIX format)
            var encoded = BitConverter.ToString(hash)
               .Replace("-", string.Empty)
               .ToLower();

            Console.WriteLine(encoded);
        }

        private static void GenerateFakeDates()
        {
            // Generate dates
            var numberOfDates = 500;

            var now = DateTime.Now;
            var start = new DateTime(2016, 01, 01);

            var secAvail = (long)(now - start).TotalMinutes / numberOfDates;
            var next = start;
            for (var i = 0; i < numberOfDates; i++)
            {
                Console.WriteLine(next);
                next = next.AddMinutes(secAvail);
            }
        }
    }
}
