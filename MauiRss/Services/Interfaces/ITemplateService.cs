// <copyright file="ITemplateService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiRss.Models;

namespace MauiRss.Services
{
    /// <summary>
    /// Template Service.
    /// </summary>
    public interface ITemplateService
    {
        /// <summary>
        /// Render Feed Item.
        /// </summary>
        /// <param name="item">FeedItem.</param>
        /// <returns>Html String.</returns>
        public string RenderFeedItem(FeedItem item);
    }
}
