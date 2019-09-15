using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorInterop.Client
{
    public interface IPromiseHandler
    {
        // Set the result of the promise
        void SetResult(string json);

        // Set the error when the promise encounters an exception
        void SetError(string error);
    }
}
