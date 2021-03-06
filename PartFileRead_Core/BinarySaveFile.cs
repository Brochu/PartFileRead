﻿using System;
using System.IO;

namespace PartFileRead_Core
{
    public class BinarySaveFile
    {
        public string Name
        {
            get { return _name; }
        }

        public bool Ready
        {
            get { return _ready; }
        }

        private string _name;
        private string _filepath;
        private bool _ready;
        private BinaryFileHeader _header;
        private const string FILE_EXT = ".bin";

        public BinarySaveFile(string filename)
        {
            _name = filename;
            _filepath = string.Format("{0}{1}", filename, FILE_EXT);
            _ready = false;

            _header = new BinaryFileHeader();
        }

        public void Save(byte[] data)
        {
            using (FileStream fs = File.Open(_filepath, FileMode.Create, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                byte[] headerBuffer = _header.GetBuffer();

                bw.Write(_header.Size);
                bw.Write(headerBuffer);
                bw.Write(data);
            }
        }

        public void LoadHeader()
        {
            using(FileStream fs = File.Open(_filepath, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                byte[] headerData = new byte[br.ReadInt64()];
                br.Read(headerData, 0, headerData.Length);

                _header = BinaryFileHeader.Parse(headerData);
            }
            _ready = true;
        }

        public void AddHeaderEntry(string name, long offset, long size)
        {
            _header.AddEntry(name, offset, size);
        }

        public void RemoveHeaderEntry(string name)
        {
            _header.RemoveEntry(name);
        }

        public byte[] LoadEntry(string name)
        {
            if (!_ready) return new byte[0];

            byte[] result = new byte[_header.GetSizeForEntry(name)];
            using (FileStream fs = File.Open(_filepath, FileMode.Open, FileAccess.Read))
            {
                // We need to offset for header size (long) + header + offet for entry
                fs.Seek(sizeof(long) + _header.Size + _header.GetOffsetForEntry(name), SeekOrigin.Begin);
                fs.Read(result, 0, result.Length);
            }
            return result;
        }
    }
}