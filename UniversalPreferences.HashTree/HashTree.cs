using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.HashTree
{
    public class HashTree : IHashTree
    {
        private readonly Node root;
        private readonly IList<SimpleRow> allRows; 

        internal HashTree(int transactionLength, int pageSize, int firstNumber)
        {
            root = new Node(pageSize, firstNumber, transactionLength, 0);
            allRows = new List<SimpleRow>();
        }

        public void FillTree(IEnumerable<SimpleRow> elements)
        {
            foreach (var simpleRow in elements)
            {
                root.Add(simpleRow);
                allRows.Add(simpleRow);
            }
        }

        public IEnumerable<SimpleRow> GetSupportedSets(Row transaction)
        {
            var supportedRows = new List<SimpleRow>();
            root.FillSupportedRows(supportedRows, transaction.Attributes, 0, transaction.HashSet);
            return supportedRows;
        }

        public IEnumerable<SimpleRow> GetRows()
        {
            return allRows;
        }
    }
}