using Android.Database.Sqlite;
using Havamal.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Models.HelperModels;
using Havamal.Parameters;
using Javax.Xml.Xpath;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Essentials;

namespace Havamal.Repositories
{
    public class VerseRepository : IVerseRepository
    {
        private readonly DataSettings _dataSettings;

        public VerseRepository(DataSettings dataSettings)
        {
            _dataSettings = dataSettings;
        }

        public async Task<Computer<IReadOnlyCollection<Verse>>> Create(IReadOnlyCollection<Verse> data, VerseParameter param)
        {
            try
            {
                var verses = new List<Verse>();

                var path = _dataSettings.DbBasePath;

                if (!File.Exists(path)) throw new Exception("Databasefile not found");

                using var con = new SqliteConnection($"DataSource = {path}");
                con.Open();
                var dbName = con.Database;
                var dbPAth = con.DataSource;

                var cmd = con.CreateCommand();

                var inserts = from ins in data
                                select $"REPLACE INTO Verses (VerseId, LanguageId, Content) VALUES ({ins.VerseId}, {ins.LanguageId}, '{ins.Content.SafeSqLiteString()}'); SELECT VerseId, LanguageId, Content FROM Verses WHERE rowid = last_insert_rowid();";


                foreach(var ins in inserts)
                {
                    cmd.CommandText = ins;

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var verse = new Verse(
                            reader.GetInt32(0)
                            , reader.GetInt32(1)
                            , reader.GetString(2));

                        verses.Add(verse);
                    }

                    reader.Close();
                }

                

                return ComputerExtensions.ComputerSaysYes(DarlingExtensions.Allow((IReadOnlyCollection<Verse>)verses));

            }
            catch (Exception e)
            {
                return ComputerExtensions.ComputerSaysNo<IReadOnlyCollection<Verse>>(e);
            }
        }

        public async Task<Computer<IReadOnlyCollection<Verse>>> Get(VerseParameter param, CancellationToken cancellationToken)
        {

            try
            {
                var verses = new List<Verse>();

                var path = _dataSettings.DbBasePath;

                if (!File.Exists(path)) throw new Exception("Databasefile not found");

                using var con = new SqliteConnection($"DataSource = {path}");
                con.Open();
                var dbName = con.Database;
                var dbPAth = con.DataSource;
                    
                var cmd = con.CreateCommand();

                var where = "";

                where = $"WHERE LanguageId IN ({string.Join(",", param.Language)})";
                    
                if (param.OnIds)
                {
                    var ids = string.Join(",", param.Ids);
                    where += $" AND VerseId IN ({ids})";
                }
                    
                cmd.CommandText = $"SELECT VerseId, LanguageId, Content FROM Verses {where}; ";
                    
                var reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    var verse = new Verse(
                        reader.GetInt32(0)
                        , reader.GetInt32(1)
                        , reader.GetString(2));

                    verses.Add(verse);
                }
                

                return ComputerExtensions.ComputerSaysYes(DarlingExtensions.Allow((IReadOnlyCollection<Verse>) verses));

            }
            catch (Exception e)
            {
                return ComputerExtensions.ComputerSaysNo<IReadOnlyCollection<Verse>>(e);
            }
        }
    }
}
