// <copyright file="DesktopFeedPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiRss.Tools.Utilities;
using MauiRss.ViewModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace MauiRss
{
    /// <summary>
    /// Desktop Feed Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DesktopFeedPage : BasePage
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopFeedPage"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public DesktopFeedPage(IServiceProvider services)
            : base(services)
        {
            this.InitializeComponent();
            this.BindingContext = this.ViewModel = services.ResolveWith<DesktopFeedViewModel>(this.FeedWebView);
        }
    }
}
