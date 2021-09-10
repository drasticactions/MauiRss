// <copyright file="FeedContentPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using MauiRss.Models;
using MauiRss.Tools.Utilities;
using MauiRss.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace MauiRss
{
    /// <summary>
    /// Feed Content Page.
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedContentPage : BasePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeedContentPage"/> class.
        /// </summary>
        /// <param name="item">FeedListItem.</param>
        /// <param name="services">IServiceProvider.</param>
        public FeedContentPage(FeedItem item, IServiceProvider services)
            : base(services)
        {
            this.InitializeComponent();
            this.BindingContext = this.ViewModel = services.ResolveWith<FeedContentViewModel>(this.FeedWebView, item);
        }
    }
}
