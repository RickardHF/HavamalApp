using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal.Views.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThemeButton : Frame
    {
        public static BindableProperty TitleProperty = BindableProperty.Create(
            nameof(Title)
            , typeof(string)
            , typeof(ThemeButton)
            , default(string)
            , Xamarin.Forms.BindingMode.OneWay
            , propertyChanged: TitleChanged
            );


        public static BindableProperty DescriptionProperty = BindableProperty.Create(
            nameof(Description)
            , typeof(string)
            , typeof(ThemeButton)
            , default(string)
            , Xamarin.Forms.BindingMode.TwoWay
            , propertyChanged: DescriptionChanged
            );

        public static BindableProperty ImageSourceProperty = BindableProperty.Create(
            nameof(ImageSource)
            , typeof(string)
            , typeof(ThemeButton)
            , default(string)
            , propertyChanged: ImageSourceChanged
            );
        private static void TitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var c = (ThemeButton)bindable;
            c.ButtonTitle.Text = newValue.ToString();
        }

        private static void DescriptionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var c = (ThemeButton)bindable;
            c.ButtonDesctiption.Text = newValue.ToString();
        }

        private static void ImageSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var c = (ThemeButton)bindable;
            c.ButtonImage.Source = newValue.ToString();
        }

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

        public string Description
        {
            get
            {
                return (string)GetValue(DescriptionProperty);
            }

            set
            {
                SetValue(DescriptionProperty, value);
            }
        }

        public string ImageSource
        {
            get
            {
                return (string)GetValue(ImageSourceProperty);
            }

            set
            {
                SetValue(ImageSourceProperty, value);
            }
        }


        public Action OnClick;
        public ThemeButton()
        {
            InitializeComponent();

            ButtonImage.Source = ImageSource;
            ButtonTitle.Text = Title;
            ButtonDesctiption.Text = Description;
        }

        public void OnButtonClicked(object sender, EventArgs args)
        {
            OnClick();
        }
    }
}