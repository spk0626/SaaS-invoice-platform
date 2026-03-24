// tests/Invoice.IntegrationTests/Infrastructure/BaseIntegrationTest.cs
using Invoice.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Invoice.IntegrationTests.Infrastructure;

public abstract class BaseIntegrationTest
    : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    private readonly IServiceScope _scope;

    protected readonly HttpClient Client;
    protected readonly IMediator Mediator;
    protected readonly AppDbContext DbContext;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Client = factory.CreateClient();
        Mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
        DbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    public void Dispose() => _scope.Dispose();
}