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
    public class LanguageSetUpRepository : ILanguageSetUpRepository
    {
        public async Task<Computer<IReadOnlyCollection<Language>>> Get(SetUpParameter param, CancellationToken cancellationToken = default)
        {
            try
            {
                var langs = new List<Language>();

                string tempName = HavamalPreferences.SetupDbName;
                string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), tempName);

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
