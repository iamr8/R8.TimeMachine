using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace R8.TimeMachine.AspNetCore
{
    public static class TimeMachineDependencyExtensions
    {
        /// <summary>
        ///     Adds Time Machine services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddTimeMachine(this IServiceCollection services)
        {
            services.AddSingleton<ITimeMachine, TimeMachine>();
            services.AddSingleton<IControllableTimerFactory, ControllableTimerFactory>();

            return services;
        }

        /// <summary>
        ///     Adds Time Machine middleware to application's request pipeline.
        /// </summary>
        /// <param name="app">An instance of <see cref="WebApplication" />.</param>
        /// <param name="options">An instance of <see cref="TimeMachineOptions" /> to configure the middleware.</param>
        /// <returns>The <see cref="IApplicationBuilder" /> instance.</returns>
        [DebuggerStepThrough]
        public static IApplicationBuilder UseTimeMachine(this IApplicationBuilder app, Action<TimeMachineOptions> options)
        {
            var opt = new TimeMachineOptions();
            options.Invoke(opt);
            app.UseMiddleware<TimeMachineMiddleware>(opt);
            return app;
        }
    }
}