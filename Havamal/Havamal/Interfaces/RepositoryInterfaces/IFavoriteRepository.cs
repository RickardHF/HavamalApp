using Havamal.Interfaces.BaseInterfaces;
using Havamal.Models;
using Havamal.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Havamal.Interfaces.RepositoryInterfaces
{
    public interface IFavoriteRepository : IRead<IReadOnlyCollection<Favorite>, FavoriteParameter>,
        ICreate<IReadOnlyCollection<Favorite>, IReadOnlyCollection<Favorite>, FavoriteParameter>,
        IDelete<bool, IReadOnlyCollection<Favorite>, FavoriteParameter>
    {
    }
}
