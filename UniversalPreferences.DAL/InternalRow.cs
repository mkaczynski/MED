using System.Collections.Generic;

namespace UniversalPreferences.DAL
{
    class InternalRow
    {
        public int  Id { get; set; }

        public IList<ushort> AttributeIds { get; private set; }

        public string ClassName { get; set; }

        public InternalRow()
        {
            AttributeIds = new List<ushort>();
        }

        public InternalRow(int id, string className) : this()
        {
            Id = id;
            ClassName = className;
        }

        public void AddAttributeId(ushort attributeId)
        {
            AttributeIds.Add(attributeId);
        }
    }
}