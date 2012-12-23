namespace UniversalPreferences.HashTree
{
    public class HashTreeFactory
    {
        /// <summary>
        /// Tworzy HashTree o pododanych w argumentach parametrach.
        /// </summary>
        /// <param name="transactionLength">Dlugosc transkacji dla ktorych tworzone jest drzewo</param>
        /// <param name="pageSize">Makssymalny liczbe elementow mogaca byc przechowywana w wezle ktory nie jest na ostatnim poziomie.</param>
        /// <param name="firstNumber">Liczba pierwsza wedlug ktorej obliczany jest hash dla elementow transakcji</param>
        /// <returns></returns>
        public static IHashTree Create(
            int transactionLength,
            int pageSize,
            int firstNumber)
        {
            return new HashTree(transactionLength, pageSize, firstNumber);
        }
    }
}