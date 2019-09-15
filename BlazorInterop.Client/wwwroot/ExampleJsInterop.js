//
// ExampleJsInterop.js - The JavaScript side of the Blazor Interop example application
// 
// Todd Littlejohn, 21 July 2019
//

window.ConvertArray = (win1251Array) => {
    // Instantiate the decoder with the 1251 decoder
    var win1251decoder = new TextDecoder('windows-1251');
    // Create the byte array
    var bytes = new Uint8Array(win1251Array);
    // Decode the array
    var decodedArray = win1251decoder.decode(bytes);
    // Log the decoded array
    console.log(decodedArray);
    // Return the decoded array
    return decodedArray;
};

window.JavascriptToDotNet = {
    returnArrayJsAsync: function () {
        // This invokes the .Net static method ReturnArrayAsync which returns
        // an array with three elements: 1, 2, 3.  The name of the assembly
        // containing the static function i.e., BlazorInterop.Client and the
        // name of the static method i.e., ReturnArrayAsync
        DotNet.invokeMethodAsync('BlazorInterop.Client', 'ReturnArrayAsync')
            // Then push the number four into the array
            .then(data => {
                data.push(4);
                console.log(data);
            });
    },
    sayHello: function (dotnetHelper) {
        // Use the dotnetHelper object to invoke the it's SayHello
        // .Net method
        return dotnetHelper.invokeMethodAsync('SayHello')
            .then(r => console.log(r));
    }
}

window.JavascriptWithPromises = {
    // This function simply asynchronously returns a promise that contains
    // the supplied message
    functionWithPromise: (messageToReturn) => {
        return new Promise((resolve, reject) => {
            // Wait 2 seconds and return a promise containing
            // the message
            setTimeout(() => {
                resolve(messageToReturn);
            }, 2000);
        });
    },
    functionThatMakesAnError: (messageToReturn) => {
        return new Promise((resolve, reject) => {
            // Wait 2 seconds and reject the promise
            setTimeout(() => {
                reject('An error has occurred, we\'re sorry');
            }, 2000);
        });
    },
    // This function calls the above promise function(s) with the supplied message
    // promiseHandler is a DotNet reference object that will process the
    // promise result on the .Net side.
    runPromiseFunction: (promiseHandler, method, message) => {
        let promise = window.JavascriptWithPromises[method](message);

        // Process the promise
        promise.then((value) => {
            // Set the returned value, if undefined, return an empty
            // string.
            if (value === undefined) value = '';
            // Log the returned value to the browser console
            console.log(value == '' ? 'Nothing returned from the promise' : value);
            // Convert the resulting promise value to JSON.  This will invoke the
            // promiseHandler's SetResult method.  The SetResult method will de-serialize the
            // result, convert it to a string and then set it as the TasCompletionSource
            // which will be used on the C# side to asynchronously process.
            promiseHandler.invokeMethodAsync('SetResult', JSON.stringify(value));
        }).catch((error) => {
            // Set the error
            if (error === undefined) error = "Something bad happened";
            // Log the error message to the browser console
            console.log(error);
            // Return the error message.  This will invoke the promiseHandler's
            // SetError method.  The SetError method will create an Exception
            // with the supplied error message and raise a TaskCompletionSource
            // Exception.
            promiseHandler.invokeMethodAsync('SetError', error);
        });
    }
}
