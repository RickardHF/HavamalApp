﻿using Android.Database.Sqlite;
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
        public async Task<ResultContainer<IReadOnlyCollection<Verse>>> Get(VerseParameter param, CancellationToken cancellationToken)
        {

            try
            {
                var verses = new List<Verse>();

                var path = _dataSettings.DbBasePath;

                if (!File.Exists(path)) throw new Exception("Databasefile not found");

                using (var con = new SqliteConnection($"DataSource = {path}"))
                {
                    con.Open();
                    var dbName = con.Database;
                    var dbPAth = con.DataSource;
                    
                    var cmd = con.CreateCommand();

                    var where = "";

                    where = $"WHERE LanguageId = {param.Language}";
                    
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
                }

                return ResultContainer<IReadOnlyCollection<Verse>>.CreateSuccess(DataContainer<IReadOnlyCollection<Verse>>.WithValue(verses));

            }
            catch (Exception e)
            {
                return ResultContainer<IReadOnlyCollection<Verse>>.CreateEmpty(e);
            }
        }
    }
}
