using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tamreed2.Application.Articles.Commands.CreateArticle;
using Tamreed2.Application.Articles.Commands.UpdateArticle;
using Tamreed2.Application.Articles.Commands.DeleteArticle;
using Tamreed2.Application.Articles.Commands.PublishArticle;
using Tamreed2.Application.Articles.Commands.UnpublishArticle;
using Tamreed2.Application.Articles.Queries.GetPublishedArticles;
using Tamreed2.Application.Articles.Queries.GetPublishedArticleBySlug;
using Tamreed2.Application.Articles.Queries.GetAllArticlesForAdmin;
using Tamreed2.Contracts.Articles;

namespace Tamreed2.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ArticlesController : ControllerBase
{
    private readonly ISender _sender;

    public ArticlesController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetPublishedArticles(
        CancellationToken cancellationToken)
    {
        var articles = await _sender.Send(
            new GetPublishedArticlesQuery(), cancellationToken);

        return Ok(articles);
    }

    [AllowAnonymous]
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetPublishedArticleBySlug(
        string slug,
        CancellationToken cancellationToken)
    {
        var article = await _sender.Send(
            new GetPublishedArticleBySlugQuery(slug), cancellationToken);

        return article is null ? NotFound() : Ok(article);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public async Task<IActionResult> GetAllArticlesForAdmin(
        CancellationToken cancellationToken)
    {
        var articles = await _sender.Send(
            new GetAllArticlesForAdminQuery(), cancellationToken);

        return Ok(articles);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateArticle(
        [FromBody] CreateArticleRequest request,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(new CreateArticleCommand(
            request.Title, request.TitleAr, request.Slug,
            request.Excerpt, request.ExcerptAr,
            request.Content, request.ContentAr,
            request.CoverImageUrl, request.SeoTitle, request.SeoTitleAr,
            request.SeoDescription, request.SeoDescriptionAr),
            cancellationToken);

        return CreatedAtAction(
            nameof(GetPublishedArticleBySlug),
            new { slug = request.Slug },
            new { Id = id });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateArticle(
        Guid id,
        [FromBody] UpdateArticleRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await _sender.Send(new UpdateArticleCommand(
            id, request.Title, request.TitleAr, request.Slug,
            request.Excerpt, request.ExcerptAr,
            request.Content, request.ContentAr,
            request.CoverImageUrl, request.SeoTitle, request.SeoTitleAr,
            request.SeoDescription, request.SeoDescriptionAr),
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteArticle(
        Guid id,
        CancellationToken cancellationToken)
    {
        var deleted = await _sender.Send(
            new DeleteArticleCommand(id), cancellationToken);

        return deleted ? NoContent() : NotFound();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/publish")]
    public async Task<IActionResult> PublishArticle(
        Guid id,
        CancellationToken cancellationToken)
    {
        var published = await _sender.Send(
            new PublishArticleCommand(id), cancellationToken);

        return published ? NoContent() : NotFound();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/unpublish")]
    public async Task<IActionResult> UnpublishArticle(
        Guid id,
        CancellationToken cancellationToken)
    {
        var unpublished = await _sender.Send(
            new UnpublishArticleCommand(id), cancellationToken);

        return unpublished ? NoContent() : NotFound();
    }
}
