using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace R8.TimeMachine.AspNetCore
{
    /// <summary>
    ///     An options to configure Time Machine middleware.
    /// </summary>
    public class TimeMachineOptions
    {
        internal TimeMachineOptions()
        {
        }

        /// <summary>
        ///     Gets or sets a provider to get timezone IANA ID.
        /// </summary>
        public Func<HttpContext, Task<string>> Provider { get; set; }
    }
}