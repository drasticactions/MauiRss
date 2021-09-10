using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiRss.Models;
using MauiRss.Tools;
using MauiRss.Tools.Utilities;

namespace MauiRss.ViewModels
{
    /// <summary>
    /// Feed View Model.
    /// </summary>
    public class FeedViewModel : BaseViewModel
    {
        private List<FeedItem> feedItems;
        private FeedListItem feedListItem;
        private FeedItem selectedItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedViewModel"/> class.
        /// </summary>
        /// <param name="item">FeedListItem.</param>
        /// <param name="services">IServiceProvider.</param>
        public FeedViewModel(FeedListItem item, IServiceProvider services)
            : base(services)
        {
            this.feedListItem = item;
            this.FeedItems = new List<FeedItem>();
            this.NavigateToFeedContentPageCommand = new AsyncCommand<FeedItem>(
                async (item) => await this.NavigateToFeedContentPageAsync(item),
                null,
                this.Error);
            this.RefreshFeedCommand = new AsyncCommand(
                async () => await this.RefreshFeedAsync(),
                null,
                this.Error);
        }

        /// <summary>
        /// Gets the NavigateToFeedContentPageCommand.
        /// </summary>
        public AsyncCommand<FeedItem> NavigateToFeedContentPageCommand { get; private set; }

        /// <summary>
        /// Gets the RefreshFeedCommand.
        /// </summary>
        public AsyncCommand RefreshFeedCommand { get; private set; }

        /// <summary>
        /// Gets or sets the Feed Items.
        /// </summary>
        public List<FeedItem> FeedItems
        {
            get => this.feedItems;
            set => this.SetProperty(ref this.feedItems, value);
        }

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        public FeedItem SelectedItem
        {
            get => this.selectedItem;
            set => this.SetProperty(ref this.selectedItem, value);
        }

        /// <inheritdoc/>
        public override async Task LoadAsync()
        {
            await base.LoadAsync();
            this.Title = this.feedListItem.Name;
            this.FeedItems = this.Database.GetFeedItems(this.feedListItem);
        }

        /// <summary>
        /// Navigates to feed content page.
        /// </summary>
        /// <param name="item">FeedItem.</param>
        /// <returns>Task.</returns>
        public Task NavigateToFeedContentPageAsync(FeedItem item)
        {
            if (item != null)
            {
                return this.Navigation.PushPageInMainWindowAsync(this.Services.ResolveWith<FeedContentPage>(item));
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        private async Task RefreshFeedAsync()
        {
            this.feedListItem = await this.AddOrUpdateNewFeedListItemAsync(this.feedListItem.Uri.ToString());
            this.FeedItems = this.Database.GetFeedItems(this.feedListItem);
            this.Title = this.feedListItem.Name;
            this.IsRefreshing = false;
        }
    }
}