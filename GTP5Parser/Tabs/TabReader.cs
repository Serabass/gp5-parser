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

        private bool AtEnd => BaseStream.Position >= BaseStream.Length;

        public string Path;
        
        public static TabReader FromStream(Stream stream)
        {
            return new TabReader(stream, null);
        }

        public static Tab ReadTabFromStream(Stream stream)
        {
            return FromStream(stream).ReadTab();
        }

        public TabReader(Stream stream, string path) : base(stream)
        {
            this.stream = stream;
            Path = path;
        }

        public Tab ReadTab()
        {
            using (var tab2 = ReadStruct<Tab>(ReadStructTab))
            {
                return tab2.Value;
            }
        }

        public Color ReadColor()
        {
            return Color.FromBytes(this << 3);
        }
    }
}
