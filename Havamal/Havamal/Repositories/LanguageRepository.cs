using Havamal.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Models.HelperModels;
using Havamal.Parameters;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Havamal.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly DataSettings _dataSettings;

        public LanguageRepository(DataSettings dataSettings)
        {
            _dataSettings = dataSettings;
        }

        public async Task<Computer<IReadOnlyCollection<Language>>> Create(IReadOnlyCollection<Language> data, LanguageParameter param)
        {
            try
            {
                var verses = new List<Language>();

                var path = _dataSettings.DbBasePath;

                if (!File.Exists(path)) throw new Exception("Databasefile not found");

                using var con = new SqliteConnection($"DataSource = {path}");
                con.Open();
                var dbName = con.Database;
                var dbPAth = con.DataSource;

                var cmd = con.CreateCommand();

                var inserts = from ins in data
                                select $"REPLACE INTO Languages (Id, Name, Authors, LanguageCode ) VALUES ({ins.Id}, '{ins.Name.SafeSqLiteString()}', '{ins.Authors.SafeSqLiteString()}', '{ins.LanguageCode.SafeSqLiteString()}'); SELECT Id, Name, Authors, LanguageCode FROM Languages WHERE rowid = last_insert_rowid();";


                foreach (var ins in inserts)
                {
                    cmd.CommandText = ins;

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var verse = new Language(
                            reader.GetInt32(0)
                            , reader.GetString(1)
                            , reader.GetString(3)
                            , reader.GetString(2)
                            );

                        verses.Add(verse);
                    }
                    reader.Close();
                }



                return ComputerExtensions.ComputerSaysYes(DarlingExtensions.Allow((IReadOnlyCollection<Language>)verses));

            }
            catch (Exception e)
            {
                return ComputerExtensions.ComputerSaysNo<IReadOnlyCollection<Language>>(e);
            }
        }

        public async Task<Computer<IReadOnlyCollection<Language>>> Get(LanguageParameter param, CancellationToken cancellationToken = default)
        {
            try
            {
                var langs = new List<Language>();

                var path = _dataSettings.DbBasePath;

                if (!File.Exists(path)) throw new Exception("Databasefile not found");
                
                using var con = new SqliteConnection($"DataSource = {path}");
                con.Open();
                var dbName = con.Database;
                var dbPAth = con.DataSource;

                var cmd = con.CreateCommand();

                var where = $""; // TODO : Add timestamp
                cmd.CommandText = $"SELECT Id, Name, Authors, LanguageCode FROM Languages {where}; ";

                var reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    var verse = new Language(
                        reader.GetInt32(0)
                        , reader.GetString(1)
                        , reader.GetString(3)
                        , reader.GetString(2)
                        );

                    langs.Add(verse);
                }
                

                return ComputerExtensions.ComputerSaysYes(DarlingExtensions.Allow((IReadOnlyCollection<Language>)langs));

            }
            catch (Exception e)
            {
                return ComputerExtensions.ComputerSaysNo<IReadOnlyCollection<Language>>(e);
            }
        }
    }
}
