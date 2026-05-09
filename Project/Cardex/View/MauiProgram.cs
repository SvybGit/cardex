using Cardex.DAL;
using Cardex.Model.Services;
using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ViewModel;

namespace View;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        // DB context
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "cardex.db");
        builder.Services.AddDbContext<CardexDbContext>(options =>
            options.UseSqlite($"Data Source={databasePath}"));

        // Services
        builder.Services.AddTransient<DeckService>();
        builder.Services.AddTransient<CategoryService>();
        builder.Services.AddTransient<StudySessionService>();

        // ViewModels
        builder.Services.AddTransient<DeckListViewModel>();
        builder.Services.AddTransient<CreateDeckViewModel>();

        // Views
        builder.Services.AddTransient<DeckListPage>();
        builder.Services.AddTransient<CreateDeckPage>();

        var app = builder.Build();
        return app;
    }
}
