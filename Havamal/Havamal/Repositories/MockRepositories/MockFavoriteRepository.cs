using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Models.HelperModels;
using Havamal.Parameters;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Havamal.Repositories.MockRepositories
{
    public class MockFavoriteRepository : IFavoriteRepository
    {
        private List<Favorite> _favorites;

        public MockFavoriteRepository()
        {
            _favorites = new List<Favorite>
            {
                new Favorite(3)
                , new Favorite(4)
                , new Favorite(7)
            };
        }
        public Task<Computer<IReadOnlyCollection<Favorite>>> Create(IReadOnlyCollection<Favorite> data, FavoriteParameter param)
        {
            var inserted = new List<Favorite>();
            foreach(var d in data)
            {
                if (!_favorites.Contains(d))
                {
                    _favorites.Add(d);
                    inserted.Add(d);
                }
            }

            return Task.Run(() => Computer<IReadOnlyCollection<Favorite>>.ComputerSaysYes((IReadOnlyCollection<Favorite>) inserted));
        }

        public Task<Computer<bool>> Delete(IReadOnlyCollection<Favorite> model, FavoriteParameter param)
        {
            foreach(var delete in model)
            {
                if (_favorites.Contains(delete)) _favorites.Remove(delete);
            }

            return Task.Run(() => Computer<bool>.ComputerSaysYes(true));
        }

        public Task<Computer<IReadOnlyCollection<Favorite>>> Get(FavoriteParameter param, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<Favorite> favs = _favorites;
            return Task.Run(() => Computer<IReadOnlyCollection<Favorite>>.ComputerSaysYes(favs));
        }
    }
}
