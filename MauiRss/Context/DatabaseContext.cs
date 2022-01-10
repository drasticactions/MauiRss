// <copyright file="DatabaseContext.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using LiteDB;
using MauiRss.Models;

namespace MauiRss.Context
{
    /// <summary>
    /// Database Context.
    /// </summary>
    public class DatabaseContext : IDatabaseContext
    {
        private const string FeedsCollection = "Feeds";
        private const string FeedItemsCollection = "FeedItems";

        private const string DatabaseName = "database.db";

        private LiteDatabase db;
        private string databasePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseContext"/> class.
        /// </summary>
        public DatabaseContext()
        {
            this.OnConfiguring();
        }

        /// <summary>
        /// Gets the Feed List Items.
        /// </summary>
        public ILiteCollection<FeedListItem> FeedListItems => this.db.GetCollection<FeedListItem>(FeedsCollection);

        /// <summary>
        /// Gets the Feed Items.
        /// </summary>
        public ILiteCollection<FeedItem> FeedItems => this.db.GetCollection<FeedItem>(FeedItemsCollection);

        /// <inheritdoc/>
        public bool DoesFeedListItemExist(FeedListItem item) => this.FeedListItems.FindOne(n => n.Uri == item.Uri) is not null;


        /// <inheritdoc/>
        public bool AddOrUpdateFeedItem(FeedItem item)
        {
            var existingItem = this.FeedItems.FindOne(n => n.RssId == item.RssId);
            if (existingItem != null)
            {
                item.Id = existingItem.Id;
            }

            return this.FeedItems.Upsert(item);
        }

        /// <inheritdoc/>
        public bool AddOrUpdateFeedListItem(FeedListItem item)
        {
            var existingItem = this.FeedListItems.FindOne(n => n.Uri == item.Uri);
            if (existingItem != null)
            {
                item.Id = existingItem.Id;
            }

            return this.FeedListItems.Upsert(item);
        }

        /// <inheritdoc/>
        public List<FeedListItem> GetFeedListItems()
        {
            return this.FeedListItems.FindAll().ToList();
        }

        /// <inheritdoc/>
        public FeedListItem? GetFeedListItem(Uri uri) => this.FeedListItems.FindOne(n => n.Uri == uri);

        /// <inheritdoc/>
        public List<FeedItem> GetFeedItems(FeedListItem item)
        {
            return this.FeedItems.Find(n => n.FeedListItemId == item.Id).ToList();
        }

        /// <inheritdoc/>
        public bool RemoveFeedItem(FeedItem item)
        {
            return this.FeedListItems.Delete(item.Id);
        }

        /// <inheritdoc/>
        public bool RemoveFeedListItem(FeedListItem item)
        {
            return this.FeedListItems.Delete(item.Id);
        }

        private void OnConfiguring(string databasePath = "")
        {
            if (string.IsNullOrEmpty(databasePath))
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.macOS:
                    case Device.iOS:
                        this.databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", DatabaseName);
                        break;
                    case Device.Android:
                        this.databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseName);
                        break;
                    case Device.UWP:
                    case Device.WPF:
                        this.databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DatabaseName);
                        break;
                    default:
                        throw new NotImplementedException("Platform not supported");
                }
            }
            else
            {
                this.databasePath = Path.Combine(this.databasePath, DatabaseName);
            }

            this.db = new LiteDatabase(this.databasePath);
        }
    }
}
