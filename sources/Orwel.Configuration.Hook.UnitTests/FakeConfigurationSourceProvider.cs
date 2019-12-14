using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Orwel.Configuration.Hook.UnitTests
{
    class FakeConfigurationSourceProvider : ConfigurationProvider, IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return this;
        }

        public void ReloadData(IEnumerable<KeyValuePair<string, string>> data)
        {
            Data.Clear();
            foreach (var d in data)
            {
                Data.Add(d.Key, d.Value);
            }
            OnReload();
        }
    }
}