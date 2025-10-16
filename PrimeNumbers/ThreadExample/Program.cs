using System;
using System.Threading;

internal class Programm
{
    class Counter
    {
        private ConsoleColor color;

        public Counter(ConsoleColor color)
        {
            this.color = color;
        }

        public void Count()
        {
            for (int i = 0; i < 100; i++) // kleinerer Wert zum Testen
            {
                lock (Console.Out) // verhindert dass sich Threads beim Schreiben überlappen
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine(i);
                }
                Thread.Sleep(10); // Optional, um die Ausgabe besser sichtbar zu machen
            }
        }
    }

    static void Main(string[] args)
    {
        Counter counter1 = new Counter(ConsoleColor.Red);
        Counter counter2 = new Counter(ConsoleColor.Green);

        Thread thread1 = new Thread(counter1.Count);
        Thread thread2 = new Thread(counter2.Count);

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        Console.ResetColor();
    }
}
