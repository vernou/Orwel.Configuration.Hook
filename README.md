# Orwel.Configuration.Hook
[![Build and Test](https://github.com/Orwel/Orwel.Configuration.Hook/workflows/Build%20and%20Test/badge.svg)](https://github.com/Orwel/Orwel.Configuration.Hook/actions?query=workflow%3A%22Build+and+Test%22)
[![codecov](https://codecov.io/gh/Orwel/Orwel.Configuration.Hook/branch/master/graph/badge.svg)](https://codecov.io/gh/Orwel/Orwel.Configuration.Hook)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A library to hook Microsoft.Extensions.Configuration on Load to modify settings' value.

### Installation

Orwel.Configuration.Hook is available on [NuGet](https://www.nuget.org/packages/Orwel.Configuration.Hook). You can add the library to your project with Visual Studio NuGet Packages Manager or with the dotnet cli command :

```sh
dotnet add package Orwel.Configuration.Hook
```

### Usage

The library has the extension method `Hook` on IConfigurationBuilder.
The following code demonstrates basic usage :

```cs
private static void ApplyHook(IConfigurationBuilder config)
{
    config.Hook(Hook);
}

private static string Hook(KeyValuePair<string, string> setting)
{
    // Prepare Url in encoded format
    if (setting.Key.StartsWith("Urls:"))
        return WebUtility.UrlEncode(setting.Value);
    // Decrypt secret
    if (setting.Key.StartsWith("Secrets:"))
        return setting.Value;
    // Remove developpements settings
    if (setting.Key.StartsWith("OnlyDev:"))
        return null;
    // Return the raw value
    return setting.Value;
}
```

With ASP.NET Core :
```cs
WebHost.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((host, config) => config.Hook(Hook))
    .UseStartup<Startup>();
```

## Contributing to this project

Anyone and everyone is welcome to contribute. Please take a moment to
review the [guidelines for contributing](CONTRIBUTING.md).

* [Ask a question](CONTRIBUTING.md#questions)
* [Bug reports](CONTRIBUTING.md#bug-reports)
* [Feature requests](CONTRIBUTING.md#feature-requests)
* [Pull requests](CONTRIBUTING.md#pull-requests)
