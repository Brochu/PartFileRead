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
            return ComputePermutations(start);
        }

        private static List<string> ComputePermutations(string s)
        {
            int n = s.Length;
            char[] a = new char[n];
            for (int i = 0; i < n; i++)
            {
                a[i] = s[i];
            }

            List<string> words = new List<string>();
            InnerPermutations(a, n, ref words);
            return words;
        }

        private static void InnerPermutations(char[] a, int n, ref List<string> words)
        {
            if (n == 1) {
                words.Add(new string(a));
                return;
            }
            for (int i = 0; i < n; i++)
            {
                swap(a, i, n-1);
                InnerPermutations(a, n-1, ref words);
                swap(a, i, n-1);
            }
        }  

        // swap the characters at indices i and j
        private static void swap(char[] a, int i, int j)
        {
            char c = a[i];
            a[i] = a[j];
            a[j] = c;
        }
    }
}