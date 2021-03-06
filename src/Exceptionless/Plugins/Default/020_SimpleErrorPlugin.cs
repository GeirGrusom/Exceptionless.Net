﻿using System;
using Exceptionless.Extensions;
using Exceptionless.Models;

namespace Exceptionless.Plugins.Default {
    [Priority(20)]
    public class SimpleErrorPlugin : IEventPlugin {
        [Android.Preserve]
        public SimpleErrorPlugin() {}

        public void Run(EventPluginContext context) {
            var exception = context.ContextData.GetException();
            if (exception == null)
                return;

            if (exception.IsProcessed()) {
                context.Cancel = true;
                return;
            }

            if (String.IsNullOrEmpty(context.Event.Type))
                context.Event.Type = Event.KnownTypes.Error;

            context.Event.Data[Event.KnownDataKeys.SimpleError] = exception.ToSimpleErrorModel(context.Client);
            exception.MarkProcessed();
        }
    }
}