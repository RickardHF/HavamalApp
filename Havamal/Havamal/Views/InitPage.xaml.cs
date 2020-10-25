using Microsoft.Data.Sqlite;
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

            string tempName = "VersesTemp.db";
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

            await Task.Delay(1000);

            OnSetupFinished(EventArgs.Empty);
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