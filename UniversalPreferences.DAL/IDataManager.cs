using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.DAL
{
    public interface IDataManager
    {
        void Initialize();

        IList<Row> GetData();

        Dictionary<int, string> GetMappings();
    }
}