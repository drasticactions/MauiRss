// <copyright file="MauiRssBaseViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using AngleSharp.Html.Parser;
using CodeHollow.FeedReader;
using DrasticMaui.ViewModels;
using MauiRss.Context;
using MauiRss.Events;
using MauiRss.Models;
using MauiRss.Services;
using System.Reflection;

namespace MauiRss.ViewModels
{
    public class MauiRssBaseViewModel : BaseViewModel
    {
        private HttpClient client;
        private HtmlParser parser;

        public MauiRssBaseViewModel(IServiceProvider services, Page? originalPage = null)
            : base(services, originalPage)
        {
            this.Database = services.GetService<IDatabaseContext>();
            this.Templates = services.GetService<ITemplateService>();
            this.client = new HttpClient();
            this.parser = new HtmlParser();
        }

        public event EventHandler<NewFeedItemEventArgs>? OnNewFeedItemAdded;

        /// <summary>
        /// Gets the database service.
        /// </summary>
        protected IDatabaseContext Database { get; private set; }

        /// <summary>
        /// Gets the template service.
        /// </summary>
        protected ITemplateService Templates { get; private set; }

        /// <summary>
        /// Adds New Feed List and returns to previous page.
        /// </summary>
        /// <param name="feedUri">The Feed Uri.</param>
        /// <returns>Task.</returns>
        public async Task<FeedListItem?> AddOrUpdateNewFeedListItemAsync(string feedUri)
        {
            try
            {
                bool isNewItem = false;
                var feed = await FeedReader.ReadAsync(feedUri);
                var item = this.Database.GetFeedListItem(new Uri(feedUri));
                if (item is null)
                {
                    item = new FeedListItem(feed, feedUri);
                    isNewItem = true;
                }

                if (item.ImageCache is null && item.ImageUri is not null)
                {
                    item.ImageCache = await this.client.GetByteArrayAsync(item.ImageUri);
                }
                else if (item.ImageCache is null)
                {
                    item.ImageCache = GetPlaceholderIcon();
                }

                var result = this.Database.AddOrUpdateFeedListItem(item);

                foreach (var feedItem in feed.Items)
                {
                    using var document = await this.parser.ParseDocumentAsync(feedItem.Content);
                    var image = document.QuerySelector("img");
                    var imageUrl = string.Empty;
                    if (image is not null)
                    {
                        imageUrl = image.GetAttribute("src");
                    }

                    this.Database.AddOrUpdateFeedItem(new Models.FeedItem(item, feedItem, imageUrl));
                }

                if (isNewItem)
                {
                    this.OnNewFeedItemAdded?.Invoke(this, new NewFeedItemEventArgs(item));
                }

                return item;
            }
            catch (Exception ex)
            {
                this.Error.HandleError(ex);
            }

            return null;
        }

        private static byte[] GetPlaceholderIcon()
        {
            var resource = GetResourceFileContent("Icon.favicon.ico");
            if (resource is null)
            {
                throw new Exception("Failed to get placeholder icon.");
            }

            MemoryStream ms = new MemoryStream();
            resource.CopyTo(ms);
            return ms.ToArray();
        }

        /// <summary>
        /// Get Resource File Content via FileName.
        /// </summary>
        /// <param name="fileName">Filename.</param>
        /// <returns>Stream.</returns>
        private static Stream? GetResourceFileContent(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MauiRSS." + fileName;
            if (assembly is null)
            {
                return null;
            }

            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}
