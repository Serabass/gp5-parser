using GTP5Parser.Binary;
using System.Collections.Generic;

namespace GTP5Parser.Tabs
{
    public enum NoteName
    {
        C,
        D,
        E,
        F,
        G,
        A,
        B
    }

    public struct NoteNameConstructorArgs
    {
        public NoteName note;
        public bool diez;
    }
    
    public class Note
    {
        public class MemoryBlock : MemoryBlock<Note>
        {
            public MemoryBlock(Note data) : base(data)
            {
            }
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

            public NoteInfo(NoteNameConstructorArgs args)
            {
                note = args.note;
                diez = args.diez;
            }
        }

        public int note;
        public bool diez;
        public int octave = 1;

        public NoteName noteName;

        private readonly NoteInfo[] notesInfo = new NoteInfo[]
        {
            new NoteInfo(note: NoteName.C, diez: false),
            new NoteInfo(note: NoteName.C, diez: true),
            new NoteInfo(note: NoteName.D, diez: false),
            new NoteInfo(note: NoteName.D, diez: true),
            new NoteInfo(note: NoteName.E, diez: false),
            new NoteInfo(note: NoteName.F, diez: false),
            new NoteInfo(note: NoteName.F, diez: true),
            new NoteInfo(note: NoteName.G, diez: false),
            new NoteInfo(note: NoteName.G, diez: true),
            new NoteInfo(note: NoteName.A, diez: false),
            new NoteInfo(note: NoteName.A, diez: true),
            new NoteInfo(note: NoteName.B, diez: false),
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
            octave = (int) System.Math.Floor(note / 12f);
        }

        public new string ToString()
        {
            var diezString = diez ? "#" : "";
            return $"{noteName}{diezString}{octave}";
        }
    }
}