using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorInterop.Client
{
    public class ExampleJsInterop
    {
        private readonly IJSRuntime _jsRuntime;

        public ExampleJsInterop(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Call the JavaScript sayHello method
        /// </summary>
        /// <param name="name">The nane to pass to the sayHello method, via the 
        /// HelloHelper object.</param>
        /// <returns>Task containing the result of the invocation of the sayHello method.</returns>
        public ValueTask<object> CallHelloHelperSayHello(string name)
        {
            // sayHello is implemented in wwwroot/ExampleJsInterop.js.  This
            // invokes the Javascript function JavascriptToDotNet.sayHello 
            // passing in the HelloHelper .Net object reference
            return _jsRuntime.InvokeAsync<object>(
                "JavascriptToDotNet.sayHello",
                DotNetObjectReference.Create<HelloHelper>(new HelloHelper(name)));
        }

        /// <summary>
        /// Call the JavaScript runPromiseFunction
        /// </summary>
        /// <param name="method">The JavaScript method to invoke</param>
        /// <param name="message">The message to pass to the JavaScript method</param>
        /// <returns>The result of the invocation of the JavaScript 'method'.  This could be
        /// a promise result (resolved promise) or an exception the result of a (promise rejection)</returns>
        public Task<string> CallFunctionWithPromise(string method, string message)
        {
            // TaskCompletionSource (reference: 
            // https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskcompletionsource-1?view=netcore-3.0
            // represents the producer side of a Task<TResult> unbound to a delegate.  This is consumed with a call
            // to result = await exampleJsInterop.CallFunctionWithPromise(@"a message");
            var tcs = new TaskCompletionSource<string>();

            // Create the .Net reference object from PromiseHandler, initializing it with a TaskCompletionSource
            var promiseHandler = DotNetObjectReference.Create<PromiseHandler>(new PromiseHandler() { tcs = tcs });

            // Invoke the Javascript method asynchronously
            _jsRuntime.InvokeAsync<object>("JavascriptWithPromises.runPromiseFunction", promiseHandler, method, message);

            // The task now contains the json results, return the results via the task object
            return tcs.Task;
        }
    }
}
