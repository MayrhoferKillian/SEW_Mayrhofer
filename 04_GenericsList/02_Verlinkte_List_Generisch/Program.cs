using _02_Verlinkte_List_Generisch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _02_Verlinkte_Listen_Generisch
{
    class GenericList<T>
    {
        public int Count
        {
            get
            {
                int count = 0;
                ListEntry<T> entry = firstEntry;

                while (entry != null)
                {
                    count++;
                    entry = entry.Next;
                }
                return count;
            }
        }

        private ListEntry<T> firstEntry = null;

        public void Add(T data)
        {
            ListEntry<T> newEntry = new ListEntry<T>(data);
            newEntry.Next = firstEntry;
            firstEntry = newEntry;
        }

        // TODO: Output(): Gibt alle Einträge auf der Konsole aus
        public void Output()
        {
            ListEntry<T> entry = firstEntry;
            while (entry != null)
            {
                Console.WriteLine(entry.Data);
                entry = entry.Next;
            }
        }


        // TODO: Pop(): Löscht das zuletzt eingefügte Element und gibt es an den Aufrufer zurück

        public object Pop()
        {
            if (firstEntry == null)
                return null; 

            ListEntry<T> entry = firstEntry;
            firstEntry = firstEntry.Next;
            return entry.Data;
        }
        public object FindFirst(ISelector selector)
        {
            ListEntry<T> entry = firstEntry;

            while (entry != null)
            {
                if (selector.Select(entry.Data))
                {
                    return entry.Data; 
                }
                entry = entry.Next;
            }   

            return null; 
        }

        public GenericList<T> FindAll(ISelector selector)
        {
            GenericList<T> result = new GenericList<T>();
            ListEntry<T> entry = firstEntry;

            while (entry != null)
            {
                if (selector.Select(entry.Data)) 
                {
                    result.Add(entry.Data);
                }
                entry = entry.Next;
            }

            return result;
        }

        public void Remove(ISelector selector)
        {
            while (firstEntry != null && selector.Select(firstEntry.Data))
            {
                firstEntry = firstEntry.Next;
            }

            ListEntry<T> current = firstEntry;

            while (current != null && current.Next != null)
            {
                if (selector.Select(current.Next.Data))
                {
                    current.Next = current.Next.Next;
                }
                else
                {
                    current = current.Next;
                }
            }
        }


        public void ShiftFwd(int numElements)
        {
            int n = Count;
            if (n == 0 || numElements % n == 0) return;

            numElements = numElements % n;

            // Liste in Kreis verwandeln
            ListEntry<T> last = firstEntry;
            while (last.Next != null)
            {
                last = last.Next;
            }
            last.Next = firstEntry; // Kreis schließen

            // Neuen Startknoten finden
            int stepsToNewHead = numElements;
            ListEntry<T> newLast = firstEntry;
            for (int i = 0; i < stepsToNewHead - 1; i++)
            {
                newLast = newLast.Next;
            }

            firstEntry = newLast.Next;
            newLast.Next = null; // Kreis wieder aufbrechen
        }
        public void ShiftBwd(int numElements)
        {
            int n = Count;
            if (n == 0 || numElements % n == 0) return;

            numElements = numElements % n;

            // Shift nach hinten um k == Shift nach vorne um (n - k)
            ShiftFwd(n - numElements);
        }
    }
    class ListEntry<T>
    {
        public T Data;
        public ListEntry<T> Next;

        public ListEntry(T data)
        {
            Data = data;
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            GenericList<string> list = new GenericList<string>();
            list.Add("F");
            list.Add("E");
            list.Add("D");
            list.Add("C");
            list.Add("B");
            list.Add("A");

            list.Output();           // A B C D E F
            list.ShiftFwd(2);
            list.Output();           // C D E F A B
            list.ShiftBwd(2);
            list.Output();           // A B C D E F

            Console.ReadKey();
        }
    }
}