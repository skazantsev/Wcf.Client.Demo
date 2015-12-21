#WCF CLIENT DEMO
A simple application demonstrating interaction with WCF services using ChannelFactory.

The implementation is based on [Darin Dmitrov's SO answer](http://stackoverflow.com/a/3201001).

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