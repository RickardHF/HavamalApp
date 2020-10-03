using Havamal.Interfaces.BaseInterfaces;
using Havamal.Models;
using Havamal.Models.HelperModels;
using Havamal.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Havamal.Interfaces.RepositoryInterfaces
{
    public interface IVerseRepository : IRead<IReadOnlyCollection<Verse>, VerseParameter>
    {
    }
}
