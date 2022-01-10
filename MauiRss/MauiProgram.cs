// <copyright file="MauiProgram.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMaui.Services;
using MauiRss.Context;
using MauiRss.Services;
using MauiRss.ViewModels;

namespace MauiRss;

/// <summary>
/// Maui Program.
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Create Default Maui App.
    /// </summary>
    /// <returns><see cref="MauiApp"/>.</returns>
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
        builder.Services.AddSingleton<ITemplateService, TemplateService>();
        builder.Services.AddTransient<MauiRssWindowViewModel>();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("FontAwesome6Brands-Regular-400.otf", "FontAwesomeBrands");
                fonts.AddFont("FontAwesome6Free-Regular-400.otf", "FontAwesomeRegular");
                fonts.AddFont("FontAwesome6Free-Solid-900.otf", "FontAwesomeSolid");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        return builder.Build();
    }
}
