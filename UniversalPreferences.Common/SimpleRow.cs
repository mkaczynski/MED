namespace UniversalPreferences.Common
{
    public class SimpleRow
    {
        public ushort[] Transaction { get; private set; }
        public int RelationComplied { get; set; }
        public int RelationNotComplied { get; set; }

        public SimpleRow(ushort[] transaction)
        {
            Transaction = transaction;

            RelationComplied = 0;
            RelationNotComplied = 0;
        }
    }
}