# **Net.Utils** - Network utilities.

[![NuGet version](https://img.shields.io/nuget/v/Net.Utils.svg?style=flat)](https://www.nuget.org/packages/Net.Utils/)
[![NuGet downloads](https://img.shields.io/nuget/dt/Net.Utils.svg)](https://www.nuget.org/packages/Net.Utils/)

# Net.Utils
- HttpClientEntity
- ProxyEntity

# Net.Utils.Tests
- EnumValues
- HttpClientEntityTests
- ProxyEntityTests
- Utils


## How to use
### Example of ProxyEntity usage
```C#
var proxy = new Net.Utils.ProxyEntity(use, useDefaultCredentials, host, port, domain, username, password);
```
### Example of HttpClientEntity usage
```C#
var proxy = new ProxyEntity();
var httpClient = new HttpClientEntity(isTimeout, timeout, host)
{
    IsTaskWait = isTaskWait,
};
httpClient.OpenTask(proxy);
```
