using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.HashTree
{
    public class HashTree : IHashTree
    {
        private readonly Node root;

        internal HashTree(int transactionLength, int pageSize, int firstNumber)
        {
            root = new Node(pageSize, firstNumber, transactionLength, 0);
        }

        public void FillTree(IEnumerable<Row> elements)
        {
            foreach (var simpleRow in elements)
            {
                root.Add(simpleRow);
            }
        }

        public IEnumerable<Row> GetSupportedSets(Row transaction)
        {
            var supportedRows = new List<Row>();
            root.FillSupportedRows(supportedRows, transaction.Attributes, 0);
            return supportedRows;
        }
    }
}