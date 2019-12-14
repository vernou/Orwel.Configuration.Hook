using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orwel.Configuration.Hook
{
    internal class HookConfigurationProvider : ConfigurationProvider
    {
        private readonly IConfigurationProvider provider;
        private readonly Func<KeyValuePair<string, string>, string> hook;

        public HookConfigurationProvider(IConfigurationProvider provider, Func<KeyValuePair<string, string>, string> hook)
        {
            this.provider = provider;
            this.hook = hook;

            ChangeToken.OnChange(GetReloadToken, Load);
        }

        public override void Load()
        {
            Data.Clear();
            provider.Load();
            var keys = Keys();
            foreach (var key in keys)
            {
                if (provider.TryGet(key, out string value))
                {
                    var hooked = hook(new KeyValuePair<string, string>(key, value));
                    Data.Add(key, hooked);
                }
            }
        }

        public new IChangeToken GetReloadToken()
            => provider.GetReloadToken();

        public override void Set(string key, string value)
            => throw new NotImplementedException();

        private IEnumerable<string> Keys()
            => Keys(null);

        private IEnumerable<string> Keys(string parentPath)
        {
            var prefix = parentPath == null
                    ? string.Empty
                    : parentPath + ConfigurationPath.KeyDelimiter;

            var childKeys = provider
                .GetChildKeys(Enumerable.Empty<string>(), parentPath)
                .Distinct()
                .Select(k => prefix + k).ToList();

            if (childKeys.Any())
            {
                var keys = new List<string>();
                foreach (var key in childKeys)
                {
                    keys.AddRange(Keys(key));
                }
                return keys;
            }
            else if (string.IsNullOrEmpty(parentPath))
            {
                return Enumerable.Empty<string>();
            }
            else
            {
                return new[] { parentPath };
            }
        }
    }
}