// <copyright file="DesktopFeedViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiRss.Models;
using MauiRss.Tools;
using Microsoft.Maui.Controls;

namespace MauiRss.ViewModels
{
    /// <summary>
    /// Desktop Feed View Model.
    /// </summary>
    public class DesktopFeedViewModel : BaseViewModel
    {
        private WebView webview;
        private List<FeedListItem> feeds;
        private List<FeedItem> feedItems;
        private FeedListItem selectedFeedListItem;
        private FeedItem selectedFeedItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopFeedViewModel"/> class.
        /// </summary>
        /// <param name="webview">WebView.</param>
        /// <param name="services">IServiceProvider.</param>
        public DesktopFeedViewModel(WebView webview, IServiceProvider services)
            : base(services)
        {
            this.webview = webview;
            this.Feeds = new List<FeedListItem>();
            this.FeedItems = new List<FeedItem>();
            this.AddNewFeedListItemCommand = new AsyncCommand(
               async () => await this.AddNewFeedListItemAsync(),
               () => true,
               this.Error);
            this.RefreshFeedListCommand = new AsyncCommand(
                async () => this.RefreshFeedList(),
                null,
                this.Error);
            this.UpdateFeedCommand = new AsyncCommand<FeedListItem>(
              async (item) => { this.SelectedFeedListItem = item; await this.RefreshFeedAsync(item); },
              null,
              this.Error);
            this.UpdateFeedContentCommand = new AsyncCommand<FeedItem>(
              async (item) => { this.SelectedFeedItem = item; await this.RefreshFeedContentAsync(item); },
              null,
              this.Error);
        }

        /// <summary>
        /// Gets or sets the feeds.
        /// </summary>
        public List<FeedListItem> Feeds
        {
            get => this.feeds;
            set => this.SetProperty(ref this.feeds, value);
        }

        /// <summary>
        /// Gets or sets the Feed Items.
        /// </summary>
        public List<FeedItem> FeedItems
        {
            get => this.feedItems;
            set => this.SetProperty(ref this.feedItems, value);
        }

        /// <summary>
        /// Gets or sets the feed list item.
        /// </summary>
        public FeedListItem SelectedFeedListItem
        {
            get => this.selectedFeedListItem;
            set => this.SetProperty(ref this.selectedFeedListItem, value);
        }

        /// <summary>
        /// Gets or sets the feed item.
        /// </summary>
        public FeedItem SelectedFeedItem
        {
            get => this.selectedFeedItem;
            set => this.SetProperty(ref this.selectedFeedItem, value);
        }

        /// <summary>
        /// Gets the RefreshFeedListCommand.
        /// </summary>
        public AsyncCommand RefreshFeedListCommand { get; private set; }

        /// <summary>
        /// Gets the RefreshFeedCommand.
        /// </summary>
        public AsyncCommand RefreshFeedCommand { get; private set; }

        /// <summary>
        /// Gets the Add new Feed List Item Command.
        /// </summary>
        public AsyncCommand AddNewFeedListItemCommand { get; private set; }

        /// <summary>
        /// Gets the Update Feed Command.
        /// </summary>
        public AsyncCommand<FeedListItem> UpdateFeedCommand { get; private set; }

        /// <summary>
        /// Gets the Update Feed Content Command.
        /// </summary>
        public AsyncCommand<FeedItem> UpdateFeedContentCommand { get; private set; }

        /// <summary>
        /// Add new feed list item.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task AddNewFeedListItemAsync()
        {
            var feedUri = await this.Navigation.DisplayPromptAsync(Translations.Common.NewFeedListItemTitle, Translations.Common.NewFeedListItemTitle);
            if (feedUri != null)
            {
                await this.AddOrUpdateNewFeedListItemAsync(feedUri);
                this.RefreshFeedList();
            }
        }

        /// <inheritdoc/>
        public override async Task LoadAsync()
        {
            await base.LoadAsync();
            this.SelectedFeedListItem = null;
            this.RefreshFeedList();
        }

        private void RefreshFeedList()
        {
            var feeds = this.Database.GetFeedListItems();
            if (this.Feeds != feeds)
            {
                this.Feeds = feeds;
            }

            this.IsRefreshing = false;
        }

        private async Task RefreshFeedAsync(FeedListItem item)
        {
            item = await this.AddOrUpdateNewFeedListItemAsync(item.Uri.ToString());
            this.FeedItems = this.Database.GetFeedItems(item);
            this.Title = item.Name;
            this.IsRefreshing = false;
        }

        private async Task RefreshFeedContentAsync(FeedItem item)
        {
            await this.UpdateFeedItem(item);
            this.RenderHtml(item);
            this.Title = item.Title;
            this.IsRefreshing = false;
        }

        private void RenderHtml(FeedItem item)
        {
            var source = new HtmlWebViewSource();
            source.Html = this.Templates.RenderFeedItem(item);
            Device.BeginInvokeOnMainThread(() => this.webview.Source = source);
        }

        private async Task UpdateFeedItem(FeedItem item)
        {
            SmartReader.Article article = await SmartReader.Reader.ParseArticleAsync(item.Link);
            item.Html = article.Content;
            this.Database.AddOrUpdateFeedItem(item);
        }
    }
}
