# **Net.Utils** - Network utilities.

[![NuGet version](https://img.shields.io/nuget/v/Net.Utils.svg?style=flat)](https://www.nuget.org/packages/Net.Utils/)
[![NuGet downloads](https://img.shields.io/nuget/dt/Net.Utils.svg)](https://www.nuget.org/packages/Net.Utils/)

# Net.Utils
- HttpClientEntity
- PingEntity
- ProxyEntity

# Net.Utils.Tests
- EnumValues
- HttpClientEntityTests
- ProxyEntityTests
- Utils

## Multi-targeted platforms
- netstandard2.0
- netstandard2.1
- net45
- net46
- net461
- net472
- net48

## How to use
### Example of HttpClientEntity usage
```C#
var proxy = new ProxyEntity();
var httpClient = new HttpClientEntity(timeout: 50, host: "http://google.com/");
var task = Task.Run(async () =>
{
    await httpClient.OpenAsync(proxy).ConfigureAwait(true);
});
task.Wait();
```
### Example of PingEntity usage
```C#
var ping = new PingEntity(timeoutPing: 100, bufferSize: 32, ttl: 128, dontFragment: true, timeoutTask: 1000, useRepeat: false);
ping.Hosts.Add("google.com");
ping.Hosts.Add("microsoft.com");
ping.Hosts.Add("yandex.com");
var task = Task.Run(async () =>
{
    await ping.OpenAsync().ConfigureAwait(true);
});
task.Wait();
TestContext.WriteLine($@"{ping.Settings}");
TestContext.WriteLine($@"{ping.Log}");
```
### Example of ProxyEntity usage
```C#
var proxy = new Net.Utils.ProxyEntity(use, useDefaultCredentials, host, port, domain, username, password);
```
## Net.Examples
[Visit this repo for view examples](https://github.com/DamianMorozov/Net.Examples)

## Telegram bot
[@NetUtilsBot](tg://resolve?domain=NetUtilsBot)

### Please, if this tool has been useful for you consider to donate or click on the `star` button
[![Buy me a coffee](Assets/Buy_me_a_coffee.png?raw=true)](https://www.buymeacoffee.com/DamianVM)
