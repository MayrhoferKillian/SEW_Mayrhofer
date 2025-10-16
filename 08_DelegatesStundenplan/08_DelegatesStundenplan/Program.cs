using System;
using System.Collections.Generic;
using System.Linq;


enum Subject { SEW, INSY, SYT, M, D, E, NWT, NW2, GGP, PH, BESP, FIT, CCIT, KPT, WIR, RK, ETH, ITP2, PAUSE, ENDE, SUPPL}

class Program
{
    static Dictionary<string, Subject[]> plan = new()
    {
        ["Mo"] = new[] { Subject.GGP, Subject.NW2, Subject.E, Subject.E, Subject.BESP, Subject.WIR, Subject.ENDE, Subject.ENDE, Subject.ENDE, Subject.ENDE },
        ["Di"] = new[] { Subject.CCIT, Subject.CCIT, Subject.ITP2, Subject.ITP2, Subject.INSY, Subject.FIT, Subject.ITP2, Subject.ITP2, Subject.ITP2, Subject.ITP2 },
        ["Mi"] = new[] { Subject.INSY, Subject.INSY, Subject.SYT, Subject.SYT, Subject.CCIT, Subject.CCIT, Subject.ENDE, Subject.ENDE, Subject.ENDE, Subject.ENDE },
        ["Do"] = new[] { Subject.M, Subject.M, Subject.KPT, Subject.WIR, Subject.NW2, Subject.GGP, Subject.PAUSE, Subject.SEW, Subject.SEW, Subject.SEW },
        ["Fr"] = new[] { Subject.RK, Subject.RK, Subject.D, Subject.D, Subject.INSY, Subject.INSY, Subject.ETH, Subject.ENDE, Subject.ENDE, Subject.ENDE }
    };

    delegate Subject SubjectSelector(int stunde, string tag);

    static void Main()
    {
        Console.WriteLine("Normaler Stundenplan:");
        PrintPlan(plan);

        Console.WriteLine("\nTaste drücken, um 5 zufällige Stunden zu supplieren...");
        Console.ReadKey();
        Console.Clear();

        var supplPlan = CreateSupplPlan(plan);
        PrintPlan(supplPlan);

        Console.WriteLine("\nWähle eine Strategie:");
        Console.WriteLine("1 - Zufallsauswahl");
        Console.WriteLine("2 - Round Robin");
        Console.WriteLine("3 - Regelbasiert");

        //--funktion nachschauen
        ConsoleKeyInfo input = Console.ReadKey();
        ConsoleKey keyPressed = input.Key;

        SubjectSelector strategy = keyPressed switch
        {
            ConsoleKey.D1 or ConsoleKey.NumPad1 => RandomStrategy,
            ConsoleKey.D2 or ConsoleKey.NumPad2 => RoundRobinStrategy(),
            ConsoleKey.D3 or ConsoleKey.NumPad3 => RuleBasedStrategy,
            _ => RandomStrategy
        };

        var newPlan = ApplyStrategy(supplPlan, strategy);

        Console.Clear();
        Console.WriteLine("Neuer Stundenplan nach Strategie:\n");
        PrintPlan(newPlan);
    }

    static Subject RandomStrategy(int stunde, string tag)
    {
        Array subjects = Enum.GetValues(typeof(Subject));
        Random rnd = new();
        //--funktion nachschauen
        return (Subject)(subjects.GetValue(rnd.Next(0, subjects.Length - 2)) ?? Subject.SUPPL);

    }

    static SubjectSelector RoundRobinStrategy()
    {
        Subject[] subjects = (Subject[])Enum.GetValues(typeof(Subject));
        int index = 0;
        return (stunde, tag) =>
        {
            Subject s = (Subject)subjects[index % (subjects.Length - 2)];
            index++;
            return s;
        };
    }

    static Subject RuleBasedStrategy(int stunde, string tag)
    {
        if (stunde <= 4)
        {
            Subject[] morning = { Subject.SEW, Subject.INSY, Subject.SYT, Subject.NWT };
            return morning[new Random().Next(morning.Length)];
        }
        else
        {
            Subject[] afternoon = { Subject.D, Subject.E, Subject.M, Subject.NWT };
            return afternoon[new Random().Next(afternoon.Length)];
        }
    }

    static void PrintPlan(Dictionary<string, Subject[]> p)
    {
        foreach (var day in p)
        {
            Console.WriteLine($"{day.Key}: {string.Join(", ", day.Value)}");
        }
    }

    static Dictionary<string, Subject[]> CreateSupplPlan(Dictionary<string, Subject[]> original)
    {
        var copy = original.ToDictionary(x => x.Key, x => x.Value.ToArray());
        Random rnd = new();
        for (int i = 0; i < 5 ; i++) 
        { 
            string day = copy.Keys.ElementAt(rnd.Next(copy.Count));
            int hour = rnd.Next(copy[day].Length);
            copy[day][hour] = Subject.SUPPL;
        }
        return copy;
    }

    static Dictionary<string, Subject[]> ApplyStrategy(Dictionary<string, Subject[]> p, SubjectSelector selector) 
    { 
        var result = p.ToDictionary(x => x.Key, x => x.Value.ToArray());
        foreach (var day in result.Keys.ToList()) 
        { 
            for (int i = 0; i < result[day].Length; i++)
            {
                if (result[day][i] == Subject.SUPPL)
                    result[day][i] = selector(i, day);
            }
        }
        return result;
    }
}