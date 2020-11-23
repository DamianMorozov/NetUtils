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

## How to use
### Example of HttpClientEntity usage
```C#
var proxy = new ProxyEntity();
var httpClient = new HttpClientEntity(timeout, host);
httpClient.OpenTask(isTaskWait, proxy);
if (isTaskWait)
{
    TestContext.WriteLine($@"{httpClient.Status}");
    TestContext.WriteLine($@"{httpClient.Content}");
}
```
### Example of PingEntity usage
```C#
var ping = new PingEntity(timeoutPing: timeoutPing, timeoutTask: timeoutTask, useRepeat: false, useStopWatch: useStopWatch);
ping.Hosts.Add("localhost");
ping.Hosts.Add("google.com");
ping.Hosts.Add("google-fake.com");
var task = Task.Run(async () =>
{
    await ping.OpenAsync().ConfigureAwait(true);
});
task.Wait();
TestContext.WriteLine($@"{ping.Status}");
```
### Example of ProxyEntity usage
```C#
var proxy = new Net.Utils.ProxyEntity(use, useDefaultCredentials, host, port, domain, username, password);
```
## Net.Examples
[Visit this repo for view examples](https://github.com/DamianMorozov/Net.Examples)

## Please, if this tool has been useful for you consider to donate
[![Buy me a coffee](Assets/Buy_me_a_coffee.png?raw=true)](https://www.buymeacoffee.com/DamianVM)
