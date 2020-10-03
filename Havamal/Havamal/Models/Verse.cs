using System;
using System.Collections.Generic;
using System.Text;

namespace Havamal.Models
{
    public class Verse
    {
        public int VerseId { get; }
        public int LanguageId { get; }
        public string Content { get; }

        public Verse (int verseId, int languageId, string content)
        {
            VerseId = verseId;
            LanguageId = languageId;
            Content = content;
        }
    }
}
