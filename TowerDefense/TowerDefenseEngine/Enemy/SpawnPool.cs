using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseEngine
{
    public class SpawnPool<T> where T : new()
    {
        int maxItems;

        int count;

        Queue<T> inactive;

        public SpawnPool(int max)
        {
            maxItems = max;
            inactive = new Queue<T>(maxItems);
            Populate();
        }

        public void Populate()
        {
            for (int i = 0; i < maxItems; i++)
                inactive.Enqueue(new T());

            count = maxItems;
        }
        public T Release()
        {
            count--;
            return inactive.Dequeue();
        }

        public void Add(T t)
        {
            count++;
            inactive.Enqueue(t);
        }
    }
}
