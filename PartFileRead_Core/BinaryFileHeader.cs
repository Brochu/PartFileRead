using System;
using System.Collections.Generic;
using System.IO;

namespace PartFileRead_Core
{
    public class BinaryFileHeader
    {
        public struct HeaderEntry
        {
            public long Offset
            {
                get { return _offset; }
            }

            public long Size
            {
                get { return _size; }
            }

            private long _offset;
            private long _size;

            public HeaderEntry(long offset, long size)
            {
                _offset = offset;
                _size = size;
            }
        }

        public long Size
        {
            get { return _size; }
        }

        private long _size;
        private Dictionary<string, HeaderEntry> _database;

        public BinaryFileHeader()
        {
            _size = 0;
            _database = new Dictionary<string, HeaderEntry>();
        }

        public static BinaryFileHeader Parse(byte[] headerData)
        {
            BinaryFileHeader header = new BinaryFileHeader();
            header._size = headerData.Length;
            header._database = new Dictionary<string, HeaderEntry>();

            using (MemoryStream ms = new MemoryStream(headerData))
            using (BinaryReader br = new BinaryReader(ms))
            {
                int entryCount = br.ReadInt32();

                string name;
                long offset;
                long size;
                for (int i = 0; i < entryCount; ++i)
                {
                    name = br.ReadString();
                    offset = br.ReadInt64();
                    size = br.ReadInt64();

                    header._database.Add(name, new HeaderEntry(offset, size));
                }
            }

            return header;
        }

        public byte[] GetBuffer()
        {
            byte[] headerBuffer;

            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(_database.Count);
                foreach (KeyValuePair<string, HeaderEntry> entry in _database)
                {
                    bw.Write(entry.Key);
                    bw.Write(entry.Value.Offset);
                    bw.Write(entry.Value.Size);
                }
                headerBuffer = ms.GetBuffer();
                _size = headerBuffer.Length;
            }
            return headerBuffer;
        }

        public long GetOffsetForEntry(string name)
        {
            if (!_database.ContainsKey(name))
            {
                throw new InvalidOperationException("Header entry name not found");
            }
            return _database[name].Offset;
        }

        public long GetSizeForEntry(string name)
        {
            if (!_database.ContainsKey(name))
            {
                throw new InvalidOperationException("Header entry name not found");
            }
            return _database[name].Size;
        }

        public void AddEntry(string name, long offset, long size)
        {
            if (_database.ContainsKey(name))
            {
                throw new InvalidOperationException("Name already in database");
            }
            HeaderEntry added = new HeaderEntry(offset, size);
            _database.Add(name, added);
        }

        public void RemoveEntry(string name)
        {
            if (!_database.ContainsKey(name))
            {
                throw new InvalidOperationException("Name not found in database");
            }
            _database.Remove(name);
        }
    }
}