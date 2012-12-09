using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.DAL
{
    public interface IDataManager
    {
        void Initialize();
        IList<Row> GetData();
        Dictionary<int, string> GetMappings();
        int MinLeftSideIndex(); //to ma tylko zwrocic informacje od ktorej wartosci 
                                //wyniki mapowac na lewa strone (tj. 1256 - 125 => 6), zobacz SimpleData
    }
}