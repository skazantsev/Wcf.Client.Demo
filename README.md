# WCF CLIENT DEMO [![Build status](https://ci.appveyor.com/api/projects/status/ea815p4vm4mu7wac?svg=true)](https://ci.appveyor.com/project/skazantsev/wcf-client-demo)
A simple application demonstrating interaction with WCF services using ChannelFactory. 

According to [Michèle Leroux Bustamante's article](http://devproconnections.com/net-framework/wcf-proxies-cache-or-not-cache) the ChannelFactory is cached and a new WCF channel is created on each call.

The implementation is based on [Darin Dimitrov's SO answer](http://stackoverflow.com/a/3201001).

**Synchronous calls**
``` csharp
var invoker = new ServiceInvoker(); // or just use DI container
var result = invoker.InvokeService((ICalculatorService x) => x.Multiply(5, 6));
//  or
var result = invoker.InvokeService<ICalculatorService, double>(x => x.Multiply(5, 6));

//  Method's signature: double Multiply(double num1, double num2);
```

**Asynchronous calls**
``` csharp
var invoker = new ServiceInvoker(); // or just use DI container
var result = await invoker.InvokeService(async (ICalculatorService x) => await x.Multiply(5, 6));
//  or
var result = await invoker.InvokeService<ICalculatorService, Task<double>>(async x => await x.Multiply(5, 6));

//  Method's signature: Task<double> Multiply(double num1, double num2);
```
