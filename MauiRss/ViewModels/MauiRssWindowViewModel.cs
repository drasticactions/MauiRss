// <copyright file="MauiRssWindowViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMaui.Tools;
using MauiRss.Models;
using System.Collections.ObjectModel;

namespace MauiRss.ViewModels
{
    public class MauiRssWindowViewModel : MauiRssBaseViewModel
    {
        private WebView? webview;
        private List<FeedListItem> feeds;
        private FeedListItem? selectedFeedListItem;
        private FeedItem selectedFeedItem;

        public MauiRssWindowViewModel(IServiceProvider services)
            : base(services)
        {
            this.Feeds = new List<FeedListItem>();
            this.RefreshFeedList();
            this.UpdateFeedContentCommand = new AsyncCommand<FeedItem>(
              async (item) => { this.SelectedFeedItem = item; await this.RefreshFeedContentAsync(item); },
              null,
              this.Error);
        }

        /// <summary>
        /// Gets or sets the Feed Items.
        /// </summary>
        public ObservableCollection<FeedItem> FeedItems { get; } = new ObservableCollection<FeedItem>();

        /// <summary>
        /// Gets the Update Feed Content Command.
        /// </summary>
        public AsyncCommand<FeedItem> UpdateFeedContentCommand { get; private set; }

        /// <summary>
        /// Gets or sets the feed item.
        /// </summary>
        public FeedItem? SelectedFeedItem
        {
            get => this.selectedFeedItem;
            set => this.SetProperty(ref this.selectedFeedItem, value);
        }

        /// <summary>
        /// Gets or sets the feeds.
        /// </summary>
        public List<FeedListItem> Feeds
        {
            get => this.feeds;
            set => this.SetProperty(ref this.feeds, value);
        }

        public void SetFeedListItem(FeedListItem item)
        {
            this.selectedFeedListItem = item;
            this.FeedItems.Clear();
            var items = this.Database.GetFeedItems(item);
            foreach (var feedItem in items)
            {
                this.FeedItems.Add(feedItem);
            }
        }

        public void SetWebview(WebView webView)
        {
            this.webview = webView;
        }

        private void RefreshFeedList()
        {
            var feeds = this.Database.GetFeedListItems();
            if (this.Feeds != feeds)
            {
                this.Feeds = feeds;
            }
        }

        private async Task RefreshFeedContentAsync(FeedItem item)
        {
            if (item == null)
            {
                return;
            }

            await this.UpdateFeedItem(item);
            this.RenderHtml(item);
        }

        private async Task UpdateFeedItem(FeedItem item)
        {
            SmartReader.Article article = await SmartReader.Reader.ParseArticleAsync(item.Link);
            item.Html = article.Content;
            this.Database.AddOrUpdateFeedItem(item);
        }

        private void RenderHtml(FeedItem item)
        {
            var source = new HtmlWebViewSource();
            source.Html = this.Templates.RenderFeedItem(item);
            if (Application.Current is not null && this.webview is not null)
            {
                Application.Current.Dispatcher.Dispatch(() => this.webview.Source = source);
            }
        }
    }
}
