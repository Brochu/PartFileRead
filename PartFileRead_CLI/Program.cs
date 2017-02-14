using System;
using System.IO;
using PartFileRead_Core;

public class Program
{
    public static void Main(string[] args)
    {
        bool create = false;

        if (create)
        {
            // TO CREATE A MAP
            ProgressionMap pmap = new ProgressionMap("ProgressionMap01", "DatMap");

            Puzzle p1 = new Puzzle("Puzzle01");
            p1.StartingWord = "PAWS";
            pmap.AddPuzzle(p1);
            Puzzle p2 = new Puzzle("Puzzle02");
            p2.StartingWord = "GORWS";
            pmap.AddPuzzle(p2);
            Puzzle p3 = new Puzzle("Puzzle03");
            p3.StartingWord = "DANGER";
            pmap.AddPuzzle(p3);
            Puzzle p4 = new Puzzle("Puzzle04");
            p4.StartingWord = "SHADOWS";
            pmap.AddPuzzle(p4);

            pmap.Save();
        }
        else
        {
            // TO LOAD A CREATED MAP
            ProgressionMap loaded = ProgressionMap.Load("DatMap");
            Puzzle p1 = loaded.LoadPuzzleById("Puzzle01");
            Puzzle p2 = loaded.LoadPuzzleById("Puzzle02");
            Puzzle p3 = loaded.LoadPuzzleById("Puzzle03");
            Puzzle p4 = loaded.LoadPuzzleById("Puzzle04");
            Console.WriteLine("Puzzle loaded");
        }
    }
}