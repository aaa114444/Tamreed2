using MediatR;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Contracts.Customers;
using Tamreed2.Domain.Users;

namespace Tamreed2.Application.Customers.Queries.GetAllCustomers;

public sealed class GetAllCustomersQueryHandler
    : IRequestHandler<GetAllCustomersQuery, IReadOnlyList<CustomerResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetAllCustomersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyList<CustomerResponse>> Handle(
        GetAllCustomersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);

        var customers = users
            .Where(user => user.Role == UserRole.User);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            customers = customers.Where(user =>
                user.FirstName.Contains(
                    search, StringComparison.OrdinalIgnoreCase) ||
                user.LastName.Contains(
                    search, StringComparison.OrdinalIgnoreCase) ||
                user.Email.Contains(
                    search, StringComparison.OrdinalIgnoreCase));
        }

        return customers
            .OrderBy(user => user.FirstName)
            .ThenBy(user => user.LastName)
            .Select(user => new CustomerResponse(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.IsActive,
                user.CreatedAtUtc,
                user.LastLoginAtUtc
            ))
            .ToList();
    }
}
