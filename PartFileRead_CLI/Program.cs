using System;
using PartFileRead_Core;

public class Program
{
    public static void Main(string[] args)
    {
        Puzzle[] puzzles = new Puzzle[]
        {
            new Puzzle(Guid.NewGuid().ToString()),
            new Puzzle(Guid.NewGuid().ToString()),
            new Puzzle(Guid.NewGuid().ToString()),
            new Puzzle(Guid.NewGuid().ToString()),
            new Puzzle(Guid.NewGuid().ToString())
        };
        ProgressionMap pmap = new ProgressionMap(Guid.NewGuid().ToString(), "DatMap");

        foreach (Puzzle p in puzzles)
        {
            p.StartingWord = "GORWS";
            pmap.AddPuzzle(p);
        }

        pmap.Save();

        Console.ReadKey();
    }
}