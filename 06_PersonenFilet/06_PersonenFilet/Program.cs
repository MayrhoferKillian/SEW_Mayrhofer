using System;
using System.Collections.Generic;

namespace _06_PersonenFilter
{
    public class Person
    {
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public int Geburtsjahr { get; set; }

        public Person(string vorname, string nachname, int geburtsjahr) 
        {
            Vorname = vorname;
            Nachname = nachname;   
            Geburtsjahr = geburtsjahr;
        }

        public override string ToString()
        {
            return $"{Vorname}, {Nachname}, {Geburtsjahr}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Person> personen = new List<Person>()
            {
                new Person("Anna", "Müller", 1995),
                new Person("Max", "Schmidt", 2001),
                new Person("Alex", "Meier", 1999),
                new Person("Lisa", "Bauer", 2003),
                new Person("Andreas", "Fischer", 1988)
            };

            List<Person> personenMitA = personen.FindAll(p => p.Vorname.StartsWith("A"));
            Console.WriteLine("Personen mit Vornamen, die mit 'A' beginnen");
            foreach (var p in personenMitA)
            {
                Console.WriteLine(p);
            }

            List<Person> personenNach2000 = personen.FindAll(p => p.Geburtsjahr > 2000);
            Console.WriteLine("Personen die nach 2000 geboren sind");
            foreach (var p in personenNach2000)
            {
                Console.WriteLine(p);
            }
        }
    }
}