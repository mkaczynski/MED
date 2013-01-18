using System.Collections.Generic;
using System.Linq;
using UniversalPreferences.Common;
using UniversalPreferences.HashTree;

namespace UniversalPreferences.Algorithm
{
    public class FakeHashTree : IHashTree
    {
        private IEnumerable<SimpleRow> rows; 

        public static bool IsItemsetSupported(ushort[] itemset, Row transaction)
        {
            var intersection = transaction.Attributes.Select(x => x).Intersect(itemset);
            var count = intersection.Count();
            return count == itemset.Length;
        }

        public void FillTree(IEnumerable<SimpleRow> elements)
        {
            rows = elements;
        }

        public IEnumerable<SimpleRow> GetSupportedSets(Row transaction)
        {
            var res = new List<SimpleRow>();

            foreach (var row in rows)
            {
                var intersection = transaction.Attributes.Intersect(row.Transaction);
                var count = intersection.Count();
                if (count == transaction.Attributes.Length)
                {
                    res.Add(row);
                }
            }

            return res;
        }

        public IEnumerable<SimpleRow> GetRows()
        {
            return rows;
        }

        public void FillTree(List<ushort[]> elements)
        {
            throw new System.NotImplementedException();
        }
    }
}