// <copyright file="TemplateService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HandlebarsDotNet;
using MauiRss.Models;

namespace MauiRss.Services
{
    /// <summary>
    /// Template Service.
    /// </summary>
    public class TemplateService : ITemplateService
    {
        private HandlebarsTemplate<object, object> feedItemTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateService"/> class.
        /// </summary>
        public TemplateService()
        {
            this.feedItemTemplate = Handlebars.Compile(TemplateService.GetResourceFileContentAsString("Templates.feeditem.html.hbs"));
        }

        /// <inheritdoc/>
        public string RenderFeedItem(FeedItem item)
        {
            return this.feedItemTemplate.Invoke(item);
        }

        private static string GetResourceFileContentAsString(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MauiRss." + fileName;

            string resource = null;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using StreamReader reader = new StreamReader(stream);
                resource = reader.ReadToEnd();
            }

            return resource;
        }
    }
}
