// using FluentAssertions;
// using Microsoft.Extensions.DependencyInjection;
// using Xunit;
// using Xunit.Abstractions;
//
// namespace R8.TimeMachine.Tests
// {
//     public class TimeMachineTests
//     {
//         private readonly IServiceCollection _serviceCollection;
//
//         public TimeMachineTests(ITestOutputHelper outputHelper)
//         {
//             _serviceCollection = new ServiceCollection()
//                 .AddSingleton<ITimeMachine, TimeMachine>()
//                 .AddSingleton<IControllableTimerFactory, ControllableTimerFactory>();
//         }
//         [Fact]
//         public void should()
//         {
//             var serviceProvider = _serviceCollection.BuildServiceProvider();
//             
//             var timeMachine = serviceProvider.GetRequiredService<ITimeMachine>();
//             var controllableTimerFactory = serviceProvider.GetRequiredService<IControllableTimerFactory>();
//             
//             var tzdt = timeMachine.Now();
//             var utcNow = timeMachine.UtcNow();
//             
//             tzdt.Should().NotBeNull();
//             utcNow.Should().NotBe(default);
//             
//         }
//     }
// }

