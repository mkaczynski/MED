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

        public void FillTree(IEnumerable<SimpleRow> elements)
        {
            foreach (var simpleRow in elements)
            {
                root.Add(simpleRow);
            }
        }

        public IEnumerable<SimpleRow> GetSupportedSets(SimpleRow transaction)
        {
            var supportedRows = new List<SimpleRow>();
            root.FillSupportedRows(supportedRows, transaction.Transaction, 0);
            return supportedRows;
        }
    }
}