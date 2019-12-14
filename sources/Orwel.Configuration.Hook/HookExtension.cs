using Orwel.Configuration.Hook;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Extension methods to <see cref="IConfigurationBuilder"/> to hook all registered <see cref="IConfigurationSource"/>.
    /// </summary>
    public static class HookExtension
    {
        /// <summary>
        /// Regsiter a hook to modify configuration' values on load.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/></param>
        /// <param name="hook">The hook method to modify configuration's value</param>
        /// <returns>The <see cref="IConfigurationBuilder"/></returns>
        public static IConfigurationBuilder Hook(this IConfigurationBuilder builder, Func<KeyValuePair<string, string>, string> hook)
        {
            if (hook == null)
                throw new ArgumentNullException(nameof(hook));
            for (int i = 0; i < builder.Sources.Count; i++)
                builder.Sources[i] = new HookConfigurationSource(builder.Sources[i], hook);
            return builder;
        }
    }
}