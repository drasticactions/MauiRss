// <copyright file="BaseViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using MauiRss.Context;
using MauiRss.Models;
using MauiRss.Services;
using MauiRss.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace MauiRss.ViewModels
{
    /// <summary>
    /// Base View Model.
    /// </summary>
    public class BaseViewModel : ExtendedBindableObject
    {
        private bool isBusy;
        private bool isRefreshing;
        private string title;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public BaseViewModel(IServiceProvider services)
        {
            this.Services = services;
            this.Navigation = services.GetService<INavigationService>();
            this.Error = services.GetService<IErrorHandlerService>();
            this.Database = services.GetService<IDatabaseContext>();
            this.Templates = services.GetService<ITemplateService>();
            this.CloseDialogCommand = new AsyncCommand(
               async () => await this.ExecuteCloseDialogCommand(),
               null,
               this.Error);
        }

        /// <summary>
        /// Gets or Sets a value indicating whether the view is busy.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }

            set
            {
                this.isBusy = value;
                this.OnPropertyChanged("IsBusy");
            }
        }

        /// <summary>
        /// Gets or Sets a value indicating whether the view is refreshing.
        /// </summary>
        public bool IsRefreshing
        {
            get
            {
                return this.isRefreshing;
            }

            set
            {
                this.isRefreshing = value;
                this.OnPropertyChanged("IsRefreshing");
            }
        }

        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
                this.OnPropertyChanged("Title");
            }
        }

        /// <summary>
        /// Gets the Close Dialog Command.
        /// </summary>
        public AsyncCommand CloseDialogCommand { get; private set; }

        /// <summary>
        /// Gets the service provider collection.
        /// </summary>
        protected IServiceProvider Services { get; private set; }

        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        protected INavigationService Navigation { get; private set; }

        /// <summary>
        /// Gets the error handler service.
        /// </summary>
        protected IErrorHandlerService Error { get; private set; }

        /// <summary>
        /// Gets the database service.
        /// </summary>
        protected IDatabaseContext Database { get; private set; }

        /// <summary>
        /// Gets the template service.
        /// </summary>
        protected ITemplateService Templates { get; private set; }

        /// <summary>
        /// Load VM Async.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public virtual Task LoadAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Adds New Feed List and returns to previous page.
        /// </summary>
        /// <param name="feedUri">The Feed Uri.</param>
        /// <returns>Task.</returns>
        public async Task<FeedListItem> AddOrUpdateNewFeedListItemAsync(string feedUri)
        {
            try
            {
                var feed = await FeedReader.ReadAsync(feedUri);
                var item = new FeedListItem(feed, feedUri);
                var result = this.Database.AddOrUpdateFeedListItem(item);
                foreach (var feedItem in feed.Items)
                {
                    this.Database.AddOrUpdateFeedItem(new Models.FeedItem(item, feedItem));
                }

                return item;
            }
            catch (Exception ex)
            {
                this.Error.HandleError(ex);
            }

            return null;
        }

        /// <summary>
        /// Sets title for page.
        /// </summary>
        /// <param name="title">The Title.</param>
        public virtual void SetTitle(string title = "")
        {
            this.Title = title;
        }

        /// <summary>
        /// Unload VM Async.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public virtual Task UnloadAsync()
        {
            return Task.CompletedTask;
        }

        private async Task ExecuteCloseDialogCommand()
        {
            await this.Navigation.PopModalPageInMainWindowAsync();
        }
    }
}
