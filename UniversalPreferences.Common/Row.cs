using System.Collections.Generic;

namespace UniversalPreferences.Common
{
    //note: dla tej reprezentacji nalezy przechowywac mapowanie pomiedzy deskryptorem atrybutu, a jego indeksem w tabeli Atributes
    public class Row
    {
        public int MainObjectId { get; set; }    // identyfikator pierwszego obiektu
        public int SecondObjectId { get; set; }  // identyfikator obiektu ktory z nim zestawiamy

        public ushort[] Attributes { get; set; }    // tabela zawierajaca wartosci atrybutow - jedna dla obu obiektow

        public Relation Value { get; set; }      // okresla czy relacja jest spelniona dla tego wiersza

        public Row(int firstID, int secondID, ushort[] attributes, Relation value)
        {
            this.MainObjectId = firstID;
            this.SecondObjectId = secondID;
            this.Attributes = new ushort[attributes.Length];
            for (int i = 0; i < attributes.Length; ++i)
            {
                this.Attributes[i] = attributes[i];
            }
            this.Value = value;
        }

        public Row()
        {

        }
    }
}