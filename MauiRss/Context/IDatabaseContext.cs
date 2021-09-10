// <copyright file="IDatabaseContext.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiRss.Models;

namespace MauiRss.Context
{
    /// <summary>
    /// Masto Context.
    /// </summary>
    public interface IDatabaseContext
    {
        /// <summary>
        /// Gets the list of Feed List Items.
        /// </summary>
        /// <returns>List of FeedListItems.</returns>
        public List<FeedListItem> GetFeedListItems();

        /// <summary>
        /// Adds or updates a new feed list item to the database.
        /// </summary>
        /// <param name="item">Feed list item to be added.</param>
        /// <returns>Boolean indicating whether the feed item was added.</returns>
        public bool AddOrUpdateFeedListItem(FeedListItem item);

        /// <summary>
        /// Remove a feed list item from the database.
        /// </summary>
        /// <param name="item">Feed list Item to be removed.</param>
        /// <returns>Boolean indicating whether the feed item was removed.</returns>
        public bool RemoveFeedListItem(FeedListItem item);

        /// <summary>
        /// Adds or updates a new feed item to the database.
        /// </summary>
        /// <param name="item">Feeditem to be added.</param>
        /// <returns>Boolean indicating whether the feed item was added.</returns>
        public bool AddOrUpdateFeedItem(FeedItem item);

        /// <summary>
        /// Remove a feed item from the database.
        /// </summary>
        /// <param name="item">Feed Item to be removed.</param>
        /// <returns>Boolean indicating whether the feed item was removed.</returns>
        public bool RemoveFeedItem(FeedItem item);

        /// <summary>
        /// Gets list of feed items for a given list.
        /// </summary>
        /// <param name="item">FeedListItem.</param>
        /// <returns>List of Feed Items.</returns>
        public List<FeedItem> GetFeedItems(FeedListItem item);
    }
}