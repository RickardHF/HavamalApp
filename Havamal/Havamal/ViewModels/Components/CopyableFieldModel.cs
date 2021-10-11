using Havamal.Helpers;
using Havamal.Resources.Themes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Havamal.ViewModels.Components
{
    public class CopyableFieldModel : BasePageModel
    {
        public string ImageSource => (HavamalTheme)HavamalPreferences.Theme switch
        {
            HavamalTheme.Dark => "copyDARK.xml",
            HavamalTheme.Light => "copyLIGHT.xml",
            HavamalTheme.Water => "copyWATER.xml",
            HavamalTheme.Earth => "copyEARTH.xml",
            _ => ""
        };
    }
}
