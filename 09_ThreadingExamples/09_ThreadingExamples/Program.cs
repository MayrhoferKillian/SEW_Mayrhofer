using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WordCounter
{
    class Program
    {
        private static readonly string[] urls = new string[] {
            "https://www.gutenberg.org/files/1342/1342-0.txt", // Pride and Prejudice
            "https://www.gutenberg.org/files/11/11-0.txt", // Alice's Adventures in Wonderland
            "https://www.gutenberg.org/files/84/84-0.txt", // Frankenstein
            "https://www.gutenberg.org/files/2701/2701-7.0.htm", // Moby Dick by Herman Melville
            "https://www.gutenberg.org/files/2606/2606-h/2606-h.htm", // War and Peace by Leo Tolstoy
            "https://www.gutenberg.org/files/1342/1342-h/1342-h.htm", // Pride and Prejudice by Jane Austen
            "https://www.gutenberg.org/files/23654/23654-h/23654-h.htm", // The Brothers Karamazov by Fyodor Dostoyevsky
            "https://www.gutenberg.org/files/1399/1399-h/1399-h.htm", // Anna Karenina by Leo Tolstoy
            "https://www.gutenberg.org/files/2554/2554-h/2554-h.htm", // Crime and Punishment by Fyodor Dostoyevsky
            "https://www.gutenberg.org/files/1184/1184-h/1184-h.htm", // The Count of Monte Cristo by Alexandre Dumas
            "https://www.gutenberg.org/files/996/996-h/996-h.htm", // Don Quixote by Miguel de Cervantes
            "https://www.gutenberg.org/files/135/135-h/135-h.htm", // Les Misérables by Victor Hugo
            "https://www.gutenberg.org/files/145/145-h/145-h.htm", // Middlemarch by George Eliot
        };

        private static int globalCounter = 0;
        private static readonly object lockObject = new object();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Word Counter - Counting occurrences of 'the'");
            Console.WriteLine("=============================================\n");

            // Variante 1: Single-Threaded
            Console.WriteLine("VARIANTE 1: Single-Threaded");
            Console.WriteLine("---------------------------");
            await RunSingleThreadedVersion();

            Console.WriteLine("\n" + new string('=', 60) + "\n");

            // Variante 2: Multi-Threaded
            Console.WriteLine("VARIANTE 2: Multi-Threaded");
            Console.WriteLine("---------------------------");
            await RunMultiThreadedVersion();

            Console.WriteLine("\nProgramm beendet. Drücken Sie eine Taste...");
            Console.ReadKey();
        }

        static async Task RunSingleThreadedVersion()
        {
            var stopwatch = Stopwatch.StartNew();
            int totalCount = 0;

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30);

                for (int i = 0; i < urls.Length; i++)
                {
                    try
                    {
                        Console.WriteLine($"Lade Text {i + 1}/{urls.Length}: {GetFileName(urls[i])}");
                        string pageContent = await client.GetStringAsync(urls[i]);
                        int count = CountWordOccurrences(pageContent, "the");
                        totalCount += count;
                        Console.WriteLine($"  Gefundene 'the': {count}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"  Fehler beim Verarbeiten von {urls[i]}: {ex.Message}");
                    }
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"\nGesamtzahl der 'the' Vorkommen: {totalCount}");
            Console.WriteLine($"Ausführungszeit: {stopwatch.ElapsedMilliseconds} ms");
        }

        static async Task RunMultiThreadedVersion()
        {
            var stopwatch = Stopwatch.StartNew();
            globalCounter = 0; // Reset global counter
            var tasks = new List<Task>();

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30);

                for (int i = 0; i < urls.Length; i++)
                {
                    string url = urls[i];
                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            string pageContent = await client.GetStringAsync(url);
                            int localCount = CountWordOccurrences(pageContent, "the");

                            // Synchronized access to global counter
                            lock (lockObject)
                            {
                                globalCounter += localCount;
                            }

                            Console.WriteLine($"{GetFileName(url)}: {localCount} 'the' gefunden");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Fehler bei {GetFileName(url)}: {ex.Message}");
                        }
                    }));
                }

                await Task.WhenAll(tasks);
            }

            stopwatch.Stop();
            Console.WriteLine($"\nGesamtzahl der 'the' Vorkommen: {globalCounter}");
            Console.WriteLine($"Ausführungszeit: {stopwatch.ElapsedMilliseconds} ms");
        }

        static int CountWordOccurrences(string text, string word)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(word))
                return 0;

            int count = 0;
            int index = 0;
            string searchText = text.ToLower();
            string searchWord = word.ToLower();

            while ((index = searchText.IndexOf(searchWord, index)) != -1)
            {
                count++;
                index += searchWord.Length;
            }

            return count;
        }

        static string GetFileName(string url)
        {
            return Path.GetFileName(url) ?? url;
        }
    }
}