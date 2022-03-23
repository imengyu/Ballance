using System.Collections.Generic;

namespace BallancePhysics
{
  public class FastCachePool<T> where T : class,new() {

    private Stack<T> poolAvailable = new Stack<T>();
    private Stack<T> poolUsing = new Stack<T>();

    public FastCachePool(int size) {
      for (var i = 0; i < size && size < 32; i++)
        poolAvailable.Push(new T());
    }

    public T GetNext() {
      T newItem = poolAvailable.Pop();
      if(newItem == null) newItem = new T();
      poolUsing.Push(newItem);
      return newItem;
    }

    public T PopUsing() {
      T item = poolUsing.Count > 0 ? poolUsing.Pop() : null;
      poolAvailable.Push(item);
      return item;
    }

    public void Dispose() {
      poolAvailable.Clear();
      poolUsing.Clear();
    }
  }
}