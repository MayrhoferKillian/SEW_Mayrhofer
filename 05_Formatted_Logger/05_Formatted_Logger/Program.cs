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

        static void Main(string[] args)
        {
            Logging logger = new Logging();

            logger.AddLogMessage("System gestartet");
            logger.AddLogMessage("Ein sehr langer Text, der gekürzt werden sollte...");

            Console.WriteLine("=== Mit Datum/Zeit ===");
            logger.PrintMessages(FormatAddDateTime);

            Console.WriteLine("\n=== Auf 30 Zeichen begrenzt ===");
            logger.PrintMessages(FormatLengthLimit);


        }
    }
}
