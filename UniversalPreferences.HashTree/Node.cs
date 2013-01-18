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
        private IList<Row> elements;

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
                elements = new List<Row>();
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

        public void Add(Row newRow)
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

        private int CalculateHashForElement(Row newRow)
        {
            return newRow.Attributes[depth]%firstNumber;
        }

        private void SplitPage(Row newRow)
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

        public void FillSupportedRows(ICollection<Row> supportedRows, ushort[] row, int firstIndexToCheck, HashSet<ushort> hashOfRow)
        {
            if (isLeafNode)
            {
                FillSupportedRowsFromLeaf(supportedRows, hashOfRow);
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
                children[hashOfCurrentElement].FillSupportedRows(supportedRows, row, i+1, hashOfRow);
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

        private void FillSupportedRowsFromLeaf(ICollection<Row> supportedRows, HashSet<ushort> hashOfRow)
        {
            foreach (var element in elements)
            {
                if (element.Attributes.All(hashOfRow.Contains))
                {
                    supportedRows.Add(element);
                }
            }
        }
    }
}