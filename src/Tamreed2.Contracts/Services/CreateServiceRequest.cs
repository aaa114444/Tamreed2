namespace Tamreed2.Contracts.Services;

public sealed record CreateServiceRequest(
    string Name,
    string Description,
    decimal Price
);