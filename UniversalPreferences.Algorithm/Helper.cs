using System.Linq;
using UniversalPreferences.Common;

namespace UniversalPreferences.Algorithm
{
    public class Helper
    {
        public static bool IsItemsetSupported(ushort[] itemset, Row transaction)
        {
            var intersection = transaction.Attributes.Where(x => x.HasValue).
                Select(x => x.Value).Intersect(itemset);
            
            return intersection.Count() == itemset.Length;
            
            //var contains = true;
            //for (int i = 0; i < itemset.Length; ++i)
            //{
            //    if (!transaction.Attributes.Contains(itemset[i]))
            //    {
            //        contains = false;
            //    }
            //}
            //return contains;
        } 
    }
}