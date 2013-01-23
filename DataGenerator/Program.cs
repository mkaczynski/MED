using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataGenerator
{
    public class Program
    {
        public static void Main(string[] argv)
        {
            var commandLineParser = new CommandLineParser(argv);
            commandLineParser.Parse();
            if(!commandLineParser.ValidArguments)
            {
                PrintUsage();
                return;
            }
            var generator = new Generator(commandLineParser);
            generator.Generate();
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Błędna linia argumentów, prawidłowe użycie:");
            Console.WriteLine("    DataGenerator inFileName n outFileName");
            Console.WriteLine();
            Console.WriteLine("    inFileName - nazwa pliku wejsćiowego");
            Console.WriteLine("    n - liczba linii, która powinna znaleźć się w pliku wyjściowym");
            Console.WriteLine("    outFileName - nazwa pliku wyjściowego");
        }
    }
}
