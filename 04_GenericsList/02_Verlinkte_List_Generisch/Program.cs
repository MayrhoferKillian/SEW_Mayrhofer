using _02_Verlinkte_List_Generisch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _02_Verlinkte_Listen_Generisch
{
    class GenericList
    {
        public int Count
        {
            get
            {
                int count = 0;
                ListEntry entry = firstEntry;

                while (entry != null)
                {
                    count++;
                    entry = entry.next;
                }
                return count;
            }
        }

        private ListEntry firstEntry = null;

        public void Add(Object data)
        {
            ListEntry newEntry = new ListEntry(data);
            newEntry.next = firstEntry;
            firstEntry = newEntry;
        }

        // TODO: Output(): Gibt alle Einträge auf der Konsole aus
        public void Output()
        {
            ListEntry entry = firstEntry;
            while (entry != null)
            {
                Console.WriteLine(entry.data);
                entry = entry.next;
            }
        }


        // TODO: Pop(): Löscht das zuletzt eingefügte Element und gibt es an den Aufrufer zurück

        public object Pop()
        {
            if (firstEntry == null)
                return null; 

            ListEntry entry = firstEntry;
            firstEntry = firstEntry.next;
            return entry.data;
        }
        public object FindFirst(ISelector selector)
        {
            ListEntry entry = firstEntry;

            while (entry != null)
            {
                if (selector.Select(entry.data))
                {
                    return entry.data; 
                }
                entry = entry.next;
            }

            return null; 
        }

        public GenericList FindAll(ISelector selector)
        {
            GenericList result = new GenericList();
            ListEntry entry = firstEntry;

            while (entry != null)
            {
                if (selector.Select(entry.data)) 
                {
                    result.Add(entry.data);
                }
                entry = entry.next;
            }

            return result;
        }

        public void Remove(ISelector selector)
        {
            while (firstEntry != null && selector.Select(firstEntry.data))
            {
                firstEntry = firstEntry.next;
            }

            ListEntry current = firstEntry;

            while (current != null && current.next != null)
            {
                if (selector.Select(current.next.data))
                {
                    current.next = current.next.next;
                }
                else
                {
                    current = current.next;
                }
            }
        }

    }
    class ListEntry
    {
        public ListEntry next = null;
        public object data;

        public ListEntry(object data)
        {
            this.data = data;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            GenericList list = new GenericList();
            list.Add("Welt");
            list.Add("Hallo");
            list.Add(12345);
            list.Add("Test");
            list.Add(999);

            Console.WriteLine("Original-Liste:");
            list.Output();

            Console.WriteLine("Pop Item: " + list.Pop());

            Console.WriteLine("\nAlle Strings in neuer Liste:");
            GenericList stringList = list.FindAll(new FindStringSelector());
            stringList.Output();

            Console.WriteLine("Suche ersten String: " + list.FindFirst(new FindStringSelector()));

            Console.ReadKey();
        }
    }
}