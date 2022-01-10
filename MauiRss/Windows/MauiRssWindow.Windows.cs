// <copyright file="MauiRssWindow.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

#if WINDOWS
using DrasticMaui;
using DrasticMaui.Tools;
using MauiRss.Models;
using MauiRss.ViewModels;
using Microsoft.UI.Xaml.Input;
using Windows.Storage.Streams;
using WinUIControls = Microsoft.UI.Xaml.Controls;

namespace MauiRss
{
    /// <summary>
    /// Maui Rss iOS Window.
    /// </summary>
    public class MauiRssWindow : DrasticMauiWindow
    {
        private bool isInitialized;

        private Microsoft.UI.Xaml.Controls.NavigationView navigationView;
        private Microsoft.UI.Xaml.Controls.SplitView splitView;
        private Controls.HeaderControl header;
        private Controls.FeedContentControl feedContent;
        private Controls.FeedListControl feedListControl;
        private MauiRssWindowViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="MauiRssWindow"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public MauiRssWindow(IServiceProvider services)
            : base(services)
        {
            // We're ignoring the default page on the window...
            this.Page = new BlankPage();
            this.header = new Controls.HeaderControl();
            this.header.OnNewItemClickedEvent += Header_OnNewItemClickedEvent;
            this.splitView = new Microsoft.UI.Xaml.Controls.SplitView();
            this.navigationView = new Microsoft.UI.Xaml.Controls.NavigationView();
            this.vm = new MauiRssWindowViewModel(services);
            this.vm.OnNewFeedItemAdded += Vm_OnNewFeedItemAdded;
            this.feedContent = new Controls.FeedContentControl(this.vm);
            this.feedListControl = new Controls.FeedListControl(this.vm);
        }

        private void Vm_OnNewFeedItemAdded(object? sender, Events.NewFeedItemEventArgs e)
        {
            this.navigationView.MenuItems.Add(this.GenerateNavItem(e.FeedListItem));
        }

        public override void AddVisualChildren(List<IVisualTreeElement> elements)
        {
            base.AddVisualChildren(elements);

            if (this.feedContent is IVisualTreeElement feedContentElement)
            {
                foreach (var element in feedContentElement.GetVisualChildren())
                {
                    elements.Add(element);
                }
            }

            if (this.feedListControl is IVisualTreeElement feedListElement)
            {
                foreach (var element in feedListElement.GetVisualChildren())
                {
                    elements.Add(element);
                }
            }

            if (this.header is IVisualTreeElement headerControl)
            {
                foreach (var element in headerControl.GetVisualChildren())
                {
                    elements.Add(element);
                }
            }
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

        private void SetupNavigationItems()
        {
            this.navigationView.MenuItems.Clear();

            foreach (var item in this.vm.Feeds)
            {
                this.navigationView.MenuItems.Add(this.GenerateNavItem(item));
            }
        }

        private WinUIControls.NavigationViewItem GenerateNavItem(FeedListItem item)
        {
            if (item.ImageCache is null)
            {
                throw new InvalidOperationException("ImageCache must not be null");
            }

            var icon = new Microsoft.UI.Xaml.Media.Imaging.BitmapImage();
            icon.SetSource(ToRandomAccessStream(item.ImageCache));

            return new WinUIControls.NavigationViewItem() { Tag = item.Id, Content = item.Name, Icon = new WinUIControls.ImageIcon() { Source = icon } };
        }

        private void SetupNavigationView()
        {
            var handler = this.Handler as Microsoft.Maui.Handlers.WindowHandler;
            if (handler?.NativeView is not Microsoft.UI.Xaml.Window window)
            {
                return;
            }

            if (window.Content is not Microsoft.UI.Xaml.Controls.Panel panel)
            {
                return;
            }

            panel.PointerMoved += this.Panel_PointerMoved;

            this.navigationView.Content = this.splitView;
            this.splitView.Pane = this.feedListControl.ToHandler(this.Handler.MauiContext).GetWrappedNativeView();
            this.splitView.Content = this.feedContent.ToHandler(this.Handler.MauiContext).GetWrappedNativeView();
            var header = this.header.ToHandler(this.Handler.MauiContext);

            this.navigationView.PaneHeader = header.GetWrappedNativeView();
            this.navigationView.IsSettingsVisible = false;
            this.navigationView.SelectionChanged += NavigationView_SelectionChanged;

            this.splitView.IsPaneOpen = true;
            this.splitView.DisplayMode = WinUIControls.SplitViewDisplayMode.Inline;

            panel.Children.RemoveAt(0);
            panel.Children.Add(this.navigationView);

            this.SetupNavigationItems();
            window.ExtendsContentIntoTitleBar = false;
            this.isInitialized = true;
        }

        private void NavigationView_SelectionChanged(WinUIControls.NavigationView sender, WinUIControls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is WinUIControls.NavigationViewItem item)
            {
                var feedItem = this.vm.Feeds.FirstOrDefault(x => x.Id == (int)item.Tag);
                if (feedItem is null)
                {
                    return;
                }

                this.vm.SetFeedListItem(feedItem);
            }
        }

        private void Panel_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //if (this.navigationView is null)
            //{
            //    return;
            //}

            //var pointerPoint = e.GetCurrentPoint(this.navigationView);
            //if (pointerPoint == null)
            //{
            //    return;
            //}

            //if (this.navigationView.Content is not Microsoft.UI.Xaml.FrameworkElement element)
            //{
            //    return;
            //}

            //this.navigationView.IsHitTestVisible = !element.GetBoundingBox().Contains(new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y));
        }

        private async void Header_OnNewItemClickedEvent(object? sender, EventArgs e)
        {
            if (this.Page is null)
            {
                return;
            }

            var result = await this.Page.DisplayPromptAsync(Translations.Common.NewFeedListItemTitle, Translations.Common.NewFeedListItemTitle, initialValue: "https://devblogs.microsoft.com/dotnet/category/maui/feed/", keyboard: Microsoft.Maui.Keyboard.Url);
            if (result is null)
            {
                return;
            }

            await this.vm.AddOrUpdateNewFeedListItemAsync(result);
        }

        private static IRandomAccessStream ToRandomAccessStream(byte[] array)
        {
            InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream();
            using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
            {
                writer.WriteBytes(array);
                writer.StoreAsync().GetResults();
            }

            return ms;
        }
    }
}
#endif
