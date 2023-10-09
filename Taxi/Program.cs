// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Taxi.Datasource;

class Program


{
    static void Main()
    {

        // Fájlbeolvasás

        List<Fuvar> fuvarok = new List<Fuvar>();

        string[] lines = File.ReadAllLines(".\\Datasource\\fuvar.csv");

        for (var index = 1; index < lines.Length; index++)
        {
            string[] line = lines[index].Split(';');

            int id = int.Parse(line[0]);
            DateTime indulas = DateTime.Parse(line[1]);
            int idotartam = int.Parse(line[2]);
            double megtettUt = double.Parse(line[3]);
            double viteldij = double.Parse(line[4]);
            double borravalo = double.Parse(line[5]);
            string fizetesiMod = line[6];

            Fuvar fuvarSor = new(id, indulas, idotartam, megtettUt, viteldij, borravalo, fizetesiMod);

            fuvarok.Add(fuvarSor);

        }

        Console.WriteLine();


        // 3. feladat:

        Console.WriteLine($"3. feladat: {fuvarok.Count} fuvar");

        // 4. feladat: 

        double bevetel = fuvarok.Where(x => x.Id == 6185).Sum(x => x.Viteldij + x.Borravalo);
        int fuvarokSzama = fuvarok.Where(x => x.Id == 6185).Count();
        var szukitett = fuvarok.Where(x => x.Id == 6185);
        bevetel = szukitett.Sum(x => x.Viteldij + x.Borravalo);
        fuvarokSzama = szukitett.Count();

        Console.WriteLine($"4. feladat:  {fuvarokSzama}  alatt:  {bevetel}$");

        //5.

        Console.WriteLine("5.feladat: ");

        var fizetesiModok = fuvarok.Select(x => x.FizetesiMod).Distinct();

        foreach (var fizetesiMod in fizetesiModok)
        {
            int szamlalo = fuvarok.Count(x => x.FizetesiMod == fizetesiMod);
            Console.WriteLine($"\t{fizetesiMod}: {szamlalo} fuvar");
        }

        // groupby megoldás

        var csoportok = fuvarok.GroupBy(x => x.FizetesiMod);

        foreach (var csoport in csoportok)
        {
            
            Console.WriteLine($"\t{csoport.Key}: {csoport.Count()} fuvar");
        }

        // 6. 

        Console.WriteLine($"6. feladat:  {Math.Round(fuvarok.Select(x => x.MegtettUt).Sum() * 1.6, 2)}km");

        Console.WriteLine("6. feladat:");
        double osszesKilometer = fuvarok.Sum(f => f.MegtettUt * 1.6);
        osszesKilometer = Math.Round(osszesKilometer, 2);
        Console.WriteLine($"Összesen megtett távolság: {osszesKilometer} km");

        // 7. 

        Console.WriteLine("Leghosszabb fuvar:");

        var leghosszabbFuvar = fuvarok.OrderByDescending(x => x.Idotartam).FirstOrDefault();
        leghosszabbFuvar = fuvarok.MaxBy(x => x.Idotartam);

        Console.WriteLine($"\tFuvar hossza: {leghosszabbFuvar.Idotartam} másodperc");
        Console.WriteLine($"\tTaxi azonosító: {leghosszabbFuvar.Id}");
        Console.WriteLine($"\tMegtett távolság: {leghosszabbFuvar.MegtettUt} km");
        Console.WriteLine($"\tViteldíj: {leghosszabbFuvar.Viteldij}$");

        // 8. 

        List<Fuvar> hibasFuvarok = fuvarok
        .Where(f => f.Idotartam > 0 && f.Viteldij > 0 && f.MegtettUt == 0)
        .OrderBy(f => f.Indulas)
        .ToList();

        using (StreamWriter sw = new StreamWriter("hibak.txt", false, System.Text.Encoding.UTF8))
        {
            foreach (var hibasFuvar in hibasFuvarok)
            {
                string sor = $"{hibasFuvar.Id};{hibasFuvar.Indulas:yyyy-MM-dd HH:mm:ss};" +
                    $"{hibasFuvar.Idotartam};{hibasFuvar.MegtettUt.ToString("0.00")}, ;" +
                    $"{hibasFuvar.Viteldij.ToString("0.00")}, ;" +
                    $"{hibasFuvar.Borravalo.ToString("0.00")};" +
                    $"{hibasFuvar.FizetesiMod}";
                sw.WriteLine(sor);
            }
        }

        Console.WriteLine();

    }
}
