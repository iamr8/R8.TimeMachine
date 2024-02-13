using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace R8.TimeMachine.AspNetCore
{
    public class TimeMachineMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TimeMachineOptions _options;

        public TimeMachineMiddleware(RequestDelegate next, TimeMachineOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            var timezone = LocalTimezone.Create(await _options.Provider.Invoke(context));
            LocalTimezone.StartScope(timezone);

            try
            {
                await _next(context);
            }
            finally
            {
                LocalTimezone.EndScope();
            }
        }
    }
}