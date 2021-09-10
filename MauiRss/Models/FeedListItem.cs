﻿// <copyright file="FeedListItem.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeHollow.FeedReader;

namespace MauiRss.Models
{
    /// <summary>
    /// Feed List Item.
    /// </summary>
    public class FeedListItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeedListItem"/> class.
        /// </summary>
        public FeedListItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedListItem"/> class.
        /// </summary>
        /// <param name="feed"><see cref="Feed"/>.</param>
        /// <param name="feedUri">Original Feed Uri.</param>
        public FeedListItem(Feed feed, string feedUri)
        {
            this.Name = feed.Title;
            this.Uri = new Uri(feedUri);
            this.Link = feed.Link;
            this.ImageUri = string.IsNullOrEmpty(feed.ImageUrl) ? null : new Uri(feed.ImageUrl);
            this.Description = feed.Description;
            this.Language = feed.Language;
            this.LastUpdatedDate = feed.LastUpdatedDate;
            this.LastUpdatedDateString = feed.LastUpdatedDateString;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the feed name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the feed description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the feed Language.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the last updated date.
        /// </summary>
        public DateTime? LastUpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the last updated date string.
        /// </summary>
        public string LastUpdatedDateString { get; set; }

        /// <summary>
        /// Gets or sets the image uri.
        /// </summary>
        public Uri ImageUri { get; set; }

        /// <summary>
        /// Gets or sets the Feed Uri.
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets the Feed Link.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the feed is favorited.
        /// </summary>
        public bool IsFavorite { get; set; }
    }
}
