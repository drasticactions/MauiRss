// <copyright file="NewFeedItemViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CodeHollow.FeedReader;
using MauiRss.Models;
using MauiRss.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace MauiRss.ViewModels
{
    /// <summary>
    /// New Feed Item View Model.
    /// </summary>
    public class NewFeedItemViewModel : BaseViewModel
    {
        private string feedUri;
        private string feedTitle;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewFeedItemViewModel"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public NewFeedItemViewModel(IServiceProvider services)
            : base(services)
        {
            this.Title = Translations.Common.NewFeedListItemTitle;
            this.AddNewFeedListItemAsyncCommand = new AsyncCommand(
               async () => await this.NewFeedListItemAsync(this.FeedUri),
               () => true,
               this.Error);
            this.CheckNewFeedListItemAsyncCommand = new AsyncCommand(
               async () => await this.CheckNewFeedListItemAsync(),
               () => true,
               this.Error);
            this.FeedUri = "https://arstechnica.com/feed/";
        }

        /// <summary>
        /// Gets or sets the Feed Uri.
        /// </summary>
        public string FeedUri
        {
            get => this.feedUri;
            set => this.SetProperty(ref this.feedUri, value);
        }

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        public string FeedTitle
        {
            get => this.feedTitle;
            set => this.SetProperty(ref this.feedTitle, value);
        }

        /// <summary>
        /// Gets the AddNewFeedListItemAsyncCommand.
        /// </summary>
        public AsyncCommand AddNewFeedListItemAsyncCommand { get; private set; }

        /// <summary>
        /// Gets the CheckNewFeedListItemAsyncCommand.
        /// </summary>
        public AsyncCommand CheckNewFeedListItemAsyncCommand { get; private set; }

        /// <summary>
        /// Check inputted feed item is valid.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task CheckNewFeedListItemAsync()
        {
            try
            {
                var feed = await FeedReader.ReadAsync(this.FeedUri);
            }
            catch (Exception ex)
            {
                this.Error.HandleError(ex);
            }
        }

        /// <summary>
        /// Adds New Feed List and returns to previous page.
        /// </summary>
        /// <param name="feedUri">The Feed Uri.</param>
        /// <returns>Task.</returns>
        public async Task NewFeedListItemAsync(string feedUri)
        {
            await this.AddOrUpdateNewFeedListItemAsync(feedUri);
            await this.Navigation.GoBackPageInMainWindowAsync();
        }
    }
}
