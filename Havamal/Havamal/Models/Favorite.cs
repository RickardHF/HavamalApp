using System;
using System.Collections.Generic;
using System.Text;

namespace Havamal.Models
{
    public class Favorite
    {
        public int VerseId { get; }

        public Favorite(int verseId)
        {
            VerseId = verseId;
        }
    }
}
