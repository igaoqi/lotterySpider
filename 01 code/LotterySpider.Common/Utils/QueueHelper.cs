using System.Collections.Concurrent;

namespace LotterySpider.Common.Utils
{
    public class QueueHelper<T>
    {
        public readonly static QueueHelper<T> Instance = new QueueHelper<T>();
        private static ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
        }

        public T Dequeue()
        {
            _queue.TryDequeue(out T item);
            return item;
        }

        public int Count
        {
            get
            {
                return _queue.Count;
            }
        }
    }
}