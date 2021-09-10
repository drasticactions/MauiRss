// <copyright file="MauiProgram.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using MauiRss.Context;
using MauiRss.Services;
using MauiRss.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Hosting;

namespace MauiRss
{
    /// <summary>
    /// Maui Program.
    /// </summary>
    public static class MauiProgram
    {
        /// <summary>
        /// Create Maui App.
        /// </summary>
        /// <returns><see cref="MauiApp"/>.</returns>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
            builder.Services.AddSingleton<ITemplateService, TemplateService>();
            builder.Services.AddTransient<FeedListViewModel>();
            builder.Services.AddTransient<FeedViewModel>();
            builder.Services.AddTransient<FeedContentViewModel>();
            builder.Services.AddTransient<NewFeedItemViewModel>();
            builder.Services.AddTransient<FeedListPage>();
            builder.Services.AddTransient<FeedPage>();
            builder.Services.AddTransient<FeedContentPage>();
            builder.Services.AddTransient<NewFeedListItemPage>();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            return builder.Build();
        }
    }
}