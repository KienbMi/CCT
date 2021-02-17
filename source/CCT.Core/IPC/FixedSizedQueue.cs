using System.Collections.Concurrent;

namespace FixedSizedQueue
{
    public class FixedSizedQueue<T>
    {
        private ConcurrentQueue<T> queue;

        public int Size { get; private set; }

        public int Count => queue.Count;

        public FixedSizedQueue(int size)
        {
            Size = size;
            queue = new ConcurrentQueue<T>();
        }

        public void Enqueue(T obj)
        {
            queue.Enqueue(obj);

            while (queue.Count > Size)
            {
                T outObj;
                queue.TryDequeue(out outObj);
            }
        }
    }
}
