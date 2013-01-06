namespace UniversalPreferences.Common
{
    //note: dla tej reprezentacji nalezy przechowywac mapowanie pomiedzy deskryptorem atrybutu, a jego indeksem w tabeli Atributes
    public class Row
    {
        public int MainObjectId { get; set; }    // identyfikator pierwszego obiektu
        public int SecondObjectId { get; set; }  // identyfikator obiektu ktory z nim zestawiamy

        public ushort[] Attributes { get; set; }    // tabela zawierajaca wartosci atrybutow - jedna dla obu obiektow

        public int FirstSecondObjectAtribute { get; set; } // indeks pierwszego atrybutu drugiego obiektu w tabeli wyzej

        public Relation Value { get; set; }      // okresla czy relacja jest spelniona dla tego wiersza
    }
}