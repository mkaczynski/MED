using System.Collections.Generic;
using UniversalPreferences.Common;
using System;

namespace UniversalPreferences.DAL
{
    // dane z artkulu, do testow
    // 1: bergstrasse, 2: Reichenbachstr., 3: Klinikum, 4: Siedepunkt
    public class SimpleData : IDataManager
    {
        private List<SimpleObject> objects = new List<SimpleObject>();
        private Dictionary<ushort, List<ushort>> classRelations = new Dictionary<ushort, List<ushort>>();
        private List<Row> rowList = new List<Row>();
        private Dictionary<string, ushort>[] attributeMappings;
        public void Initialize()
        {
            CreateMappings();
            ProcessObjectsFile();
            ProcessRelationsFile();
            CreateRows();
        }

        private void CreateMappings()
        {
            attributeMappings = new Dictionary<string, ushort>[SimpleObject.numberOfAttributes+1];
            for (int i = 0; i < SimpleObject.numberOfAttributes+1; ++i)
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
            attributeMappings[6].Add("unacc", 4); //klasa 4 najgorsza, klasa 1 najlepsza
            attributeMappings[6].Add("acc", 3);
            attributeMappings[6].Add("good", 2);
            attributeMappings[6].Add("vgood", 1);
            attributeMappings[6].Add("?", 0);   //to jest klasa lepsza od vgood, czyli tak jakby zbiór pusty
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

        private void ProcessRelationsFile()
        {
            string line;

            System.IO.StreamReader file =
               new System.IO.StreamReader(@"C:\Documents and Settings\Bartek\Moje dokumenty\MED\UniversalPreferences\relationsNotStrict.txt");
            while ((line = file.ReadLine()) != null)
            {
                string[] parts = line.Split('<');
                if (parts.Length == 2)
                {
                    ushort classNumber = GetBGMappings(SimpleObject.numberOfAttributes)[parts[0]];
                    ushort superiorClassNumber = GetBGMappings(SimpleObject.numberOfAttributes)[parts[1]];
                    if (!classRelations.ContainsKey(classNumber))
                        classRelations.Add(classNumber, new List<ushort>());
                    classRelations[classNumber].Add(superiorClassNumber);
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
                    ushort[] attributes = new ushort[SimpleObject.numberOfAttributes << 1];
                    for(int k = 0; k < SimpleObject.numberOfAttributes; ++k)
                    {
                        attributes[k] = objects[i].Attributes[k];
                    }
                    for(int l = SimpleObject.numberOfAttributes; l < SimpleObject.numberOfAttributes << 1; ++l)
                    {
                        attributes[l] = objects[j].Attributes[l-SimpleObject.numberOfAttributes];
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
            Console.WriteLine("Finished creating non-strict relations!");
            //Console.ReadLine();
        }

        public IList<Row> GetData()
        {
            var res = new List<Row>();

            res.Add(new Row
            {
                MainObjectId = 1,
                SecondObjectId = 1,
                Attributes = new ushort[] { 1, 2, 4, 5, 6, 8 },
                Value = Relation.Complied
            });

            res.Add(new Row
            {
                MainObjectId = 1,
                SecondObjectId = 2,
                Attributes = new ushort[] { 1, 2, 4, 5, 7 },
                Value = Relation.Complied
            });

            res.Add(new Row
            {
                MainObjectId = 1,
                SecondObjectId = 3,
                Attributes = new ushort[] { 1, 2, 4, 5, 6 },
                Value = Relation.NotComplied
            });

            res.Add(new Row
            {
                MainObjectId = 1,
                SecondObjectId = 4,
                Attributes = new ushort[] { 1, 2, 4, 5 },
                Value = Relation.Complied
            });

            res.Add(new Row
            {
                MainObjectId = 2,
                SecondObjectId = 1,
                Attributes = new ushort[] { 1, 3, 5, 6, 8 },
                Value = Relation.NotComplied
            });

            res.Add(new Row
            {
                MainObjectId = 2,
                SecondObjectId = 2,
                Attributes = new ushort[] { 1, 3, 5, 7 },
                Value = Relation.Complied
            });

            res.Add(new Row
            {
                MainObjectId = 2,
                SecondObjectId = 3,
                Attributes = new ushort[] { 1, 3, 5, 6 },
                Value = Relation.NotComplied
            });

            res.Add(new Row
            {
                MainObjectId = 2,
                SecondObjectId = 4,
                Attributes = new ushort[] { 1, 3, 5 },
                Value = Relation.Complied
            });

            res.Add(new Row
            {
                MainObjectId = 3,
                SecondObjectId = 1,
                Attributes = new ushort[] { 1, 2, 5, 6, 8 },
                Value = Relation.NotComplied
            });

            res.Add(new Row
            {
                MainObjectId = 3,
                SecondObjectId = 2,
                Attributes = new ushort[] { 1, 2, 5, 7 },
                Value = Relation.NotComplied
            });

            res.Add(new Row
            {
                MainObjectId = 3,
                SecondObjectId = 3,
                Attributes = new ushort[] { 1, 2, 5, 6 },
                Value = Relation.Complied
            });

            res.Add(new Row
            {
                MainObjectId = 3,
                SecondObjectId = 4,
                Attributes = new ushort[] { 1, 2, 5 },
                Value = Relation.Complied
            });

            res.Add(new Row
            {
                MainObjectId = 4,
                SecondObjectId = 1,
                Attributes = new ushort[] { 1, 5, 6, 8 },
                Value = Relation.NotComplied
            });

            res.Add(new Row
            {
                MainObjectId = 4,
                SecondObjectId = 2,
                Attributes = new ushort[] { 1, 5, 7 },
                Value = Relation.NotComplied
            });

            res.Add(new Row
            {
                MainObjectId = 4,
                SecondObjectId = 3,
                Attributes = new ushort[] { 1, 5, 6 },
                Value = Relation.NotComplied
            });

            res.Add(new Row
            {
                MainObjectId = 4,
                SecondObjectId = 4,
                Attributes = new ushort[] { 1, 5 },
                Value = Relation.Complied
            });

            return res;
        }

        public IList<Row> GetBGData()
        {
            return this.rowList;
        }

        public Dictionary<int, string> GetMappings()
        {
            var dict = 
                new Dictionary<int, string>
                    {
                        {1, "vegetarian meal"},
                        {2, "non-vegetarian meal without pork"},
                        {3, "meal containing alcohol"},
                        {4, "other meal"},
                        {5, "vegetarian meal"},
                        {6, "non-vegetarian meal without pork"},
                        {7, "meal containing alcohol"},
                        {8, "other meal"}
                    };
            
            return dict;

        }

        public Dictionary<string, ushort> GetBGMappings(int attributeID)
        {
            return this.attributeMappings[attributeID];
        }

        public int MinLeftSideIndex()
        {
            return 5;
        }
    }
}