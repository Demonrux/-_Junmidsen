using Junmidsen.Core;
using System;

namespace Junmidsen.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Task 1 
            string original = "aaabbcccdde";
            string compressed = StringCompressor.Compress(original);
            string decompressed = StringCompressor.Decompress(compressed);
            Console.WriteLine($"Original:  {original}");
            Console.WriteLine($"Compressed: {compressed}");
            Console.WriteLine($"Decompressed: {decompressed}");
            Console.WriteLine($"Success: {original == decompressed}\n");

            // Task 2 
            Console.WriteLine($"Initial count: {Server.GetCount()}");
            Server.AddToCount(10);
            Console.WriteLine($"After +10: {Server.GetCount()}");


            // Task 3 
            string inputFile = "input.log";
            string outputFile = "output.log";
            string problemsFile = "problems.txt";

            if (!File.Exists(inputFile))
            {
                File.WriteAllText(inputFile,
                    "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'\n" +
                    "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'\n" +
                    "invalid line here");
            }

            var processor = new LogProcessor(inputFile, outputFile, problemsFile);
            processor.Process();

            Console.WriteLine($"Log processing completed. Output: {outputFile}, Problems: {problemsFile}");
        }
    }
}