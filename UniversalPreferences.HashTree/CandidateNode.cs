using System.Collections.Generic;
using System.Linq;
using UniversalPreferences.Common;

namespace UniversalPreferences.HashTree
{
    public class CandidateNode
    {
         private readonly int pageSize;
        private readonly int firstNumber;
        private readonly int transactionLength;
        private readonly int depth;
        private CandidateNode[] children;
        private IList<ushort[]> elements;

        private bool isLeafNode;

        public CandidateNode(int pageSize, int firstNumber, int transactionLength, int depth)
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
                elements = new List<ushort[]>();
            }
            
        }

        private void InitializeChildren()
        {
            children = new CandidateNode[firstNumber];
            for (int i = 0; i < firstNumber; i++)
            {
                children[i] = new CandidateNode(pageSize, firstNumber, transactionLength, depth + 1);
            }
        }

        public void Add(ushort[] newRow)
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

        private int CalculateHashForElement(ushort[] newRow)
        {
            return newRow[depth]%firstNumber;
        }

        private void SplitPage(ushort[] newRow)
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

        public void FillSupportedRows(ICollection<ushort[]> supportedRows, ushort[] row, int firstIndexToCheck, HashSet<ushort> hashOfRow)
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

        private void FillSupportedRowsFromLeaf(ICollection<ushort[]> supportedRows, HashSet<ushort> hashOfRow)
        {
            foreach (var element in elements)
            {
                if (element.All(hashOfRow.Contains))
                {
                    supportedRows.Add(element);
                }
            }
        } 
    }
}