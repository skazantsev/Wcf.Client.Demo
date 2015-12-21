#WCF CLIENT DEMO
A simple application demonstrating interaction with WCF services using ChannelFactory.
According to [Michèle Leroux Bustamante's article](http://devproconnections.com/net-framework/wcf-proxies-cache-or-not-cache) the ChannelFactory is cached and a new WCF channel is created on each call . 

The implementation is based on [Darin Dimitrov's SO answer](http://stackoverflow.com/a/3201001).

**Synchronous calls**
``` csharp
var invoker = new ServiceInvoker(); // or just use DI container
var result = invoker.InvokeService((ICalculatorService x) => x.Multiply(5, 6)); //  double Multiply(double num1, double num2);
//  or
var result = invoker.InvokeService<ICalculatorService, double>(x => x.Multiply(5, 6));
```

**Asynchronous calls**
``` csharp
var invoker = new ServiceInvoker(); // or just use DI container
var result = await invoker.InvokeService(async (ICalculatorService x) => await x.Multiply(5, 6));
//  or
var result = await invoker.InvokeService<ICalculatorService, Task<double>>(async x => await x.Multiply(5, 6)); //  Task<double> Multiply(double num1, double num2);
```