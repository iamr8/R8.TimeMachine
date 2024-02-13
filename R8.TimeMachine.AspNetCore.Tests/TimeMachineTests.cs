using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace R8.TimeMachine.AspNetCore.Tests;

public class TimeMachineTests : IClassFixture<Fixture>, IDisposable
{
    private readonly Fixture _fixture;
    private readonly ITestOutputHelper _outputHelper;

    public TimeMachineTests(Fixture fixture, ITestOutputHelper outputHelper)
    {
        _fixture = fixture;
        _outputHelper = outputHelper;
        _fixture.OnWriteLine += outputHelper.WriteLine;
    }

    public void Dispose()
    {
        _fixture.OnWriteLine -= _outputHelper.WriteLine;
    }

    [Fact]
    public void should_return_TimeMachine_from_serviceprovider()
    {
        using var scope = _fixture.ServiceProvider.CreateAsyncScope();
        var timeMachine = scope.ServiceProvider.GetRequiredService<ITimeMachine>();

        timeMachine.Should().NotBeNull();
    }
}