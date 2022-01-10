// <copyright file="NewFeedItemEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using MauiRss.Models;

namespace MauiRss.Events
{
    public class NewFeedItemEventArgs : EventArgs
    {
        private readonly FeedListItem feedItem;

        public NewFeedItemEventArgs(FeedListItem item)
        {
            this.feedItem = item;
        }

        public FeedListItem FeedListItem => this.feedItem;
    }
}
