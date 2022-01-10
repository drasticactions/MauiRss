// <copyright file="FeedListControl.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using MauiRss.ViewModels;

namespace MauiRss.Controls
{
    public partial class FeedListControl : ContentView
    {
        private MauiRssWindowViewModel vm;

        public FeedListControl(MauiRssWindowViewModel vm)
        {
            this.InitializeComponent();
            this.vm = vm;
            this.BindingContext = vm;
        }
    }
}
