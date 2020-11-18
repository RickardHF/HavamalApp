using Havamal.Interfaces.BaseInterfaces;
using Havamal.Models;
using Havamal.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Havamal.Interfaces.RepositoryInterfaces
{
    public interface ILanguageSetUpRepository : IRead<IReadOnlyCollection<Language>, SetUpParameter>
    {
    }
}
