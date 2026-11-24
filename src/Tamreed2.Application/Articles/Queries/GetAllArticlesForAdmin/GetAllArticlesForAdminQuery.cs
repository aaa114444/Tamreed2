using MediatR;
using Tamreed2.Contracts.Articles;

namespace Tamreed2.Application.Articles.Queries.GetAllArticlesForAdmin;

public sealed record GetAllArticlesForAdminQuery
    : IRequest<IReadOnlyList<ArticleResponse>>;
