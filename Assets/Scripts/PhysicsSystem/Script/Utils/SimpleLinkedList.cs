using System;

namespace PhysicsRT.Utils
{
	interface LinkedListItem<T>
    {
		T prev { get; set; }
		T next { get; set; }
	}

	class SimpleLinkedList<T> : IDisposable where T : class, LinkedListItem<T>
	{
		T begin = null;
		T end = null;

		public void add(T n)
		{
			if (begin == null || end == null)
			{
				begin = n;
				end = n;
				m_size = 1;
			}
			else
			{
				n.prev = end;
				end.next = n;
				end = n;
				m_size++;
			}
		}
		public void remove(T n)
		{
			if (n.prev != null)
				n.prev.next = n.next;
			if (n.next != null)
				n.next.prev = n.prev;
			if(n == begin)
				begin = n.next;
			if(n == end)
				end = n.prev;
			m_size--;
		}
		public void clear()
		{
			T ptr = begin;
			while (ptr != null)
			{
				ptr.prev = null;
				T oldPtr = ptr.next;
				ptr.next = null;
				ptr = oldPtr;
			}
			begin = null;
			end = null;
			m_size = 0;
		}

		public int getSize()
		{
			return m_size;
		}
		public T getBegin()
		{
			return begin;
		}
		public T getEnd()
		{
			return end;
		}

		private int m_size = 0;
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
					clear();
				}

				begin = null;
				end = null;
				disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
