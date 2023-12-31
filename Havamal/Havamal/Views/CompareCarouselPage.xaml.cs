﻿using Havamal.Helpers;
using Havamal.ViewModels;
using Havamal.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompareCarouselPage : ContentPage
    {
        private readonly CompareCarouselPageModel _pageModel;
        public CompareCarouselPage()
        {
            InitializeComponent();
            var bindingContext = Startup.ServiceProvider.GetService<CompareCarouselPageModel>();

            bindingContext.ChangePos = ChangeVerse;

            _pageModel = bindingContext;

            from.OnClick = () => FromClicked(new object(), null);
            to.OnClick = () => ToClicked(new object(), null);

            BindingContext = bindingContext;

            ComparisonsView.ItemsSource = _pageModel.Comparisons;
            ComparisonsView.Loop = false;
            ComparisonsView.IsScrollAnimated = false;

        }

        private void ChangeVerse(int verseId)
        {
            var pos = _pageModel.VersePosition(verseId);
            if (pos < 0) return;
            _pageModel.SaveChange = false;
            ComparisonsView.Position = pos;
            _pageModel.SaveChange = true;
        }

        private void TapVerseId(object sender, EventArgs e)
        {
            var page = new VerseChoicePopup(i =>
            {
                ChangeVerse(i);
            }, _pageModel.Comparisons.Count);
            Navigation.PushPopupAsync(page);
        }

        

        private void FromClicked(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(new LanguageChoicePopup(
                _pageModel.FromLanguages.ToList()
                , (value) => _pageModel.CurrentFromLanguage = value
                , null
                , false));
        }

        private void ToClicked(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(new LanguageChoicePopup(
                _pageModel.ToLanguages.ToList()
                , (value) => _pageModel.CurrentToLanguage = value
                , null
                , false));
        }
    }
}