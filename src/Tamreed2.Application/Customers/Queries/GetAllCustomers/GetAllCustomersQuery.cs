using MediatR;
using Tamreed2.Contracts.Customers;

namespace Tamreed2.Application.Customers.Queries.GetAllCustomers;

public sealed record GetAllCustomersQuery(string? Search)
    : IRequest<IReadOnlyList<CustomerResponse>>;
