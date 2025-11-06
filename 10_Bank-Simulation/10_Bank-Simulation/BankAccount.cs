using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10_Bank_Simulation
{
    public class BankAccount
    {
        private readonly object _lock = new object();

        public string AccountNumber { get; }
        public double Balance { get; private set; }

        public BankAccount(string accountNumber, double initialBalace)
        {
            AccountNumber = accountNumber;
            Balance = initialBalace;
        }

        public void Deposit(double amount)
        {
            lock (_lock)
            {
                Balance += amount;
                Console.WriteLine($"[{AccountNumber}] Einzahlung: +{amount:F2} € -> neuer Kontostand: {Balance:F2} €");
            }
        }

        public void Withdraw(double amount)
        {
            lock (_lock)
            {
                if (Balance >= amount)
                {
                    Balance -= amount;
                    Console.WriteLine($"[{AccountNumber}] Abhebung: -{amount:F2} € -> Neuer Kontostand: {Balance:F2} €");
                }
                else
                {
                    Console.WriteLine($"[{AccountNumber}] Abhebung fehlgeschlagen (zu wenig Guthaben).");
                }
            }
        }

        public void Transfer(BankAccount target, double amount)
        {
            var firstLock = string.Compare(AccountNumber, target.AccountNumber) < 0 ? this : target;
            var secondLock = firstLock == this ? target : this;
            
            lock (firstLock._lock)
            {
                lock (secondLock._lock)
                {
                    if (Balance >= amount)
                    {
                        Balance -= amount;
                        target.Balance += amount;
                        Console.WriteLine($"Überweisung: {amount:F2} € von [{AccountNumber}] → [{target.AccountNumber}]");
                    }
                    else
                    {
                        Console.WriteLine($"[{AccountNumber}] Überweisung fehlgeschlagen (zu wenig Guthaben).");
                    }
                }
            }
        }
    }
}
