using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubledLinkedList
{
    [Serializable]
    public class Node<T>
    {
        private T data;
        private Node<T> next;
        private Node<T> prev;
        public Node(T Data)
        {
            this.data = Data;
            this.next = null;
            this.prev = null;
        }
        public Node<T> Next
        {
            get { return next; }
            set { next = value; }
        }
        public Node<T> Prev
        {
            get { return prev; }
            set { prev = value; }
        }
        public T Data
        {
            get { return data; }
            set { data = value; }
        }

    }
}
