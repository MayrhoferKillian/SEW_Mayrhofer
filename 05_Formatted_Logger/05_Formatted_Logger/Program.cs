using System;
using System.Collections.Generic;

namespace LoggingDelegates
{
    public delegate string LogFormatter(string message);

    public class Logging
    {
        private List<string> logMessages = new List<string>();

        public void AddLogMessage(string msg)
        {
            logMessages.Add(msg);
        }

        public void PrintMessages(LogFormatter formatter)
        {
            foreach (var msg in logMessages)
            {
                string output = formatter != null ? formatter(msg) : msg;
                Console.WriteLine(output);
            }
        }

        public void PrintMessagedFiltered(Predicate<string> filterFunc)
        {
            foreach (var msg in logMessages)
            {
                if (filterFunc(msg))
                {
                    Console.WriteLine(msg);
                }
            }
        }

       
    }

    class Program
    {
        public static string FormatAddDateTime(string msg)
        {
            return $"[{DateTime.Now}] {msg}";
        }

        public static string FormatUpperCase(string msg)
        {
            return msg.ToUpper();
        }

        public static string FormatLengthLimit(string msg)
        {
            return msg.Length > 30 ? msg.Substring(0, 30) : msg;
        }

        public static bool FilterError(string msg)
        {
            return msg.Contains("Error", StringComparison.OrdinalIgnoreCase);
        }

        public static bool FilterWarning(string msg)
        {
            return msg.Contains("Warning", StringComparison.OrdinalIgnoreCase);
        }

        static void Main(string[] args)
        {
            Logging logger = new Logging();

            logger.AddLogMessage("System gestartet");
            logger.AddLogMessage("Warning: Niedriger Speicherstand");
            logger.AddLogMessage("Error: Verbindung fehlgeschlagen");
            logger.AddLogMessage("Benutzer angemeldet");

            Console.WriteLine("=== Alle Nachrichten ====");
            logger.PrintMessages(null);


            Console.WriteLine("=== Alle Errors ====");
            logger.PrintMessagedFiltered(FilterError);


            Console.WriteLine("=== Alle Warnings ====");
            logger.PrintMessagedFiltered(FilterWarning);

        }
    }
}
