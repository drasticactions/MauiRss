// <copyright file="FeedListPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using MauiRss.ViewModels;

namespace MauiRss
{
    public partial class FeedListPage : ContentPage
    {
        private MauiRssWindowViewModel vm;

        public FeedListPage(MauiRssWindowViewModel vm)
        {
            this.InitializeComponent();
            this.vm = vm;
            this.BindingContext = vm;
        }
    }
}
