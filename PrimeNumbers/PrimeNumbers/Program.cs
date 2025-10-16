using System;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Bitte Start- und Endwert als Argumente angeben!");
            return;
        }

        int start = int.Parse(args[0]);
        int ende = int.Parse(args[1]);

        Console.WriteLine($"Primzahlen von {start} bis {ende}:");

        for (int i = start; i <= ende; i++)
        {
            if (IsPrime(i))
            {
                Console.WriteLine(i);
            }
        }
    }

    static bool IsPrime(int number)
    {
        if (number < 2)
            return false;

        for (int i = 2; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0)
                return false;
        }

        return true;
    }
}
