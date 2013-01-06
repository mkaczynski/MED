using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.DAL
{
    // dane z artkulu, do testow
    // 1: bergstrasse, 2: Reichenbachstr., 3: Klinikum, 4: Siedepunkt
    public class SimpleData : IDataManager
    {
        public void Initialize()
        {
            //nie ma nic do robienia
        }

        public IList<Row> GetData()
        {
            var res = new List<Row>();

            res.Add(new Row { MainObjectId = 1, SecondObjectId = 1,
                Attributes = new ushort[] { 1, 2, 4, 5, 6, 8 }, 
                FirstSecondObjectAtribute = 4, Value = Relation.Complied
            });

            res.Add(new Row { MainObjectId = 1, SecondObjectId = 2, 
                Attributes = new ushort[] { 1, 2, 4, 5, 7}, 
                FirstSecondObjectAtribute = 4, Value = Relation.Complied
            });

            res.Add(new Row { MainObjectId = 1, SecondObjectId = 3,
                Attributes = new ushort[] { 1, 2, 4, 5, 6 }, 
                FirstSecondObjectAtribute = 4, Value = Relation.NotComplied
            });

            res.Add(new Row { MainObjectId = 1, SecondObjectId = 4, 
                Attributes = new ushort[] { 1, 2, 4, 5 }, 
                FirstSecondObjectAtribute = 4, Value = Relation.Complied
            });

            res.Add(new Row { MainObjectId = 2, SecondObjectId = 1, 
                Attributes = new ushort[] { 1, 3, 5, 6, 8}, 
                FirstSecondObjectAtribute = 4, Value = Relation.NotComplied
            });

            res.Add(new Row { MainObjectId = 2, SecondObjectId = 2, 
                Attributes = new ushort[] { 1, 3, 5, 7 }, 
                FirstSecondObjectAtribute = 4, Value = Relation.Complied
            });

            res.Add(new Row { MainObjectId = 2, SecondObjectId = 3, 
                Attributes = new ushort[] { 1, 3, 5, 6 }, 
                FirstSecondObjectAtribute = 4, Value = Relation.NotComplied
            });

            res.Add(new Row { MainObjectId = 2, SecondObjectId = 4,
                Attributes = new ushort[] { 1, 3, 5 }, 
                FirstSecondObjectAtribute = 4, Value = Relation.Complied
            });

            res.Add(new Row { MainObjectId = 3, SecondObjectId = 1, 
                Attributes = new ushort[] { 1, 2, 5, 6, 8}, 
                FirstSecondObjectAtribute = 4, Value = Relation.NotComplied
            });

            res.Add(new Row { MainObjectId = 3, SecondObjectId = 2, 
                Attributes = new ushort[] { 1, 2, 5, 7 }, 
                FirstSecondObjectAtribute = 4, Value = Relation.NotComplied
            });

            res.Add(new Row { MainObjectId = 3, SecondObjectId = 3, 
                Attributes = new ushort[] { 1, 2, 5, 6 }, 
                FirstSecondObjectAtribute = 4, Value = Relation.Complied
            });

            res.Add(new Row { MainObjectId = 3, SecondObjectId = 4, 
                Attributes = new ushort[] { 1, 2, 5 }, 
                FirstSecondObjectAtribute = 4, Value = Relation.Complied
            });

            res.Add(new Row { MainObjectId = 4, SecondObjectId = 1, 
                Attributes = new ushort[] { 1, 5, 6, 8}, 
                FirstSecondObjectAtribute = 4, Value = Relation.NotComplied
            });

            res.Add(new Row { MainObjectId = 4, SecondObjectId = 2, 
                Attributes = new ushort[] { 1, 5, 7 }, 
                FirstSecondObjectAtribute = 4, Value = Relation.NotComplied
            });

            res.Add(new Row { MainObjectId = 4, SecondObjectId = 3, 
                Attributes = new ushort[] { 1, 5, 6 }, 
                FirstSecondObjectAtribute = 4, Value = Relation.NotComplied
            });

            res.Add(new Row { MainObjectId = 4, SecondObjectId = 4, 
                Attributes = new ushort[] { 1, 5 }, 
                FirstSecondObjectAtribute = 4, Value = Relation.Complied
            });

            return res;
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

        public int MinLeftSideIndex()
        {
            return 5;
        }
    }
}