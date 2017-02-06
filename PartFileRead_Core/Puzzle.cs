using System;
using System.Collections.Generic;
using System.IO;

namespace PartFileRead_Core
{
    public class Puzzle
    {
        public string Id
        {
            get { return _id; }
        }

        public string StartingWord
        {
            get { return _startWord; }
            set
            {
                _startWord = value;
                _words = ComputeWordList(_startWord);
            }
        }

        public List<string> Words
        {
            get { return _words; }
        }

        private string _id;
        private string _startWord;
        private List<string> _words;

        private Puzzle()
        { }

        public Puzzle(string id)
        {
            _id = id;
        }

        public byte[] SerializeBinary(out int size)
        {
            byte[] result;
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(_id);
                bw.Write(_startWord);
                bw.Write(_words.Count);
                foreach (string w in _words)
                {
                    bw.Write(w);
                }

                result = ms.GetBuffer();
                size = result.Length;
            }

            return result;
        }

        public static Puzzle DeserializeBinary(byte[] data)
        {
            Puzzle result = new Puzzle();
            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader br = new BinaryReader(ms))
            {
                result._id = br.ReadString();
                result._startWord = br.ReadString();

                result._words = new List<string>();
                int wordsCount = br.ReadInt32();
                for (int i = 0; i < wordsCount; ++i)
                {
                    result._words.Add(br.ReadString());
                }
            }
            return result;
        }

        private List<string> ComputeWordList(string start)
        {
            //TODO: Actual algo
            return new List<string>()
            { "ROW", "SOW", "GROW", "ROWS", "GROWS" };
        }
    }
}