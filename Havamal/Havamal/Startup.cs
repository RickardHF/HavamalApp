using Havamal.Helpers;
using Havamal.Interfaces.Helpers;
using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Repositories;
using Havamal.Repositories.MockRepositories;
using Havamal.Resources.Themes;
using Havamal.ViewModels;
using Havamal.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Xamarin.Essentials;

namespace Havamal
{
    public static class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void Init(IThemeChanger themeChanger)
        {
            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("Havamal.applicationsettings.json");

            var host = new HostBuilder()
                        .ConfigureHostConfiguration(c =>
                        {
                            c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                            c.AddJsonStream(stream);
                        }).ConfigureServices((c, x) =>
                        {
                            ConfigureServices(c, x, themeChanger);
                        })
                        .Build();

            ServiceProvider = host.Services;
        }

        public static App InitApplication(IThemeChanger themeChanger)
        {
            Init(themeChanger);
            
            return ServiceProvider.GetService<App>();
        }

        static void ConfigureServices(HostBuilderContext context, IServiceCollection services, IThemeChanger themeChanger)
        {
            //var stream = Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(x => x.Contains("Havamal.HavamalVerses.db"));

            string dbName = "HavamalVerses.db";
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
            
            var dataSettings = new DataSettings
            {

                DbBasePath = dbPath
            };
            themeChanger.ChangeTheme((HavamalTheme)HavamalPreferences.Theme);
            services.AddSingleton<IThemeChanger>(themeChanger);

            services
                .AddTransient<StanzaCarouselPage>()
                .AddTransient<StanzaListPage>()
                .AddTransient<StanzaPage>()
                .AddTransient<RandomPage>()
                .AddTransient<SearchPage>()
                .AddTransient<CompareCarouselPage>()
                .AddTransient<FavoritesPage>()
                .AddTransient<SettingsPage>()
                ;

            services
                .AddSingleton(dataSettings)
                .ConfigureDependencies()
                .AddTransient<VersePage>()
                .AddTransient<SettingsPage>()
                .AddTransient<SettingsPageModel>()
                .AddTransient<VersePageModel>()
                .AddTransient<FavoritesPageModel>()
                .AddTransient<SearchPageModel>()
                .AddTransient<ComparePageModel>()
                .AddTransient<StanzaCarouselPageModel>()
                .AddTransient<RandomPageModel>()
                .AddTransient<CompareCarouselPageModel>()
                .AddTransient<InitPageModel>()
                .AddTransient<App>()
                ;
        }
    }

    public static class IoCConfig
    {
        public static IServiceCollection ConfigureDependencies(this IServiceCollection services)
        {

            services
                .AddSingleton<IVerseRepository, VerseRepository>()
                .AddSingleton<ILanguageRepository, LanguageRepository>()
                .AddSingleton<IFavoriteRepository, FavoriteRepository>()
                .AddSingleton<IVerseSetupRepository, VerseSetUpRepository>()
                .AddSingleton<ILanguageSetUpRepository, LanguageSetUpRepository>()
                ;
            return services;
        }
    }
}
