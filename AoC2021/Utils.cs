using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
    public class Utils
    {
        public static string ReadInput(string fileName)
        {
            var content = string.Empty;

            using (var reader = File.OpenText(fileName))
            {
                content = reader.ReadToEnd();
            }

            return content;
        }
    }

    public class PrioQueue<TElement, TPriority>
    {
        int total_size;
        SortedDictionary<TPriority, Queue<TElement>> storage;

        public PrioQueue()
        {
            this.storage = new SortedDictionary<TPriority, Queue<TElement>>();
            this.total_size = 0;
        }

        public bool IsEmpty()
        {
            return (total_size == 0);
        }

        public int Count => total_size;

        public TElement Dequeue()
        {
            if (IsEmpty())
            {
                throw new Exception("Please check that priorityQueue is not empty before dequeing");
            }
            else
                foreach (Queue< TElement> q in storage.Values)
                {
                    // we use a sorted dictionary
                    if (q.Count > 0)
                    {
                        total_size--;
                        return q.Dequeue();
                    }
                }

           // Debug.Assert(false, "not supposed to reach here. problem with changing total_size");

            return default; // not supposed to reach here.
        }

        // same as above, except for peek.

        public TElement Peek()
        {
            if (IsEmpty())
                throw new Exception("Please check that priorityQueue is not empty before peeking");
            else
                foreach (Queue< TElement> q in storage.Values)
                {
                    if (q.Count > 0)
                        return q.Peek();
                }

            //Debug.Assert(false, "not supposed to reach here. problem with changing total_size");

            return default; // not supposed to reach here.
        }

        public TElement Dequeue(TPriority prio)
        {
            total_size--;
            return storage[prio].Dequeue();
        }

        public void Enqueue(TElement item, TPriority prio)
        {
            if (!storage.ContainsKey(prio))
            {
                storage.Add(prio, new Queue<TElement>());
            }
            storage[prio].Enqueue(item);
            total_size++;
        }
    }
}
