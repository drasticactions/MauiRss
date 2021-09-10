// <copyright file="FeedContentViewModel.cs" company="Drastic Actions">
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
    /// Feed Content View Model.
    /// </summary>
    public class FeedContentViewModel : BaseViewModel
    {
        private WebView webview;
        private FeedItem feedItem;
        private string html;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedContentViewModel"/> class.
        /// </summary>
        /// <param name="webview">WebView.</param>
        /// <param name="item">FeedItem.</param>
        /// <param name="services">IServiceProvider.</param>
        public FeedContentViewModel(WebView webview, FeedItem item, IServiceProvider services)
            : base(services)
        {
            this.webview = webview;
            this.FeedItem = item;
            this.RefreshFeedCommand = new AsyncCommand(
                async () => await this.RefreshFeedAsync(),
                null,
                this.Error);
        }

        /// <summary>
        /// Gets the RefreshFeedCommand.
        /// </summary>
        public AsyncCommand RefreshFeedCommand { get; private set; }

        /// <summary>
        /// Gets or sets the Feed Item.
        /// </summary>
        public FeedItem FeedItem
        {
            get => this.feedItem;
            set => this.SetProperty(ref this.feedItem, value);
        }

        /// <summary>
        /// Gets or sets the Feed Html.
        /// </summary>
        public string Html
        {
            get => this.html;
            set => this.SetProperty(ref this.html, value);
        }

        /// <inheritdoc/>
        public override async Task LoadAsync()
        {
            await base.LoadAsync();
            if (string.IsNullOrEmpty(this.FeedItem.Html))
            {
                await this.UpdateFeedItem();
            }

            if (string.IsNullOrEmpty(this.Html))
            {
                this.RenderHtml();
            }
        }

        private async Task RefreshFeedAsync()
        {
            await this.UpdateFeedItem();
            this.RenderHtml();
            this.Title = this.FeedItem.Title;
            this.IsRefreshing = false;
        }

        private void RenderHtml()
        {
            this.Html = this.Templates.RenderFeedItem(this.feedItem);
            var source = new HtmlWebViewSource();
            source.Html = this.Html;
            Device.BeginInvokeOnMainThread(() => this.webview.Source = source);
        }

        private async Task UpdateFeedItem()
        {
            SmartReader.Article article = await SmartReader.Reader.ParseArticleAsync(this.FeedItem.Link);
            this.FeedItem.Html = article.Content;
            this.Database.AddOrUpdateFeedItem(this.FeedItem);
        }
    }
}
