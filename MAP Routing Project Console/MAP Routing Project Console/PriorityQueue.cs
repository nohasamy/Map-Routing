using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP_Routing_Project_Console
{
    public class PriorityQueue
    {
        public List<KeyValuePair<Int32, double>> data;
        public int Size { get; set; }

        public PriorityQueue()
        {
            this.data = new List<KeyValuePair<Int32, double>>();
            Size = 0;
        }
        public bool peek()
        {
            if (Size == 0)
            {
                return false;
            }
            return true;
        }
        public int getLeftChildIndex(int parentIndex)
        {
            return 2 * parentIndex + 1;
        }

        public bool hasLeftChild(int parentIndex)
        {
            return getLeftChildIndex(parentIndex) < Size;
        }

        public KeyValuePair<Int32, double> leftChild(int index)
        {
            return data[getLeftChildIndex(index)];
        }

        public int getRightChildIndex(int parentIndex)
        {
            return 2 * parentIndex + 2;
        }

        public bool hasRightChild(int parentIndex)
        {
            return getRightChildIndex(parentIndex) < Size;
        }

        public KeyValuePair<Int32, double> rightChild(int index)
        {
            return data[getRightChildIndex(index)];
        }

        public int getParentIndex(int childIndex)
        {
            return (childIndex - 1) / 2;
        }

        public bool hasParent(int childIndex)
        {
            return getParentIndex(childIndex) >= 0;
        }

        public KeyValuePair<Int32, double> parent(int index)
        {
            return data[getParentIndex(index)];
        }
        public void swap(int index1, int index2)
        {
            KeyValuePair<Int32, double> temp = data[index1];
            data[index1] = data[index2];
            data[index2] = temp;
        }
        public void Enqueue(Int32 vertix, double time)
        {
            KeyValuePair<Int32, double> item = new KeyValuePair<int, double>(vertix, time);
            data.Add(item);
            Int32 ci = data.Count - 1; // child index; start at end
            while (ci > 0)
            {
                Int32 pi = getParentIndex(ci); // parent index
                if (data[ci].Value.CompareTo(data[pi].Value) >= 0)
                {
                    break;
                } // child item is larger than (or equal) parent so we're done
                swap(ci, pi);
            }
            Size++;
        }

        public KeyValuePair<Int32, double> Dequeue()
        {
            if (Size == 0)
                throw new InvalidOperationException("Heap is empty");
            // assumes pq is not empty; up to calling code
            Int32 li = data.Count - 1; // last index (before removal)
            KeyValuePair<Int32, double> frontItem = Root();   // fetch the front
            data[0] = data[li];
            data.RemoveAt(li);

            --li; // last index (after removal)
            Int32 pi = 0; // parent index. start at front of pq
            while (true)
            {
                Int32 ci = getLeftChildIndex(pi); // left child index of parent
                if (ci > li) break;  // no children so done
                Int32 rc = getRightChildIndex(pi);     // right child
                if (rc <= li && data[rc].Value.CompareTo(data[ci].Value) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
                    ci = rc;
                if (data[pi].Value.CompareTo(data[ci].Value) <= 0) break; // parent is smaller than (or equal to) smallest child so done
                KeyValuePair<Int32, double> tmp = data[pi];
                swap(ci, pi); // swap parent and child
            }
            Size--;
            return frontItem;
        }

        public KeyValuePair<Int32, double> Root()
        {
            KeyValuePair<Int32, double> frontItem = data[0];
            return frontItem;
        }

        public int Count()
        {
            return data.Count;
        }
        public bool IsConsistent()
        {
            // is the heap property true for all data?
            if (data.Count == 0) return true;
            int li = data.Count - 1; // last index
            for (int pi = 0; pi < data.Count; ++pi) // each parent index
            {
                int lci = getLeftChildIndex(pi); // left child index
                int rci = getRightChildIndex(pi); // right child index

                if (lci <= li && data[pi].Value.CompareTo(data[lci]) > 0) return false; // if lc exists and it's greater than parent then bad.
                if (rci <= li && data[pi].Value.CompareTo(data[rci]) > 0) return false; // check the right child too.
            }
            return true; // passed all checks
        } // IsConsistent
    } // PriorityQueue

}





