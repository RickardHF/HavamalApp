using Havamal.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Parameters;
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

        private readonly IVerseRepository _verseRepository;
        private readonly IVerseSetupRepository _verseSetupRepository;
        private readonly DataSettings _dataSettings;

        public InitPage()
        {
            InitializeComponent();

            _verseRepository = Startup.ServiceProvider.GetService<IVerseRepository>();
            _verseSetupRepository = Startup.ServiceProvider.GetService<IVerseSetupRepository>();
            _dataSettings = Startup.ServiceProvider.GetService<DataSettings>();

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
            var dbPath = _dataSettings.DbBasePath;

            string tempName = HavamalPreferences.SetupDbName;
            string tempPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), tempName);
            // Check if your DB has already been extracted.
            if (File.Exists(tempName))
            {
                File.Delete(tempName);
            }
            using (BinaryReader br = new BinaryReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Verses.db")))
            {
                using (BinaryWriter bw = new BinaryWriter(new FileStream(tempPath, FileMode.Create)))
                {
                    byte[] buffer = new byte[2048];
                    int len = 0;
                    while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        bw.Write(buffer, 0, len);
                    }
                }
            }

            CreateFavoriteTable(dbPath);
            CreateVerseTable(dbPath);
            CreateLanguageTable(dbPath);

            await UpdateVerses().ConfigureAwait(false);

            await Task.Delay(1000);

            OnSetupFinished(EventArgs.Empty);
        }

        private async Task UpdateVerses()
        {
            var fromStartup = await _verseSetupRepository.Get(new SetUpParameter(), CancellationToken.None).ConfigureAwait(false);
            fromStartup.CanI(async yes =>
            {
                var insert = await _verseRepository.Create(yes, new VerseParameter()).ConfigureAwait(false);
                insert.CanI(yes => {
                    // TODO : do something smart
                }, no => { 
                    // TODO : Add Notification
                });
            }, no =>
            {
                // TODO : Add notification
            });
        }

        private void CreateFavoriteTable(string path)
        {
            try
            {
                using (var con = new SqliteConnection($"DataSource = {path}"))
                {
                    con.Open();
                    var dbName = con.Database;
                    var dbPAth = con.DataSource;

                    var cmd = con.CreateCommand();

                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Favorites ( VerseId INTEGER NOT NULL PRIMARY KEY);";

                    cmd.ExecuteNonQuery();
                }
            } catch (Exception e)
            {

            }
        }

        private void CreateLanguageTable(string path)
        {
            try
            {
                using (var con = new SqliteConnection($"DataSource = {path}"))
                {
                    con.Open();
                    var dbName = con.Database;
                    var dbPAth = con.DataSource;

                    var cmd = con.CreateCommand();

                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Language ( Id INTEGER NOT NULL PRIMARY KEY, Name TEXT, Authors TEXT, LanguageCode TEXT, PictureLink TEXT);";

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {

            }
        }

        private void CreateVerseTable(string path)
        {
            try
            {
                using (var con = new SqliteConnection($"DataSource = {path}"))
                {
                    con.Open();
                    var dbName = con.Database;
                    var dbPAth = con.DataSource;

                    var cmd = con.CreateCommand();

                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Verse ( VerseId INTEGER NOT NULL, LanguageId INTEGER NOT NULL, Content TEXT, PRIMARY KEY (VerseId, LanguageId));";

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}