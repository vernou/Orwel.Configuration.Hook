using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Orwel.Configuration.Hook
{
    internal class HookConfigurationSource : IConfigurationSource
    {
        private readonly IConfigurationSource source;
        private readonly Func<KeyValuePair<string, string>, string> hook;

        public HookConfigurationSource(IConfigurationSource source, Func<KeyValuePair<string, string>, string> hook)
        {
            this.source = source;
            this.hook = hook;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new HookConfigurationProvider(source.Build(builder), hook);
        }
    }
}