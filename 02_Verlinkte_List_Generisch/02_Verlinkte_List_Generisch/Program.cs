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
		
		// TODO: Pop(): Löscht das zuletzt eingefügte Element und gibt es an den Aufrufer zurück
		
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
            list.Output();
            Console.WriteLine("Pop Item: " + list.Pop());
            list.Output();
        }
    }
}