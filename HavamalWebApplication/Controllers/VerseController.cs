using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HavamalWebApplication.Controllers
{
    public class VerseController
    {
    }

    public record Verse
    {
        public string Content { get; }
        public Verse(string content) => Content = content;
    }
}
