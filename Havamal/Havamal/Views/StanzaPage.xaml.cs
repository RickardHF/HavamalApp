using Havamal.ViewModels;
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
    public partial class StanzaPage : TabbedPage
    {
        public StanzaPage()
        {
            InitializeComponent();

            var bindingContext = Startup.ServiceProvider.GetService<VersePageModel>();
            BindingContext = bindingContext;
;
        }
    }
}