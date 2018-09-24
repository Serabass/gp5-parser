using System;
using System.Collections.Generic;
using System.IO;
using GTP5Parser.Binary;
using GTP5Parser.Tabs.Structure;

namespace GTP5Parser.Tabs
{
    partial class TabReader : MyBinaryReader
    {
        readonly Stream stream;

        public static string[] SupportedVersions = {
            "v5.10"
        };

        private List<TrackMeta> TrackMetaArray = new List<TrackMeta>();
        private int TrackMetaIterator;

        private bool atEnd => BaseStream.Position >= BaseStream.Length;

        public static Tab ReadTabFromStream(Stream stream)
        {
            return new TabReader(stream).ReadTab();
        }

        public TabReader(Stream stream) : base(stream)
        {
            this.stream = stream;
        }

        public Tab ReadTab()
        {
            using (var tab2 = ReadStruct<Tab>(ReadStructTab).Value)
            {
                return tab2;
            }
        }

        public Color ReadColor()
        {
            var color = ReadBytes(3);
            return Color.FromBytes(color.Value);
        }
    }
}
