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
    public class FavoriteRepository : IFavoriteRepository
    {

        private readonly DataSettings _dataSettings;

        public FavoriteRepository(DataSettings dataSettings)
        {
            _dataSettings = dataSettings;
        }
        public async Task<Computer<IReadOnlyCollection<Favorite>>> Create(IReadOnlyCollection<Favorite> data, FavoriteParameter param)
        {
            try
            {
                var favs = new List<Favorite>();

                var path = _dataSettings.DbBasePath;

                if (!File.Exists(path)) throw new Exception("Databasefile not found");

                await Task.Run(() =>
                {
                    using var con = new SqliteConnection($"DataSource = {path}");
                    con.Open();

                    var cmd = con.CreateCommand();

                    foreach(var toAdd in data)
                    {
                        cmd.CommandText = $"INSERT INTO Favorites (VerseId) VALUES ({toAdd.VerseId});";

                        cmd.ExecuteNonQuery();
                        favs.Add(toAdd);
                        // TODO : hent ut siste 
                        //var reader = cmd.ExecuteReader();

                        //while (reader.Read())
                        //{
                        //    var favorite = new Favorite(reader.GetInt32(0));
                        //    favs.Add(favorite);
                        //}
                    }

                });

                return ComputerExtensions.ComputerSaysYes(DarlingExtensions.Allow((IReadOnlyCollection<Favorite>)favs));
            }
            catch (Exception e)
            {
                return ComputerExtensions.ComputerSaysNo<IReadOnlyCollection<Favorite>>(e);
            }
        }

        public async Task<Computer<bool>> Delete(IReadOnlyCollection<Favorite> model, FavoriteParameter param)
        {
            try
            {
                var favs = new List<Favorite>();

                var path = _dataSettings.DbBasePath;

                if (!File.Exists(path)) throw new Exception("Databasefile not found");

                await Task.Run(() =>
                {
                    using var con = new SqliteConnection($"DataSource = {path}");
                    con.Open();

                    var cmd = con.CreateCommand();

                    var idsList = model.Select(x => x.VerseId).Distinct();
                    var ids = string.Join(",", idsList);

                    cmd.CommandText = $"DELETE FROM Favorites WHERE VerseId IN ({ids})";

                    cmd.ExecuteNonQuery();

                });

                return ComputerExtensions.ComputerSaysYes(DarlingExtensions.Allow(true));
            }
            catch (Exception e)
            {
                return ComputerExtensions.ComputerSaysNo<bool>(e);
            }
        }

        public async Task<Computer<IReadOnlyCollection<Favorite>>> Get(FavoriteParameter param, CancellationToken cancellationToken)
        {
            try
            {
                var favs = new List<Favorite>();

                var path = _dataSettings.DbBasePath;

                if (!File.Exists(path)) throw new Exception("Databasefile not found");

                await Task.Run(() =>
                {
                    using var con = new SqliteConnection($"DataSource = {path}");
                    con.Open();

                    var cmd = con.CreateCommand();

                    cmd.CommandText = "SELECT VerseId FROM Favorites;";

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var favorite = new Favorite(reader.GetInt32(0));
                        favs.Add(favorite);
                    }
                });

                return ComputerExtensions.ComputerSaysYes(DarlingExtensions.Allow((IReadOnlyCollection<Favorite>) favs));
            }
            catch (Exception e)
            {
                return ComputerExtensions.ComputerSaysNo<IReadOnlyCollection<Favorite>>(e);
            }
        }
    }
}
