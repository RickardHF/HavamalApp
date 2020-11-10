using Havamal.Resources.Sections;
using Havamal.Resources.TextResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Havamal.Helpers
{
    public static class SectionHelpers
    {
        public static HavamalSection GetSection(this int verseId)
        {
            if (verseId < 83) return HavamalSection.Gestathattr;
            else if (verseId < 111) return HavamalSection.Women;
            else if (verseId < 139) return HavamalSection.Loddfafnismal;
            else if (verseId < 146) return HavamalSection.Runaljod;
            return HavamalSection.Ljodathal;
        }

        public static string GetSectionString(this HavamalSection section)
        {
            switch (section)
            {
                case HavamalSection.Gestathattr:
                    return AppResources.SectionGas;
                case HavamalSection.Women:
                    return AppResources.SectionWom;
                case HavamalSection.Loddfafnismal:
                    return AppResources.SectionLod;
                case HavamalSection.Runaljod:
                    return AppResources.SectionRun;
                case HavamalSection.Ljodathal:
                    return AppResources.SectionLjo;
                default:
                    return "";
            }
        }
    }
}
