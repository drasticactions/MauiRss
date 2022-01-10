// <copyright file="MauiRssWindow.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

#if IOS || __MACCATALYST__
namespace MauiRss
{
    /// <summary>
    /// Maui Rss iOS Window.
    /// </summary>
    public class MauiRssWindow : DrasticMaui.DrasticMauiWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MauiRssWindow"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public MauiRssWindow(IServiceProvider services)
            : base(services)
        {
            this.Page = new MainPage();
        }
    }
}
#endif
