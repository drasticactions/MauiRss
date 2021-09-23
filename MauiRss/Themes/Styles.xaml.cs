// <copyright file="Styles.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace MauiRss.Themes
{
    /// <summary>
    /// Styles.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Styles : ResourceDictionary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Styles"/> class.
        /// </summary>
        public Styles()
        {
            this.InitializeComponent();
        }
    }
}
