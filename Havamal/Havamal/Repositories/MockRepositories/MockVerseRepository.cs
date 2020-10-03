using Havamal.Interfaces.BaseInterfaces;
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
    public class MockVerseRepository : IVerseRepository
    {
        public async Task<ResultContainer<IReadOnlyCollection<Verse>>> Get(VerseParameter param, CancellationToken cancellationToken)
        {
            var verses = await Task.Run(() => {
                return new List<Verse>
                {
                    new Verse(1,1, "Augo du bruke \nfyrr inn du gjeng, \ni kot og i kråom, \ni kot og i krokom. \nFor d'er uvist å vita \nkvar uvener sit \nfyre din fot."),
                    new Verse(2,1, "Sæl den som gjev! \nGjest er inn komen, \nkvar finn han sess åt seg? \nBrå vert den \nsom på brandom skal sitja \nog føre ærend fram.")
                };
            });

            return ResultContainer<IReadOnlyCollection<Verse>>.CreateSuccess(DataContainer<IReadOnlyCollection<Verse>>.WithValue(verses.AsReadOnly()));
        }
    }
}
