using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Havamal.Resources.Themes;
using Havamal.Helpers;
using Havamal.ViewModels.Components;

namespace Havamal.Views.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CopyableField : Frame
    {
        public CopyableField()
        {
            BindingContext = new CopyableFieldModel();
            InitializeComponent();

        }
        public static BindableProperty TitleProperty = BindableProperty.Create(
            nameof(Title)
            , typeof(string)
            , typeof(ThemeButton)
            , default(string)
            , Xamarin.Forms.BindingMode.OneWay
            , propertyChanged: TitleChanged
            );

        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }

            set
            {
                SetValue(TitleProperty, value);
            }
        }

        private static void TitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var c = (CopyableField)bindable;
            c.TitleField.Text = newValue.ToString();
        }

        private async void Copy_Clicked(object sender, EventArgs e)
        {
            var text = TitleField.Text;
            await Clipboard.SetTextAsync(text);
        }
    }
}