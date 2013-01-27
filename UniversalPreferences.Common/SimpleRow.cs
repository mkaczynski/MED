using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalPreferences.Common
{
    public class SimpleRow
    {
        public ushort [] Transaction { get; private set; }
        public int RelationComplied { get; set; }
        public int RelationNotComplied { get; set; }

        public int MinRelationComplied { get; set; }
        public int MinRelationNotComplied { get; set; }

        public SimpleRow(IEnumerable<ushort> transaction)
        {
            Transaction = transaction.ToArray();

            RelationComplied = 0;
            RelationNotComplied = 0;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (ushort t in Transaction)
            {
                sb.Append(t);
                sb.Append(", ");
            }

            sb.AppendFormat(" - ({0}, {1})", RelationNotComplied, RelationComplied);

            return sb.ToString();
        }
    }
}