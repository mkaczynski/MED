using System.Collections.Generic;
using System.Linq;
using UniversalPreferences.Common;

namespace UniversalPreferences.HashTree
{
    internal class Node
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

        public void FillSupportedRows(ICollection<SimpleRow> supportedRows, ushort[] row, int firstIndexToCheck)
        {
            if (isLeafNode)
            {
                FillSupportedRowsFromLeaf(supportedRows, row);
                return;
            }

            var viewedNodes = new HashSet<int>();
            for (int i = firstIndexToCheck; i < row.Length; i++)
            {
                if (CanCutSearching(row.Length, i))
                    return;

                var currentElement = row[i];
                var hashOfCurrentElement = currentElement%firstNumber;
                if (viewedNodes.Contains(hashOfCurrentElement))
                    continue;
                viewedNodes.Add(hashOfCurrentElement);
                children[hashOfCurrentElement].FillSupportedRows(supportedRows, row, i+1);
            }
        }

        private bool CanCutSearching(int elementsInRow, int currentPositionInRow)
        {
            var elementsToCheckInRow = elementsInRow - currentPositionInRow;
            var possiblyMatchedElements = transactionLength - depth;
            if (elementsToCheckInRow < possiblyMatchedElements)
            {
                return true;
            }
            return false;
        }

        private void FillSupportedRowsFromLeaf(ICollection<SimpleRow> supportedRows, ushort[] row)
        {
            foreach (var element in elements)
            {
                // aktualnie przy oznaczeniach element.Transaction.Length = n, row.Length = m
                // przy sprawdzaniu czy element istnieje w tablicy zlozonosc jest rowna nm
                // przy uzyciu hashset dla sprawdzanie zlozonosc jest rowna n+m
                var set = new HashSet<ushort>(row);
                if (element.Transaction.All(set.Contains))
                {
                    supportedRows.Add(element);
                }
            }
        }
    }
}