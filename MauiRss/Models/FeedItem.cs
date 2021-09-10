// <copyright file="FeedItem.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiRss.Models
{
    /// <summary>
    /// Feed Item.
    /// </summary>
    public class FeedItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeedItem"/> class.
        /// </summary>
        public FeedItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedItem"/> class.
        /// </summary>
        /// <param name="feedListItem"><see cref="FeedListItem"/>.</param>
        /// <param name="item"><see cref="CodeHollow.FeedReader.FeedItem"/>.</param>
        public FeedItem(FeedListItem feedListItem, CodeHollow.FeedReader.FeedItem item)
        {
            this.RssId = item.Id;
            this.FeedListItemId = feedListItem.Id;
            this.Title = item.Title;
            this.Link = item.Link;
            this.Description = item.Description;
            this.PublishingDate = item.PublishingDate;
            this.Author = item.Author;
            this.Content = item.Content;
            this.PublishingDateString = item.PublishingDateString;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the id from the rss feed.
        /// </summary>
        public string RssId { get; set; }

        /// <summary>
        /// Gets or sets the feed list item id.
        /// </summary>
        public int FeedListItemId { get; set; }

        /// <summary>
        /// Gets or sets the title of the feed item.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets link (url) to the feed item.
        /// </summary>
        public string Link
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets description of the feed item.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets The publishing date as string.
        /// </summary>
        public string PublishingDateString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets The published date as datetime.
        /// </summary>
        public DateTime? PublishingDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets The author of the feed item.
        /// </summary>
        public string Author
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets The content of the feed item.
        /// </summary>
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets The html of the feed item.
        /// </summary>
        public string Html
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the feed is favorited.
        /// </summary>
        public bool IsFavorite { get; set; }
    }
}
