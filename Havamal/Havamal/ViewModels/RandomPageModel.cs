using Havamal.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havamal.ViewModels
{
    public class RandomPageModel : BasePageModel
    {
        private readonly IVerseRepository _verseRepository;

        private int _verseId;
        private string _content;

        public RandomPageModel(IVerseRepository verseRepository)
        {
            _verseRepository = verseRepository;
            Init();
        }

        private async void Init()
        {
            await GetRandomVerse();
        }

        public int VerseId
        {
            get { return _verseId; }
            set { _verseId = value; OnPropertyChanged(nameof(VerseId)); OnPropertyChanged(nameof(Chapter)); }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; OnPropertyChanged(nameof(Content)); }
        }

        public string Chapter
        {
            get { return _verseId.GetSection().GetSectionString(); }
        }

        public async Task GetRandomVerse()
        {
            try
            {
                var param = new VerseParameter { Language = new List<int> { HavamalPreferences.SelectedLanguage }, OnIds = false };
                var verses = await _verseRepository.Get(param).ConfigureAwait(false);
                verses.CanI(data => {
                    var amount = data.Count;
                    var r = new Random();
                    var random = data.ElementAt(r.Next(0, amount - 1));
                    VerseId = random.VerseId;
                    Content = random.Content;
                }, exception =>
                {
                    // TODO : Alert user
                });
            }catch (Exception e)
            {
                // TODO : Alert user
            }
        }
     }
}
