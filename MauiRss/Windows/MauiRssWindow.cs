#if !IOS && !WINDOWS && !__MACCATALYST__
using DrasticMaui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiRss
{
    /// <summary>
    /// Maui Rss iOS Window.
    /// </summary>
    public class MauiRssWindow : DrasticMauiWindow
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