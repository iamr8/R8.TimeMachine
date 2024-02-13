using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using R8.XunitLogger;
using Xunit;

namespace R8.TimeMachine.AspNetCore.Tests;

public class Fixture : IAsyncLifetime, IXunitLogProvider
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;

    public Fixture()
    {
        _webApplicationFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services => { services.AddXunitLogger(msg => OnWriteLine?.Invoke(msg), o => o.MinimumLevel = LogLevel.Debug); });
            builder.UseTestServer();
        });
    }

    protected internal IServiceProvider ServiceProvider { get; private set; }

    public Task InitializeAsync()
    {
        ServiceProvider = _webApplicationFactory.Services;
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _webApplicationFactory.DisposeAsync();
    }

    public event Action<string>? OnWriteLine;
}