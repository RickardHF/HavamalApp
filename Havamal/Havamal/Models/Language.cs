using System;
using System.Collections.Generic;
using System.Text;

namespace Havamal.Models
{
    public class Language
    {
        public int Id { get; }
        public string Name { get; }
        public string Authors { get; }
        public string LanguageCode { get; }
        public Language(int id, string name, string languageCode, string authors)
        {
            Id = id;
            Name = name;
            LanguageCode = languageCode;
            Authors = authors;
        }


        public override string ToString()
        {
            return $"{LanguageCode} - {Name}";
        }
    }
}
