// <copyright file="FeedContentPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using MauiRss.ViewModels;

namespace MauiRss
{
    public partial class FeedContentPage : ContentPage
    {
        private MauiRssWindowViewModel vm;

        public FeedContentPage(MauiRssWindowViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
            this.BindingContext = vm;
            this.vm.SetWebview(this.FeedWebView);
        }
    }
}
