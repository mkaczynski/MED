using System.Collections.Generic;
using System.Linq;
using UniversalPreferences.Common;
using UniversalPreferences.HashTree;

namespace UniversalPreferences.Algorithm
{
    public class FakeHashTree : IHashTree
    {
        private IEnumerable<Row> rows; 

        public static bool IsItemsetSupported(ushort[] itemset, Row transaction)
        {
            var intersection = transaction.Attributes.Select(x => x).Intersect(itemset);
            var count = intersection.Count();
            return count == itemset.Length;
        }

        public void FillTree(IEnumerable<Row> elements)
        {
            rows = elements;
        }

        public IEnumerable<Row> GetSupportedSets(Row transaction)
        {
            var res = new List<Row>();

            foreach (var row in rows)
            {
                var intersection = transaction.Attributes.Intersect(row.Attributes);
                var count = intersection.Count();
                if (count == transaction.Attributes.Length)
                {
                    res.Add(row);
                }
            }

            return res;
        }
    }
}