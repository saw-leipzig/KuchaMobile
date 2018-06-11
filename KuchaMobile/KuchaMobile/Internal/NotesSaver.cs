using System;
using System.Collections.Generic;
using System.Text;

namespace KuchaMobile.Internal
{
    public class NotesSaver
    {
        public enum NOTES_TYPE
        {
            NOTE_TYPE_CAVE,
            NOTE_TYPE_PAINTEDREPRESENTATION,
            NOTES_TYPE_IMAGE
        }

        public int ID { get; set; }
        public NOTES_TYPE Type { get; set; }
        public string Note { get; set; }

        public NotesSaver(NOTES_TYPE type, int ID, string note)
        {
            this.Type = type;
            this.ID = ID;
            this.Note = note;
        }

    }
}
