using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorInterop.Client
{
    public class HelloHelper
    {
        public HelloHelper(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name property that is displayed in the string returned
        /// when SayHello is invoked
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// SayHello - This method is invoked from JavaScript.  This method returns
        /// a string with the supplied name property.
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public string SayHello() => $"Hello, {Name}!";
    }
}
