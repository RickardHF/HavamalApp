using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Models.HelperModels;
using Havamal.Parameters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Havamal.Repositories.MockRepositories
{
    public class MockLanguageRepository : ILanguageRepository
    {
        public Task<Computer<IReadOnlyCollection<Language>>> Get(LanguageParameter param, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<Language> languages = new List<Language>
            {
                new Language(1, "Håvamål", "NO", "Ivar Mortensson-Egnund")
                , new Language(2, "Den Høie's ord", "NO", "G. A. Gjessing")
            };

            return Task.Run(() => ComputerExtensions.ComputerSaysYes(DarlingExtensions.Allow<IReadOnlyCollection<Language>>(languages)));
        }
    }
}
