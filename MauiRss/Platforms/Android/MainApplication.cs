// <copyright file="MainApplication.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Android.App;
using Android.Runtime;
using Microsoft.Maui;

namespace MauiRss
{
    /// <summary>
    /// Main Application.
    /// </summary>
    [Application]
    public class MainApplication : MauiApplication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainApplication"/> class.
        /// Main Application.
        /// </summary>
        /// <param name="handle">App Handle.</param>
        /// <param name="ownership">Ownership.</param>
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}