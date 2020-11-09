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

[![Buy me a coffee](Assets/Buy_me_a_coffee.png?raw=true)](https://www.buymeacoffee.com/DamianVM)

## How to use
### Example of HttpClientEntity usage
```C#
var proxy = new ProxyEntity();
var httpClient = new HttpClientEntity(isTimeout, timeout, host)
{
    IsTaskWait = isTaskWait,
};
httpClient.OpenTask(proxy);
if (httpClient.IsTaskWait)
{
    var status = httpClient.Status;
    var content = httpClient.Content;
}
```
### Example of PingEntity usage
```C#
var ping = new PingEntity(isTimeout, timeout, false);
ping.Hosts.Add("google.com", false);
ping.OpenTask();
if (ping.IsTaskWait)
{
    var status = ping.Status;
}
```
### Example of ProxyEntity usage
```C#
var proxy = new Net.Utils.ProxyEntity(use, useDefaultCredentials, host, port, domain, username, password);
```
