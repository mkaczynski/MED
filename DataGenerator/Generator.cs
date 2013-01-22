using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataGenerator
{
    public class Generator
    {
        private readonly string inFileName;
        private readonly string outFileName;
        private readonly int linesCount;
        private readonly Random random;

        public Generator(CommandLineParser commandLineParser)
        {
            inFileName = commandLineParser.InFileName;
            outFileName = commandLineParser.OutFileName;
            linesCount = commandLineParser.LinesCount;
            random = new Random();
        }

        public void Generate()
        {
            var allLines = File.ReadAllLines(inFileName);

            string[] linesToWrite;
            if(allLines.Length <= linesCount)
            {
                linesToWrite = allLines;
            }
            else
            {
                linesToWrite = GenerateRandomLines(allLines.ToList());
            }
            File.WriteAllLines(outFileName, linesToWrite);
        }

        private string[] GenerateRandomLines(IList<string> allLines)
        {
            var retLines = new string[linesCount];
            for (int i = 0; i < linesCount; i++)
            {
                int r = random.Next(allLines.Count);
                var line = allLines[r];
                allLines.RemoveAt(r);
                retLines[i] = line;
            }
            return retLines;
        }
    }
}