using Havamal.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Models.HelperModels;
using Havamal.Parameters;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Havamal.Repositories
{
    public class VerseSetUpRepository : IVerseSetupRepository
    {
        public async Task<Computer<IReadOnlyCollection<Verse>>> Get(SetUpParameter param, CancellationToken cancellationToken)
        {
            try
            {
                var verses = new List<Verse>();

                string tempName = HavamalPreferences.SetupDbName;
                string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), tempName);

                if (!File.Exists(path)) throw new Exception("Databasefile not found");

                using (var con = new SqliteConnection($"DataSource = {path}"))
                {
                    con.Open();
                    var dbName = con.Database;
                    var dbPAth = con.DataSource;

                    var cmd = con.CreateCommand();

                    var where = $""; // TODO : Add timestamp

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

                return ComputerExtensions.ComputerSaysYes(DarlingExtensions.Allow((IReadOnlyCollection<Verse>)verses));

            }
            catch (Exception e)
            {
                return ComputerExtensions.ComputerSaysNo<IReadOnlyCollection<Verse>>(e);
            }
        }
    }
}
