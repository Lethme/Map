using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Map.BinaryTree
{
    public class BinaryTree<T> : IEnumerable<T>
    {
        private List<T> Items { get; } = new List<T>();
        public int Count => Items.Count;
        public bool Empty => Items.Count == 0;
        public BinaryTree() {  }
        public T this[int index] => Items[index];
        public void Add(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            Items.Add(obj);
        }
        public T GetLeftChildByIndex(int index)
        {
            if (Items.Count >= (2 * index + 1))
                return Items[2 * index + 1];
            return default;
        }
        public T GetRightChildByIndex(int index)
        {
            if (Items.Count >= (2 * index + 2))
                return Items[2 * index + 2];
            return default;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}
