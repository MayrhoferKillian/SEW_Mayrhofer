using System;

namespace FantasyKampf
{
    public class Charakter
    {
        public string Name { get; set; }
        public int Leben { get; set; }
        public int Angriffskraft { get; set; }

        public Charakter(string name, int leben, int angriffskraft)
        {
            Name = name;
            Leben = leben;
            Angriffskraft = angriffskraft;
        }

        public void StatusAnzeigen()
        {
            Console.WriteLine($"{Name} - Leben: {Leben}, Angriffskraft: {Angriffskraft}");
        }
    }

    public delegate void KampfAktion(Charakter angreifer, Charakter ziel);

    class Program
    {
        public static void NormalerAngriff(Charakter angreifer, Charakter ziel)
        {
            ziel.Leben -= angreifer.Angriffskraft;
            if (ziel.Leben < 0) ziel.Leben = 0;
            Console.WriteLine($"{angreifer.Name} greift {ziel.Name} an und verursacht {angreifer.Angriffskraft} Schaden.");
        }

        public static void Heilen(Charakter angreifer, Charakter ziel)
        {
            angreifer.Leben += 10;
            if (angreifer.Leben > 100) angreifer.Leben = 100;

            ziel.Leben += 5;
            if (ziel.Leben > 100) ziel.Leben = 100;

            Console.WriteLine($"{angreifer.Name} heilt sich (+10) und {ziel.Name} (+5).");
        }

        public static void SpezialAngriff(Charakter angreifer, Charakter ziel)
        {
            int schaden = angreifer.Angriffskraft * 2;
            ziel.Leben -= schaden;
            if (ziel.Leben < 0) ziel.Leben = 0;

            angreifer.Leben -= 5;
            if (angreifer.Leben < 0) angreifer.Leben = 0;

            Console.WriteLine($"{angreifer.Name} führt einen Spezialangriff auf {ziel.Name} aus ({schaden} Schaden), verliert dabei selbst 5 Leben.");
        }

        public static void FuehreAktionAus(Charakter angreifer, Charakter ziel, KampfAktion aktion)
        {
            aktion(angreifer, ziel);
            angreifer.StatusAnzeigen();
            ziel.StatusAnzeigen();
            Console.WriteLine("---------------------------------");
        }

        static void Main(string[] args)
        {
            Charakter held = new Charakter("Held", 100, 20);
            Charakter drache = new Charakter("Drache", 150, 25);

            Random rnd = new Random();
            bool spielLäuft = true;
            int runde = 1;

            while (spielLäuft)
            {
                Console.WriteLine($"\n--- Runde {runde} ---");
                held.StatusAnzeigen();
                drache.StatusAnzeigen();

                Console.WriteLine("\nWähle Aktion für Held: 1 = Angriff, 2 = Heilen, 3 = Spezialangriff");
                string eingabe = Console.ReadLine();
                KampfAktion aktionHeld = null;

                switch (eingabe)
                {
                    case "1": aktionHeld = NormalerAngriff; break;
                    case "2": aktionHeld = Heilen; break;
                    case "3": aktionHeld = SpezialAngriff; break;
                    default: Console.WriteLine("Ungültige Eingabe, normaler Angriff wird ausgeführt."); aktionHeld = NormalerAngriff; break;
                }

                FuehreAktionAus(held, drache, aktionHeld);

                if (drache.Leben <= 0)
                {
                    Console.WriteLine("Der Held hat gewonnen!");
                    break;
                }

                int aktionDracheIndex = rnd.Next(1, 4);
                KampfAktion aktionDrache = aktionDracheIndex switch
                {
                    1 => NormalerAngriff,
                    2 => Heilen,
                    3 => SpezialAngriff,
                    _ => NormalerAngriff
                };

                FuehreAktionAus(drache, held, aktionDrache);

                // Prüfen ob Held besiegt
                if (held.Leben <= 0)
                {
                    Console.WriteLine("Der Drache hat gewonnen!");
                    break;
                }

                runde++;
            }

            Console.WriteLine("Spiel beendet.");
        }
    }
}
