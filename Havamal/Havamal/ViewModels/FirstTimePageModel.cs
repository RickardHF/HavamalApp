using Havamal.Views.FirstPageViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Havamal.ViewModels
{
    public class FirstTimePageModel : BasePageModel
    {
        private int MinIndex { get; set; }
        private int MaxIndex { get; set; }
        public float Progress { get {
                return SelectedItem != null && MaxIndex > 0 ? (float) SelectedItem.Index / (float) MaxIndex : 0.0f;
            } 
        }
        public bool VisibleBack { get; set; } = false;
        public bool VisibleNext { get; set; } = true;
        public bool VisibleFinish { get; set; } = false;
        private EventHandler PageChangeEvent { get; set; }
        public ViewListItem SelectedItem { get; set; }

        public List<ViewListItem> Items { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand FinishCommand { get; set; }
        public ICommand NextCommand { get; set; }
        
        
        public FirstTimePageModel()
        {
            Items = new List<ViewListItem>(){
                new ViewListItem{Index = 1, View = new WelcomeView()}
                , new ViewListItem{Index = 2, View = new AppLanguageView()}
                , new ViewListItem{Index = 3, View = new ThemeView()}
            }.OrderBy(x => x.Index).ToList();

            NextCommand = new Command(execute: OnNextPage);
            FinishCommand = new Command(execute: OnFinish);
            BackCommand = new Command(execute: OnPreviousPage);

            MaxIndex = Items.Max(x => x.Index);
            MinIndex = Items.Min(x => x.Index);

            SelectedItem = Items.FirstOrDefault(x => x.Index == MinIndex);
            
            PageChangeEvent -= PageChanged;
            PageChangeEvent += PageChanged;
        }

        public void PageChanged(object sender, EventArgs e)
        {
            VisibleBack = SelectedItem.Index > MinIndex;
            VisibleNext = SelectedItem.Index < MaxIndex;
            VisibleFinish = SelectedItem.Index >= MaxIndex;

            OnPropertyChanged(nameof(VisibleNext));
            OnPropertyChanged(nameof(VisibleBack));
            OnPropertyChanged(nameof(VisibleFinish));
            OnPropertyChanged(nameof(Progress));
        }

        public void OnNextPage()
        {
            var newInd = Math.Min(SelectedItem.Index + 1, MaxIndex);
            var newPage = Items.FirstOrDefault(x => x.Index == newInd);
            if (newPage != null) SelectedItem = newPage;
            OnPropertyChanged("SelectedItem");

            PageChangeEvent.Invoke(this, new EventArgs());
        }

        public void OnPreviousPage()
        {
            var newInd = Math.Max(SelectedItem.Index - 1, MinIndex);
            var newPage = Items.FirstOrDefault(x => x.Index == newInd);
            if (newPage != null) SelectedItem = newPage;
            OnPropertyChanged("SelectedItem");

            PageChangeEvent.Invoke(this, new EventArgs());
        }
        public void OnFinish()
        {

        }
    }
}
