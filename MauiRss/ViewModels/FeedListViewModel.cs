// <copyright file="FeedListViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiRss.Models;
using MauiRss.Tools;
using MauiRss.Tools.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace MauiRss.ViewModels
{
    /// <summary>
    /// Feed List View Model.
    /// </summary>
    public class FeedListViewModel : BaseViewModel
    {
        private List<FeedListItem> feeds;
        private FeedListItem selectedItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedListViewModel"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public FeedListViewModel(IServiceProvider services)
            : base(services)
        {
            this.Feeds = new List<FeedListItem>();
            this.Title = Translations.Common.AppTitle;
            this.NavigateToFeedPageCommand = new AsyncCommand<FeedListItem>(
               async (item) => await this.NavigateToFeedPageAsync(item),
               null,
               this.Error);
            this.AddNewFeedListItemCommand = new AsyncCommand(
               async () => await this.AddNewFeedListItemAsync(),
               () => true,
               this.Error);
            this.RefreshFeedListCommand = new AsyncCommand(
               async () => this.RefreshFeedList(),
               null,
               this.Error);
        }

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        public List<FeedListItem> Feeds
        {
            get => this.feeds;
            set => this.SetProperty(ref this.feeds, value);
        }

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        public FeedListItem SelectedItem
        {
            get => this.selectedItem;
            set => this.SetProperty(ref this.selectedItem, value);
        }

        /// <summary>
        /// Gets the RefreshFeedListCommand.
        /// </summary>
        public AsyncCommand RefreshFeedListCommand { get; private set; }

        /// <summary>
        /// Gets the Add new Feed List Item Command.
        /// </summary>
        public AsyncCommand AddNewFeedListItemCommand { get; private set; }

        /// <summary>
        /// Gets the NavigateToFeedPageCommand.
        /// </summary>
        public AsyncCommand<FeedListItem> NavigateToFeedPageCommand { get; private set; }

        /// <summary>
        /// Navigates to Add New Feed List Item page.
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

        /// <summary>
        /// Navigates to feed page.
        /// </summary>
        /// <param name="item">FeedListItem.</param>
        /// <returns>Task.</returns>
        public Task NavigateToFeedPageAsync(FeedListItem item)
        {
            if (item != null)
            {
                return this.Navigation.PushPageInMainWindowAsync(this.Services.ResolveWith<FeedPage>(item));
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        /// <inheritdoc/>
        public override async Task LoadAsync()
        {
            await base.LoadAsync();
            this.SelectedItem = null;
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
    }
}
