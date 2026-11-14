
namespace Tamreed2.Contracts.Services;

public sealed record UpdateServiceRequest(
    string Name,
    string Description,
    decimal Price
);
