// <copyright file="FeedListPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using MauiRss.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace MauiRss
{
    /// <summary>
    /// Feed List Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedListPage : BasePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeedListPage"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public FeedListPage(IServiceProvider services)
            : base(services)
        {
            this.InitializeComponent();
            this.BindingContext = this.ViewModel = services.GetService<FeedListViewModel>();
        }
    }
}
