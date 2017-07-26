using System;
using System.Collections.Generic;
using System.IO;

namespace ConcurrencyModeFixer
{
    internal class Program
    {
        private static readonly Dictionary<string, string> replacements = new Dictionary<string, string>
        {
            {
                "<Property Name=\"rowVersion\" Type=\"Binary\" MaxLength=\"8\" FixedLength=\"true\" annotation:StoreGeneratedPattern=\"Computed\" />",
                "<Property Name=\"rowVersion\" Type=\"Binary\" MaxLength=\"8\" FixedLength=\"true\" annotation:StoreGeneratedPattern=\"Computed\" ConcurrencyMode=\"Fixed\" />"
            }
        };

        private static void Main(string[] args)
        {
            const bool turnConcurrencyOn = false;

            // find all .edmx
            var directoryPath = "G:\\Git\\drs-backend-phase1\\drs-backend-phase1\\Models";
            foreach (var file in Directory.GetFiles(directoryPath))
            {
                // only edmx
                if (!file.EndsWith(".edmx"))
                    continue;

                Console.WriteLine("File Name Found : " + file);

                // read file
                var fileContents = File.ReadAllText(file);

                if (turnConcurrencyOn)
                {
                    // replace lines
                    foreach (var item in replacements)
                        fileContents = fileContents.Replace(item.Key, item.Value);
                }
                else
                {
                    foreach (var item in replacements)
                        fileContents = fileContents.Replace(item.Value, item.Key);
                }
                // overwite file
                File.WriteAllText(file, fileContents);

                Console.WriteLine("\nFile : " + file + "Changed");
            }
        }
    }
}