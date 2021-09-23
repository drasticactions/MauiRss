// <copyright file="UrlImageConverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiRss.Tools.Converters
{
    /// <summary>
    /// Url Image Converter.
    /// </summary>
    public class UrlImageConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Uri urlValue)
            {
                return new UriImageSource() { Uri = urlValue };
            }

            return new FileImageSource() { File = "dotnet_bot.png" };
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
