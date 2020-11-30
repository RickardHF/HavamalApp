using Havamal.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Parameters;
using Havamal.Resources.TextResources;
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


namespace Havamal.ViewModels
{
    public class InitPageModel : BasePageModel
    {
        public EventHandler SetUpFinished;

        private readonly IVerseRepository _verseRepository;
        private readonly IVerseSetupRepository _verseSetupRepository;

        private readonly ILanguageRepository _languageRepository;
        private readonly ILanguageSetUpRepository _languageSetUpRepository;

        private readonly DataSettings _dataSettings;

        private string _infoText = "";

        public string InfoText { get { return _infoText; } set
            {
                _infoText = value;
                OnPropertyChanged(nameof(InfoText));
            } }


        public InitPageModel(IVerseRepository verseRepository
            , IVerseSetupRepository verseSetupRepository
            , ILanguageRepository languageRepository
            , ILanguageSetUpRepository languageSetUpRepository
            , DataSettings dataSettings)
        {

            _verseRepository = verseRepository;
            _verseSetupRepository = verseSetupRepository;

            _languageRepository = languageRepository;
            _languageSetUpRepository = languageSetUpRepository;

            _dataSettings = dataSettings;

            OnPropertyChanged(nameof(InfoText));

            SetUp();
        }

        private void SetUp()
        {
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

        public async Task SetUpDb()
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

            var updateVersesTask = UpdateVerses();
            var updateLangsTask = UpdateLanguages();

            Task.WaitAll(updateLangsTask, updateVersesTask);

            InfoText = AppResources.FinishedUpdate;

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
                    InfoText = AppResources.InfoVersesLoaded;
                }, no => {
                    InfoText = AppResources.InfoVersesInsertFail;
                });
            }, no =>
            {
                InfoText = AppResources.InfoLangsLoadedError;
            });
        }

        private async Task UpdateLanguages()
        {
            var fromStartup = await _languageSetUpRepository.Get(new SetUpParameter(), CancellationToken.None).ConfigureAwait(false);
            fromStartup.CanI(async yes =>
            {
                var insert = await _languageRepository.Create(yes, new LanguageParameter()).ConfigureAwait(false);
                insert.CanI(yes => {
                    InfoText = AppResources.InfoLangsLoaded;
                }, no => {
                    InfoText = AppResources.InfoLangsInsertFail;
                });
            }, no =>
            {
                InfoText = AppResources.InfoLangsLoadedError;
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
            }
            catch (Exception e)
            {
                InfoText = AppResources.InfoSetupFavsFailed;
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

                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Languages ( Id INTEGER NOT NULL PRIMARY KEY, Name TEXT, Authors TEXT, LanguageCode TEXT);";

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                InfoText = AppResources.InfoSetupLangsFailed;

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

                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Verses ( VerseId INTEGER NOT NULL, LanguageId INTEGER NOT NULL, Content TEXT, PRIMARY KEY (VerseId, LanguageId));";

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                InfoText = AppResources.InfoSetupVersesFailed;

            }
        }
    }
}
