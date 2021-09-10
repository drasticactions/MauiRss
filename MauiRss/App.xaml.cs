// <copyright file="App.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using MauiRss.Context;
using MauiRss.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Application = Microsoft.Maui.Controls.Application;

namespace MauiRss
{
    /// <summary>
    /// App.
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider services;
        private readonly INavigationService navigation;
        private readonly IDatabaseContext db;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public App(IServiceProvider services)
        {
            this.services = services;
            this.navigation = services.GetService<INavigationService>();
            this.db = services.GetService<IDatabaseContext>();
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override Window CreateWindow(IActivationState activationState)
        {
            return new Window(new NavigationPage(this.services.GetService<FeedListPage>()));
        }
    }
}
