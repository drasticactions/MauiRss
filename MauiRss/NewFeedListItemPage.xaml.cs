// <copyright file="NewFeedListItemPage.xaml.cs" company="Drastic Actions">
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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewFeedListItemPage : BasePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewFeedListItemPage"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public NewFeedListItemPage(IServiceProvider services)
            : base(services)
        {
            this.InitializeComponent();
            this.BindingContext = this.ViewModel = services.GetService<NewFeedItemViewModel>();
        }
    }
}
