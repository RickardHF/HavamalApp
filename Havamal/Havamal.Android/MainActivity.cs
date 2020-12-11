using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Havamal.Interfaces.Helpers;
using Havamal.Resources.Themes;

namespace Havamal.Droid
{
    [Activity(Label = "Havamal"
        //, Theme = "@style/MainTheme"
        //, MainLauncher = true
        , ConfigurationChanges = ConfigChanges.ScreenSize
        | ConfigChanges.Orientation 
        | ConfigChanges.UiMode 
        | ConfigChanges.ScreenLayout 
        | ConfigChanges.SmallestScreenSize
        , ScreenOrientation = Android.Content.PM.ScreenOrientation.Locked)
        ]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IThemeChanger
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //TabLayoutResource = Resource.Layout.Tabbar;
            //ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(Startup.InitApplication(this));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void ChangeTheme(HavamalTheme theme)
        {
            var changeTo = theme switch
            {
                HavamalTheme.Earth => Resource.Style.EarthTheme,
                HavamalTheme.Dark => Resource.Style.DarkTheme,
                HavamalTheme.Light => Resource.Style.HLightTheme,
                HavamalTheme.Water => Resource.Style.WaterTheme,
                _ => Resource.Style.MainTheme
            };

            SetTheme(changeTo);
        }
    }
}