using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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


        public static BindableProperty ImageSourceProperty = BindableProperty.Create(
            nameof(ImageSource)
            , typeof(string)
            , typeof(ThemeButton)
            , default(string)
            , propertyChanged: ImageSourceChanged
            );

        public static BindableProperty ClickedProperty = BindableProperty.Create(
            nameof(Clicked)
            , typeof(ICommand)
            , typeof(ThemeButton)
            , new Command(() => { })
            , propertyChanged: ClickedChanged
            );
        private static void TitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var c = (ThemeButton)bindable;
            c.ButtonTitle.Text = newValue.ToString();
        }

        private static void ImageSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var c = (ThemeButton)bindable;
            c.ButtonImage.Source = newValue.ToString();
        }

        private static void ClickedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var c = (ThemeButton)bindable;
            c.Clicked = (ICommand)newValue;
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

        public string ImageSource
        {
            get => (string)GetValue(ImageSourceProperty);

            set => SetValue(ImageSourceProperty, value);
        }

        public ICommand Clicked
        {
            get => (ICommand)GetValue(ClickedProperty);

            set => SetValue(ClickedProperty, value);
        }


        public Action OnClick;
        public ThemeButton()
        {
            InitializeComponent();

            ButtonImage.Source = ImageSource;
            ButtonTitle.Text = Title;
        }

        public void OnButtonClicked(object sender, EventArgs args)
        {
            if (OnClick != null) OnClick();
            if ((bool)(Clicked?.CanExecute(null))) Clicked.Execute(null); 
        }
    }
}