// <copyright file="MauiRssWindow.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

#if IOS || __MACCATALYST__
using Foundation;
using MauiRss.Models;
using MauiRss.ViewModels;
using Microsoft.Maui.Platform;
using UIKit;

namespace MauiRss
{
    /// <summary>
    /// Maui Rss iOS Window.
    /// </summary>
    public class MauiRssWindow : DrasticMaui.DrasticMauiWindow
    {
        private bool isInitialized;
        private MauiRssWindowViewModel vm;
        private SidebarViewController sidebar;
        private UISplitViewController splitView;
        private Controls.FeedContentControl feedContentControl;
        private Controls.FeedListControl feedListControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="MauiRssWindow"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public MauiRssWindow(IServiceProvider services)
            : base(services)
        {
            this.Page = new MainPage();
            this.vm = new MauiRssWindowViewModel(services);
            this.sidebar = new SidebarViewController(this, this.vm);
            this.splitView = new UISplitViewController();
            this.splitView.PrimaryBackgroundStyle = UISplitViewControllerBackgroundStyle.Sidebar;
            this.feedContentControl = new Controls.FeedContentControl(this.vm);
            this.feedListControl = new Controls.FeedListControl(this.vm);
        }

        /// <inheritdoc/>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            if (!this.isInitialized)
            {
                this.SetupNavigationView();
            }
        }

