using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Orwel.Configuration.Hook.UnitTests
{
    public class ModifyConfigurationOnLoadTest
    {
        [Fact]
        public void WhenEmptyThenDoNothing()
        {
            Assert.Empty(new ConfigurationBuilder().Hook(kv => kv.Value).Build().AsEnumerable());
        }

        [Fact]
        public void WhenHookDoNothingThenNoModification()
        {
            Assert.Equal(
                new Dictionary<string, string> { { "Key1", "Value1" }, { "Key2", "Value2" }, { "Key3", "Value3" } },
                new ConfigurationBuilder()
                    .AddInMemoryCollection(
                        new Dictionary<string, string> { { "Key1", "Value1" }, { "Key2", "Value2" }, { "Key3", "Value3" } }
                    ).Hook(kv => kv.Value).Build().AsEnumerable().ToDictionary(kv => kv.Key, kv => kv.Value)
            );
        }

        [Fact]
        public void HookUpdateAllValues()
        {
            Assert.Equal(
                new Dictionary<string, string> { { "Key1", "Value1 Updated" }, { "Key2", "Value2 Updated" }, { "Key3", "Value3 Updated" } },
                new ConfigurationBuilder()
                    .AddInMemoryCollection(
                        new Dictionary<string, string> { { "Key1", "Value1" }, { "Key2", "Value2" }, { "Key3", "Value3" } }
                    ).Hook(kv => kv.Value + " Updated")
                    .Build().AsEnumerable().ToDictionary(kv => kv.Key, kv => kv.Value)
            );
        }

        [Fact]
        public void KeyIsCaseInsensitive()
        {
            var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(
                        new Dictionary<string, string> { { "Key", "Value" } }
                    ).Hook(kv => kv.Value).Build();
            Assert.Equal("Value", config["key"]);
            Assert.Equal("Value", config["Key"]);
            Assert.Equal("Value", config["KEy"]);
            Assert.Equal("Value", config["KEY"]);
            Assert.Equal("Value", config["kEy"]);
            Assert.Equal("Value", config["kEY"]);
            Assert.Equal("Value", config["keY"]);
        }

        [Fact]
        public void Reload()
        {
            var provider = new FakeConfigurationSourceProvider();
            var config = new ConfigurationBuilder().Add(provider).Hook(kv => kv.Value).Build();
            provider.ReloadData(new Dictionary<string, string> { { "Key1", "Value1" }, { "Key2", "Value2" }, { "Key3", "Value3" } }.AsEnumerable());
            Assert.Equal(
                new Dictionary<string, string> { { "Key1", "Value1" }, { "Key2", "Value2" }, { "Key3", "Value3" } },
                config.AsEnumerable().ToDictionary(kv => kv.Key, kv => kv.Value)
            );
        }
    }
}