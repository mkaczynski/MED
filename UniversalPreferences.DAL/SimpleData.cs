using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.DAL
{
    // dane z artkulu, do testow
    // 1: bergstrasse, 2: Reichenbachstr., 3: Klinikum, 4: Siedepunkt
    public class SimpleData
    {
        public IList<Row> GetData()
        {
            var res = new List<Row>();

            res.Add(new Row { MainObjectId = 1, SecondObjectId = 1, 
                Attributes = new[] { true, true, false, true, true, true, false, true}, 
                FirstSecondObjectAtribute = 4, Value = Relation.Complied
            });

            res.Add(new Row { MainObjectId = 1, SecondObjectId = 2, 
                Attributes = new[] { true, true, false, true, true, false, true, false}, 
                FirstSecondObjectAtribute = 4, Value = Relation.Complied
            });

            res.Add(new Row { MainObjectId = 1, SecondObjectId = 3, 
                Attributes = new[] { true, true, false, true, true, true, false, false}, 
                FirstSecondObjectAtribute = 4, Value = Relation.NotComplied
            });

            res.Add(new Row { MainObjectId = 1, SecondObjectId = 4, 
                Attributes = new[] { true, true, false, true, true, false, false, false}, 
                FirstSecondObjectAtribute = 4, Value = Relation.Complied
            });

            res.Add(new Row { MainObjectId = 2, SecondObjectId = 1, 
                Attributes = new[] { true, false, true, false, true, true, false, true}, 
                FirstSecondObjectAtribute = 4, Value = Relation.NotComplied
            });

            res.Add(new Row { MainObjectId = 2, SecondObjectId = 2, 
                Attributes = new[] { true, false, true, false, true, false, true, false}, 
                FirstSecondObjectAtribute = 4, Value = Relation.Complied
            });

            res.Add(new Row { MainObjectId = 2, SecondObjectId = 3, 
                Attributes = new[] { true, false, true, false, true, true, false, false}, 
                FirstSecondObjectAtribute = 4, Value = Relation.NotComplied
            });

            res.Add(new Row { MainObjectId = 2, SecondObjectId = 4, 
                Attributes = new[] { true, false, true, false, true, false, false, false}, 
                FirstSecondObjectAtribute = 4, Value = Relation.Complied
            });

            res.Add(new Row { MainObjectId = 3, SecondObjectId = 1, 
                Attributes = new[] { true, true, false, false, true, true, false, true}, 
                FirstSecondObjectAtribute = 4, Value = Relation.NotComplied
            });

            res.Add(new Row { MainObjectId = 3, SecondObjectId = 2, 
                Attributes = new[] { true, true, false, false, true, false, true, false}, 
                FirstSecondObjectAtribute = 4, Value = Relation.NotComplied
            });

            res.Add(new Row { MainObjectId = 3, SecondObjectId = 3, 
                Attributes = new[] { true, true, false, false, true, true, false, false}, 
                FirstSecondObjectAtribute = 4, Value = Relation.Complied
            });

            res.Add(new Row { MainObjectId = 3, SecondObjectId = 4, 
                Attributes = new[] { true, true, false, false, true, false, false, false}, 
                FirstSecondObjectAtribute = 4, Value = Relation.Complied
            });

            res.Add(new Row { MainObjectId = 4, SecondObjectId = 1, 
                Attributes = new[] { true, false, false, false, true, true, false, true}, 
                FirstSecondObjectAtribute = 4, Value = Relation.NotComplied
            });

            res.Add(new Row { MainObjectId = 4, SecondObjectId = 2, 
                Attributes = new[] { true, false, false, false, true, false, true, false}, 
                FirstSecondObjectAtribute = 4, Value = Relation.NotComplied
            });

            res.Add(new Row { MainObjectId = 4, SecondObjectId = 3, 
                Attributes = new[] { true, false, false, false, true, true, false, false}, 
                FirstSecondObjectAtribute = 4, Value = Relation.NotComplied
            });

            res.Add(new Row { MainObjectId = 4, SecondObjectId = 4, 
                Attributes = new[] { true, false, false, false, true, false, false, false}, 
                FirstSecondObjectAtribute = 4, Value = Relation.Complied
            });

            return res;
        }
    }
}