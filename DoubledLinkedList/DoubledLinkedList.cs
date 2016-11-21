using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Specialized;
using System.Data.SqlClient;

namespace DoubledLinkedList
{
    [Serializable]
    public class DoubledLinkedList<T> : IEnumerable, INotifyCollectionChanged
    {
        #region Fields
        private Node<T> First;
        private Node<T> Last;
        private Node<T> Curr;

        private uint count;

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        #endregion
        

        public DoubledLinkedList()
        {
            this.count = 0;
            this.First = this.Last = this.Curr = null;
        }
        
        #region Add Methods
        public void Add(T elem)
        {
            Node<T> NewNode = new Node<T>(elem);
            if (First == null)
            {
                First = Last = NewNode;
            }
            else
            {
                Last.Next = NewNode;
                NewNode.Prev = Last;
                Last = NewNode;
            }
            count++;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, elem));
        }
        public void AddFront(T elem)
        {
            Node<T> NewNode = new Node<T>(elem);
            if (First == null)
            {
                First = Last = NewNode;
            }
            else
            {
                NewNode.Next = First;
                First.Prev = NewNode;
                First = NewNode;
            }
            count++;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, elem));
        }
        public void AddByIndex(T newElement, uint index)
        {
            if (index < 1 || index > count + 1)
            {
                throw new IndexOutOfRangeException();
            }
            else if (index == 1)
            {
                AddFront(newElement);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            else if (index == count + 1)
            {
                Add(newElement);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            else
            {
                uint i = 1;
                Curr = First;
                while (Curr != null && i != index)
                {
                    Curr = Curr.Next;
                    i++;
                }
                Node<T> newNode = new Node<T>(newElement);
                count++;
                Curr.Prev.Next = newNode;
                newNode.Prev = Curr.Prev;
                Curr.Prev = newNode;
                newNode.Next = Curr;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
        #endregion
       
        #region Print Methods
        public void Print(PrintFunc p)
        {
            if (First == null)
            {
                p("Doubly Linked List is empty");
                return;
            }
            Curr = First;
            while (Curr != null)
            {
                p(Curr.Data.ToString());
                Curr = Curr.Next;
            }
        }
        public delegate Object PrintFunc(string obj);
        public void ReversePrint(PrintFunc p)
        {
            if (Last == null)
            {
                p("Doubly Linked List is empty");
                return;
            }
            Curr = Last;
            while (Curr != null)
            {
                p(Curr.Data.ToString());
                Curr = Curr.Prev;
            }
        }
        #endregion

        #region Delete Methods
        public Node<T> PopFront()
        {
            if (First == null)
            {
                throw new InvalidOperationException("List empty\n");
            }
            else
            {
                Node<T> temp = First;
                if (First.Next != null)
                {
                    First.Next.Prev = null;
                }
                First = First.Next;
                count--;
                temp.Next = null;
                return temp;
            }
        }
        public Node<T> Pop()
        {
            if (Last == null)
            {
                throw new InvalidOperationException("List empty\n");
            }
            else
            {
                Node<T> temp = Last;
                if (Last.Prev != null)
                {
                    Last.Prev.Next = null;
                }
                Last = Last.Prev;
                temp.Prev = null;
                count--;
                return temp;
            }
        }
        public void ClearList()
        {
            count = 0;
            Curr = null;
            First = null;
            Last = null;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        public void RemoveByIndex(uint index)
        {
            if (index < 1 || index > count)
            {
                throw new IndexOutOfRangeException();
            }
            else if (index == 1)
            {
                PopFront();
            }
            else if (index == count)
            {
                Pop();
            }
            else
            {
                uint i = 1;
                Curr = First;
                while (Curr != null && i != index)
                {
                    Curr = Curr.Next;
                    i++;
                }
                Curr.Prev.Next = Curr.Next;
                Curr.Next.Prev = Curr.Prev;
                --count;
            }
        }
        public void RemoveByPredicate(Predicate<T> condition)
        {
            Curr = First;
            while (Curr != null)
            {
                if (condition(Curr.Data))
                {
                    if (Curr == First)
                        First = Curr.Next;
                    if (Curr == Last)
                        Last = Curr.Prev;
                    if (Curr.Next != null)
                        Curr.Next.Prev = Curr.Prev;
                    if (Curr.Prev != null)
                        Curr.Prev.Next = Curr.Next;
                    count--;
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
                Curr = Curr.Next;
            }
        }
        #endregion
        
        #region Save and Load
        public bool Save(string filename)
        {
            DoubledLinkedList<T> obj = this;
            System.IO.FileStream fileStream = null;
            bool flag = true;
            try
            {
                fileStream = new System.IO.FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                binaryFormatter.Serialize(fileStream, obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Исключение: " + ex.Message);
                flag = false;
            }
            finally
            {
                if (fileStream != null) fileStream.Close();
            }
            return flag;
        }
        public static bool Load(string filename, ref DoubledLinkedList<T> obj)
        {
            System.IO.FileStream fileStream = null;
            bool flag = true;
            try
            {
                fileStream = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                obj = binaryFormatter.Deserialize(fileStream) as DoubledLinkedList<T>;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Исключение: " + ex.Message);
                flag = false;
            }
            finally
            { if (fileStream != null) fileStream.Close(); }
            return flag;
        }
        #endregion
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, e);
        }
        public uint Count
        {
            get { return count; }
            private set { count = value; }
        }
        public bool isempty()
        {
            return count == 0;
        }
        public delegate int Condtion(T obj1, T obj2);
        public void SortByPred(Condtion cond)
        {
            for (int i = 0; i < count; ++i)
            {
                Curr = First;
                while (Curr != null)
                {
                    if (Curr.Next != null)
                    {
                        if (cond(Curr.Data, Curr.Next.Data) == 1)
                        {
                            T temp = Curr.Data;
                            Curr.Data = Curr.Next.Data;
                            Curr.Next.Data = temp;
                        }
                    }
                    Curr = Curr.Next;
                }
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            Node<T> Numerator = First;
            while (Numerator != null)
            {
                yield return Numerator.Data;
                Numerator = Numerator.Next;
            }
        }
    }
}