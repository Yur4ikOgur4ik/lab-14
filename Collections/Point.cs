namespace Collections
{
    public class Point<T>
    {
        public T? Data { get; set; }
        public Point<T>? Next { get; set; }
        public Point<T>? Prev { get; set; } // Добавляем указатель на предыдущий элемент

        public Point()
        {
            Data = default(T);
            Next = null;
            Prev = null;
        }

        public Point(T info)
        {
            Data = info;
            Next = null;
            Prev = null;
        }

        public override string ToString()
        {
            return Data?.ToString() ?? "null";
        }

    }
}
