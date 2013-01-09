using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversalPreferences.Common;

namespace UniversalPreferences.DAL
{
    public class DataFromFile: IDataManager
    {
        private List<SimpleObject> objects = new List<SimpleObject>();
        private Dictionary<ushort, List<ushort>> classRelations = new Dictionary<ushort, List<ushort>>();
        private List<Row> rowList = new List<Row>();
        private Dictionary<string, ushort>[] attributeMappings;

        private char choice;
        public int numberOfAttributes { get; set; }
        public int numberOfClasses { get; set; }

        public void Initialize()
        {
            this.numberOfAttributes = 6;
            ChoosePreferencesType();
            CreateMappings();
            ProcessObjectsFile();
            ProcessRelationsFile(this.choice);
            CreateRows();
        }

        public void ChoosePreferencesType()
        {
            Console.WriteLine("Welcome in our application.\n\nPlease select the type of preferences you want to analyse:\n\n\t[s] - strict\n\t[n] - non-strict\n\n");
            string choice = Console.ReadLine();
            while(choice != "s" && choice != "n")
            {
                Console.WriteLine("Please type [s] or [n]");
                choice = Console.ReadLine();
            }
            this.choice = choice[0];
            
        }
        private void CreateMappings()
        {
            attributeMappings = new Dictionary<string, ushort>[7];
            for (int i = 0; i < 7; ++i)
            {
                attributeMappings[i] = new Dictionary<string, ushort>();
            }
            attributeMappings[0].Add("low", 1);
            attributeMappings[0].Add("med", 2);
            attributeMappings[0].Add("high", 3);
            attributeMappings[0].Add("vhigh", 4);
            attributeMappings[1].Add("low", 1);
            attributeMappings[1].Add("med", 2);
            attributeMappings[1].Add("high", 3);
            attributeMappings[1].Add("vhigh", 4);
            attributeMappings[2].Add("2", 2);
            attributeMappings[2].Add("3", 3);
            attributeMappings[2].Add("4", 4);
            attributeMappings[2].Add("5more", 5);
            attributeMappings[3].Add("2", 2);
            attributeMappings[3].Add("4", 4);
            attributeMappings[3].Add("more", 5);
            attributeMappings[4].Add("small", 1);
            attributeMappings[4].Add("med", 2);
            attributeMappings[4].Add("big", 3);
            attributeMappings[5].Add("low", 1);
            attributeMappings[5].Add("med", 2);
            attributeMappings[5].Add("high", 3);
            attributeMappings[6].Add("unacc", 1); 
            attributeMappings[6].Add("acc", 2);
            attributeMappings[6].Add("good", 3);
            attributeMappings[6].Add("vgood", 4);
        }

        private void ProcessObjectsFile()
        {
            string line;

            System.IO.StreamReader file =
               new System.IO.StreamReader(@"C:\Documents and Settings\Bartek\Moje dokumenty\MED\UniversalPreferences\cardatashort.txt");
            while ((line = file.ReadLine()) != null)
            {
                objects.Add(new SimpleObject(line, this));
            }

            file.Close();
            Console.WriteLine("Finished processing objects file");
        }

        private void ProcessRelationsFile(char choice)
        {
            string line;
            string[] parts;
            System.IO.StreamReader file =
               new System.IO.StreamReader(@"C:\Documents and Settings\Bartek\Moje dokumenty\MED\UniversalPreferences\relations.txt");
            if ((line = file.ReadLine()) != null)
            {
                parts = line.Split('<');
                this.numberOfClasses = parts.Length;
                for (int i = 0; i < this.numberOfClasses; ++i)
                {
                    ushort classNumber = GetBGMappings(numberOfAttributes)[parts[i]];
                    classRelations.Add(classNumber, new List<ushort>());
                    int diff = 0;
                    if (choice == 's')
                        diff++;
                    for (int j = i+diff; j < numberOfClasses; ++j)
                    {
                        ushort superiorClassNumber = GetBGMappings(numberOfAttributes)[parts[j]];
                        classRelations[classNumber].Add(superiorClassNumber);
                    }
                }
            }   
            
            file.Close();

            Console.WriteLine("Finished processing relations file");

        }

        private void CreateRows()
        {
            for (int i = 0; i < this.objects.Count; ++i)
            {
                for (int j = 0; j < this.objects.Count; ++j)
                {
                    ushort[] attributes = new ushort[numberOfAttributes << 1];
                    for (int k = 0; k < numberOfAttributes; ++k)
                    {
                        attributes[k] = objects[i].Attributes[k];
                    }
                    for (int l = numberOfAttributes; l < numberOfAttributes << 1; ++l)
                    {
                        attributes[l] = objects[j].Attributes[l - numberOfAttributes];
                    }

                    bool contains = classRelations[objects[i].ClassNumber].Contains(objects[j].ClassNumber);
                    Relation value;
                    if (contains)
                        value = Relation.Complied;
                    else
                        value = Relation.NotComplied;
                    rowList.Add(new Row(objects[i].ObjectID, objects[j].ObjectID, attributes, value));
                }
            }
            if (choice == 'n')
                Console.WriteLine("Finished creating non-strict relations!");
            else
                Console.WriteLine("Finished creating strict relations!");

        }

        public Dictionary<string, ushort> GetBGMappings(int attributeID)
        {
            return this.attributeMappings[attributeID];
        }

        public IList<Row> GetData()
        {
            return this.rowList;
        }

        public int MinLeftSideIndex()
        {
            return numberOfAttributes;
        }

        public Dictionary<int, string> GetMappings()
        {
            return new Dictionary<int, string>(); //TODO zwracać odpowiedni słownik
        }
    }
}
