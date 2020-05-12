namespace Gasanov.Extensions
{
    public class Couple<T,TU>
    {
        public Couple()
        {
            Item1 = default(T);
            Item2 = default(TU);
        }

        public Couple(T item1, TU item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
        
        /// <summary>
        /// Первый объект
        /// </summary>
        public T Item1 { get; set; }
        
        /// <summary>
        /// Второй объект
        /// </summary>
        public TU Item2 { get; set; }

    }
}