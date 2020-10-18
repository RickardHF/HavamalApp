using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitPage : ContentPage
    {
        public EventHandler SetUpFinished;

        public InitPage()
        {
            InitializeComponent();
            SetUpDb();
        }

        private void OnSetupFinished(EventArgs args)
        {
            var handler = SetUpFinished;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public async void SetUpDb()
        {
            string dbName = "HavamalVerses.db";
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
            // Check if your DB has already been extracted.
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }
            using (BinaryReader br = new BinaryReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Verses.db")))
            {
                using (BinaryWriter bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                {
                    byte[] buffer = new byte[2048];
                    int len = 0;
                    while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        bw.Write(buffer, 0, len);
                    }
                }
            }

            await Task.Delay(1000);

            OnSetupFinished(EventArgs.Empty);
        }
    }
}