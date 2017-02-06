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
            Puzzle[] puzzles = new Puzzle[]
            {
                new Puzzle("Puzzle01"),
                new Puzzle("Puzzle02"),
                new Puzzle("Puzzle03"),
                new Puzzle("Puzzle04"),
                new Puzzle("Puzzle05")
            };
            ProgressionMap pmap = new ProgressionMap("ProgressionMap01", "DatMap");

            foreach (Puzzle p in puzzles)
            {
                p.StartingWord = "GORWS";
                pmap.AddPuzzle(p);
            }

            pmap.Save();
        }
        else
        {
            // TO LOAD A CREATED MAP
            ProgressionMap loaded = ProgressionMap.Load("DatMap");
            Puzzle test = loaded.LoadPuzzleById("Puzzle01");
        }
    }
}