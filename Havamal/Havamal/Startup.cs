using Havamal.Interfaces.RepositoryInterfaces;
using Havamal.Models;
using Havamal.Repositories;
using Havamal.Repositories.MockRepositories;
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

        public static void Init()
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
                            ConfigureServices(c, x);
                        })
                        .Build();

            ServiceProvider = host.Services;
        }

        public static App InitApplication()
        {
            Init();

            return ServiceProvider.GetService<App>();
        }

        static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            //var stream = Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(x => x.Contains("Havamal.HavamalVerses.db"));

            string dbName = "HavamalVerses.db";
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
            // Check if your DB has already been extracted.
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }
            using (BinaryReader br = new BinaryReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Verses.db")))
            {
                using (BinaryWriter bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                {
                    byte[] buffer = new byte[2048];
                    int len = 0;
                    while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        bw.Write(buffer, 0, len);
                    }
                }
            }
            
            var dataSettings = new DataSettings
            {

                DbBasePath = dbPath
            };
            services
                .AddSingleton(dataSettings)
                .ConfigureDependencies()
                .AddTransient<VersePage>()
                .AddTransient<AppShell>()
                .AddTransient<SettingsPageModel>()
                .AddTransient<SettingsPage>()
                .AddTransient<VersePageModel>()
                .AddTransient<FavoritesPageModel>()
                .AddTransient<SearchPageModel>()
                .AddTransient<ComparePageModel>()
                .AddTransient<App>()
                ;
        }
    }

    public static class IoCConfig
    {
        public static IServiceCollection ConfigureDependencies(this IServiceCollection services)
        {
            var userSettings = new UserSettings();
            userSettings.SelectedLanguage = Preferences.Get("SelectedLanguage", 1);
            userSettings.CurrentVerse = Preferences.Get("CurrentVerse", 1);

            services
                .AddSingleton<IVerseRepository, VerseRepository>()
                .AddSingleton<ILanguageRepository, MockLanguageRepository>()
                .AddSingleton<IFavoriteRepository, MockFavoriteRepository>()
                .AddSingleton(userSettings);
                ;
            return services;
        }
    }
}
