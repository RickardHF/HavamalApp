using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Models.HelperModels;
using Havamal.Parameters;
using Havamal.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.VerseReposTests
{
    [TestClass]
    public class VerseReposTests
    {
        private IVerseRepository InitRepos()
        {
            var dataSettings = new DataSettings
            {
                DbBasePath = Environment.CurrentDirectory
            };
            var provider = new ServiceCollection()
                .AddSingleton<IVerseRepository, VerseRepository>()
                .AddSingleton(dataSettings)
                .BuildServiceProvider();

            return provider.GetService<IVerseRepository>();
        }

        [TestMethod]
        public async Task TestGetData()
        {
            var repos = InitRepos();

            var result = await repos.Get(new VerseParameter { Language = 1}, CancellationToken.None).ConfigureAwait(false);

            Assert.IsTrue(result.Answer == Answer.Yes, "Fail status");
        }

        [TestMethod]
        public async Task TestGetDataCorrectLanguage()
        {
            var repos = InitRepos();

            var result = await repos.Get(new VerseParameter { Language = 1 }, CancellationToken.None).ConfigureAwait(false);

            Assert.IsTrue(result.Answer == Answer.Yes, "Fail status");

            result.Happy.MayI(some =>
            {
                var count = some.Count(x => x.LanguageId != 1);
                Assert.AreEqual(0, count, "Not valid data given");
            }, () => { Assert.Fail("No data returned"); });
        }
    }
}
