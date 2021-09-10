// <copyright file="DataTemplates.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace MauiRss.Themes
{
    /// <summary>
    /// Data Templates.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataTemplates : ResourceDictionary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTemplates"/> class.
        /// </summary>
        public DataTemplates()
        {
            this.InitializeComponent();
        }
    }
}
