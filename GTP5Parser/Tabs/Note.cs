
using GTP5Parser.Binary;
using System.Collections.Generic;

namespace GTP5Parser.Tabs
{
    public enum NoteName
    {
        C, D, E, F, G, A, B
    }

    public class Note
    {
        public class MemoryBlock : MemoryBlock<Note> {
            public MemoryBlock(Note data) : base(data) { }
        };

        private class NoteInfo
        {
            public NoteName note;
            public bool diez;

            public NoteInfo(NoteName note, bool diez)
            {
                this.note = note;
                this.diez = diez;
            }
        }

        public int note;
        public bool diez;
        public int octave = 1;

        private readonly NoteInfo[] notesInfo = new NoteInfo[] {
            new NoteInfo(NoteName.C, false),
            new NoteInfo(NoteName.C, true),
            new NoteInfo(NoteName.D, false),
            new NoteInfo(NoteName.D, true),
            new NoteInfo(NoteName.E, false),
            new NoteInfo(NoteName.F, false),
            new NoteInfo(NoteName.F, true),
            new NoteInfo(NoteName.G, false),
            new NoteInfo(NoteName.G, true),
            new NoteInfo(NoteName.A, false),
            new NoteInfo(NoteName.A, true),
            new NoteInfo(NoteName.B, false),
        };

        public static Note From(int note)
        {
            return new Note(note);
        }

        public Note(int note)
        {
            this.note = note;

            var mod = note % 12;
            var data = notesInfo[mod];
            noteName = data.note;
            diez = data.diez;
            octave = (int)System.Math.Floor(note / 12f);
        }

        public NoteName noteName;

        public new string ToString()
        {
            string diezString = diez ? "#" : "";
            return $"{noteName}{diezString}{octave}";
        }
    }
}
