// <copyright file="FeedContentControl.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using MauiRss.ViewModels;

namespace MauiRss.Controls
{
    public partial class FeedContentControl : ContentView
    {
        private MauiRssWindowViewModel vm;

        public FeedContentControl(MauiRssWindowViewModel vm)
        {
            this.InitializeComponent();
            this.vm = vm;
            this.BindingContext = vm;
            this.vm.SetWebview(this.FeedWebView);
        }
    }
}
