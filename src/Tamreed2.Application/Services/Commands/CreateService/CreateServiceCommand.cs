using MediatR;

namespace Tamreed2.Application.Services.Commands.CreateService;

public sealed record CreateServiceCommand(
    string Title,
    string Name,
    string Description,
    IReadOnlyList<string> Points,
    string TitleAr,
    string NameAr,
    string DescriptionAr,
    IReadOnlyList<string> PointsAr
) : IRequest<Guid>;
