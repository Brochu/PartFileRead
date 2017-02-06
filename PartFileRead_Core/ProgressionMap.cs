using System;
using System.Collections.Generic;
using System.IO;

namespace PartFileRead_Core
{
    public class ProgressionMap
    {
        public string Id
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
        }

        public int PuzzleCount
        {
            get { return _puzzles.Count; }
        }

        private string _id;
        private string _name;
        private List<Puzzle> _puzzles;
        private BinarySaveFile _file;

        public ProgressionMap()
            : this(string.Empty, string.Empty)
        { }

        public ProgressionMap(string id, string name)
        {
            _id = id;
            _name = name;

            _puzzles = new List<Puzzle>();
        }

        public void AddPuzzle(Puzzle p)
        {
            if (!_puzzles.Contains(p))
            {
                _puzzles.Add(p);
            }
        }

        public void RemovePuzzle(Puzzle p)
        {
            if (_puzzles.Contains(p))
            {
                _puzzles.Remove(p);
            }
        }

        public void Save()
        {
            _file = new BinarySaveFile(_id);
            byte[] filedata;

            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(_id);
                bw.Write(_name);

                // save all puzzle, add header entry for each puzzle
                foreach (Puzzle p in _puzzles)
                {
                    long offset = ms.Position;
                    int size = 0;
                    bw.Write(p.SerializeBinary(out size));
                    _file.AddHeaderEntry(p.Id, offset, size);
                }
                filedata = ms.GetBuffer();
            }
            _file.Save(filedata);
        }

        public void Load()
        {
        }
    }
}