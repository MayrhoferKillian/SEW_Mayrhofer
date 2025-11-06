using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using _10_Bank_Simulation;


class Program
{
    static Random random = new Random();
    static List<BankAccount> accounts = new List<BankAccount>();
    static double totalStartBalance;

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        for (int i = 0; i < 10; i++)
        {
            double startBalance = random.Next(500, 2000);
            accounts.Add(new BankAccount($"Konto{i + 1:D2}", startBalance));
            totalStartBalance += startBalance;
        }

        Console.WriteLine($"Startsumme aller Konten: {totalStartBalance:F2} €\n");

        List<Thread> threads = new List<Thread>();
        for (int i = 0; i < 5; i++)
        {
            Thread t = new Thread(SimulateTransactions);
            threads.Add(t);
            t.Start();
        }

        foreach (var t in threads)
            t.Join();

        double totalEndBalance = accounts.Sum(a => a.Balance); 
        Console.WriteLine($"Differenz: {totalEndBalance - totalStartBalance:F2} €");
        Console.WriteLine("Simulation beendet");
    }

    static void SimulateTransactions()
    {
        for (int i = 0; i < 20; i++)
        {
            int action = random.Next(3);
            var account = accounts[random.Next(accounts.Count)]; 

            switch (action)
            {
                case 0:
                    double deposit = random.Next(10, 200);
                    account.Deposit(deposit);
                    break;

                case 1:
                    double withdraw = random.Next(10, 200);
                    account.Withdraw(withdraw);
                    break;

                case 2:
                    var target = accounts[random.Next(accounts.Count)]; 
                    if (target != account)
                    {
                        double amount = random.Next(10, 150);
                        account.Transfer(target, amount);
                    }
                    break;
            }

            Thread.Sleep(random.Next(50, 150));
        }
    }
}