        private void SetupNavigationView()
        {
            if (this.Page == null)
            {
                return;
            }

            var window = this.Handler?.NativeView as UIWindow;
            if (window is null)
            {
                return;
            }

            var context = this.Handler?.MauiContext;
            if (context is null)
            {
                return;
            }

            var feedListUIControl = this.feedListControl.ToUIViewController(context);
            var feedContextUIControl = this.feedContentControl.ToUIViewController(context);

            this.splitView.ViewControllers = new UIViewController[] { new UINavigationController(this.sidebar), feedListUIControl };
            window.RootViewController = this.splitView;
        }
    }

    public class NavigationSidebarItem : NSObject
    {
        public NavigationSidebarItem(FeedListItem item)
        {
            this.FeedListItem = item;
            if (item.ImageCache is not null)
            {
                var imageData = Foundation.NSData.FromArray(item.ImageCache);
                if (imageData is not null)
                {
                    this.Image = UIKit.UIImage.LoadFromData(imageData);
                }
            }
        }

        public FeedListItem FeedListItem { get; }

        /// <summary>
        /// Gets the image.
        /// </summary>
        public UIKit.UIImage? Image { get; private set; }
    }

    public class SidebarViewController : UIViewController, IUICollectionViewDelegate
    {
        private UICollectionView? collectionView;
        private UICollectionViewDiffableDataSource<NSString, NavigationSidebarItem>? dataSource;
        private MauiRssWindowViewModel vm;
        private MauiRssWindow window;

        public SidebarViewController(MauiRssWindow window, MauiRssWindowViewModel vm)
        {
            this.window = window;
            this.vm = vm;
            this.vm.OnNewFeedItemAdded += Vm_OnNewFeedItemAdded;
        }

        /// <inheritdoc/>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.NavigationItem.Title = Translations.Common.AppTitle;
            this.NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Always;

            var addButton = new UIBarButtonItem(UIImage.GetSystemImage("gear"), UIBarButtonItemStyle.Plain, this.OpenAddFeed);
            addButton.AccessibilityLabel = Translations.Common.AddNewFeedListItemButton;
            this.NavigationItem.RightBarButtonItem = addButton;

            if (this.View is null)
            {
                throw new NullReferenceException(nameof(this.View));
            }

            this.collectionView = new UICollectionView(this.View.Bounds, this.CreateLayout());
            this.collectionView.Delegate = this;
            this.View.AddSubview(this.collectionView);

            // Anchor collectionView
            this.collectionView.TranslatesAutoresizingMaskIntoConstraints = false;

            var constraints = new List<NSLayoutConstraint>();
            constraints.Add(this.collectionView.BottomAnchor.ConstraintEqualTo(this.View.BottomAnchor));
            constraints.Add(this.collectionView.LeftAnchor.ConstraintEqualTo(this.View.LeftAnchor));
            constraints.Add(this.collectionView.RightAnchor.ConstraintEqualTo(this.View.RightAnchor));
            constraints.Add(this.collectionView.HeightAnchor.ConstraintEqualTo(this.View.HeightAnchor));

            NSLayoutConstraint.ActivateConstraints(constraints.ToArray());

            this.ConfigureDataSource();

            this.SetupNavigationItems(this.GetNavigationSnapshot(this.vm.Feeds));
        }

        /// <summary>
        /// Item Selected.
        /// </summary>
        /// <param name="collectionView">Collection View.</param>
        /// <param name="indexPath">Index Path.</param>
        [Export("collectionView:didSelectItemAtIndexPath:")]
        protected void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var sidebarItem = this.dataSource?.GetItemIdentifier(indexPath);
            if (sidebarItem is not null)
            {
                this.vm.SetFeedListItem(sidebarItem.FeedListItem);
            }
        }

        private void SetupNavigationItems(NSDiffableDataSourceSectionSnapshot<NavigationSidebarItem> snapshot)
        {
            if (this.dataSource is null)
            {
                return;
            }

            // Add base sidebar items
            var sectionIdentifier = new NSString("base");
            this.dataSource.ApplySnapshot(snapshot, sectionIdentifier, false);
        }

        private NSDiffableDataSourceSectionSnapshot<NavigationSidebarItem> GetNavigationSnapshot(List<FeedListItem> items)
        {
            var navigationSidebarItems = items.Select(n => new NavigationSidebarItem(n));

            var snapshot = new NSDiffableDataSourceSectionSnapshot<NavigationSidebarItem>();
            snapshot.AppendItems(navigationSidebarItems.ToArray());
            return snapshot;
        }

        private UICollectionViewLayout CreateLayout()
        {
            var config = new UICollectionLayoutListConfiguration(UICollectionLayoutListAppearance.Sidebar);
            config.HeaderMode = UICollectionLayoutListHeaderMode.None;
            config.ShowsSeparators = false;

            return UICollectionViewCompositionalLayout.GetLayout(config);
        }

        private void ConfigureDataSource()
        {
            var rowRegistration = UICollectionViewCellRegistration.GetRegistration(typeof(UICollectionViewListCell),
               new UICollectionViewCellRegistrationConfigurationHandler((cell, indexpath, item) =>
               {
                   var sidebarItem = item as NavigationSidebarItem;
                   if (sidebarItem is null)
                   {
                       return;
                   }

                   var cfg = UIListContentConfiguration.SidebarCellConfiguration;
                   cfg.Text = sidebarItem.FeedListItem.Name;
                   cfg.Image = sidebarItem.Image;

                   cell.ContentConfiguration = cfg;
               })
            );

            if (this.collectionView is null)
            {
                throw new NullReferenceException(nameof(this.collectionView));
            }

            this.dataSource = new UICollectionViewDiffableDataSource<NSString, NavigationSidebarItem>(this.collectionView,
                new UICollectionViewDiffableDataSourceCellProvider((collectionView, indexPath, item) =>
                {
                    var sidebarItem = item as NavigationSidebarItem;
                    if (sidebarItem is null || this.collectionView is null)
                    {
                        throw new Exception();
                    }
                    return this.collectionView.DequeueConfiguredReusableCell(rowRegistration, indexPath, item);
                })
            );
        }

        private async void OpenAddFeed(object? sender, EventArgs e)
        {
            if (this.window.Page is null)
            {
                return;
            }

            var result = await this.window.Page.DisplayPromptAsync(Translations.Common.NewFeedListItemTitle, Translations.Common.NewFeedListItemTitle, initialValue: "https://devblogs.microsoft.com/dotnet/category/maui/feed/", keyboard: Microsoft.Maui.Keyboard.Url);
            if (result is null)
            {
                return;
            }

            await this.vm.AddOrUpdateNewFeedListItemAsync(result);
        }

        private void Vm_OnNewFeedItemAdded(object? sender, Events.NewFeedItemEventArgs e)
        {
            this.SetupNavigationItems(this.GetNavigationSnapshot(this.vm.Feeds));
        }
    }
}
#endif
