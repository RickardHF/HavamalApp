using Havamal.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Parameters;
using Havamal.Resources.TextResources;
using Havamal.ViewModels;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitPage : ContentPage
    {
        public EventHandler SetUpFinished;
        private readonly Action<object, EventArgs> _finishedAction;

        public InitPage(Action<object, EventArgs> setUpFinished)
        {
            InitializeComponent();

            var bindingContext = Startup.ServiceProvider.GetService<InitPageModel>();
            _finishedAction = setUpFinished;

            bindingContext.SetUpFinished -= RunFinisher;
            bindingContext.SetUpFinished += RunFinisher;

            
            BindingContext = bindingContext;
        }

        private void RunFinisher(object sender, EventArgs args)
        {
            _finishedAction(sender, args);
        }

    }
}