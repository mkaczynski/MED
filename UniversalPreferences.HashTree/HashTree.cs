using System.Collections.Generic;
using System.Linq;
using UniversalPreferences.Common;

namespace UniversalPreferences.HashTree
{
    public class HashTree : IHashTree
    {
        private const int PageSize = 2;
        private const int FirstNumber = 3;
        private readonly Node root;

        public HashTree(int transactionLength)
        {
            root = new Node(PageSize, FirstNumber, transactionLength, 0);
        }

        public void CreateTree(IEnumerable<SimpleRow> elements)
        {
            foreach (var simpleRow in elements)
            {
                root.Add(simpleRow);
            }
        }

        public IEnumerable<SimpleRow> GetSupportedSets(SimpleRow transaction)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Node
    {
        private readonly int pageSize;
        private readonly int firstNumber;
        private readonly int transactionLength;
        private readonly int depth;
        private Node[] children;
        private IList<SimpleRow> elements;

        private bool isLeafNode;

        public Node(int pageSize, int firstNumber, int transactionLength, int depth)
        {
            this.pageSize = pageSize;
            this.firstNumber = firstNumber;
            this.transactionLength = transactionLength;
            this.depth = depth;
            if (depth != 0)
                isLeafNode = true;
            if (!isLeafNode)
            {
                InitializeChildren();    
            }
            else
            {
                elements = new List<SimpleRow>();
            }
            
        }

        private void InitializeChildren()
        {
            children = new Node[firstNumber];
            for (int i = 0; i < firstNumber; i++)
            {
                children[i] = new Node(pageSize, firstNumber, transactionLength, depth + 1);
            }
        }

        public void Add(SimpleRow newRow)
        {
            if (isLeafNode)
            {
                if (CanAddElementToPage())
                {
                    elements.Add(newRow);
                }
                else
                {
                    SplitPage(newRow);    
                }
                return;
            }
            var hash = CalculateHashForElement(newRow);
            children[hash].Add(newRow);
        }

        private int CalculateHashForElement(SimpleRow newRow)
        {
            return newRow.Transaction[depth]%firstNumber;
        }

        private void SplitPage(SimpleRow newRow)
        {
            isLeafNode = false;
            InitializeChildren();
            elements.Add(newRow);
            foreach (var simpleRow in elements)
            {
                int hash = CalculateHashForElement(simpleRow);
                children[hash].Add(simpleRow);
            }
            elements = null;
        }

        private bool CanAddElementToPage()
        {
            return depth == transactionLength || elements.Count  != pageSize;
        }
    }
}