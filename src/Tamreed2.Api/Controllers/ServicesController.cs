using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tamreed2.Application.Services.Commands.CreateService;
using Tamreed2.Application.Services.Commands.DeleteService;
using Tamreed2.Application.Services.Commands.UpdateService;
using Tamreed2.Application.Services.Queries.GetAllServices;
using Tamreed2.Application.Services.Queries.GetServiceById;
using Tamreed2.Contracts.Services;

namespace Tamreed2.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly ISender _sender;

    public ServicesController(ISender sender)
    {
        _sender = sender;
    }

    private static Tamreed2.Domain.Services.ServiceDetailsContent MapDetails(
        ServiceDetailsContentRequest request)
    {
        return new Tamreed2.Domain.Services.ServiceDetailsContent
        {
            AboutTitle = request.AboutTitle,
            AboutText = request.AboutText,
            AboutTitleAr = request.AboutTitleAr,
            AboutTextAr = request.AboutTextAr,
            AudienceTitle = request.AudienceTitle,
            AudienceDescription = request.AudienceDescription,
            AudienceTitleAr = request.AudienceTitleAr,
            AudienceDescriptionAr = request.AudienceDescriptionAr,
            AudienceItems = request.AudienceItems.ToList(),
            AudienceItemsAr = request.AudienceItemsAr.ToList(),
            IncludedTitle = request.IncludedTitle,
            IncludedDescription = request.IncludedDescription,
            IncludedTitleAr = request.IncludedTitleAr,
            IncludedDescriptionAr = request.IncludedDescriptionAr,
            IncludedItems = request.IncludedItems.ToList(),
            IncludedItemsAr = request.IncludedItemsAr.ToList(),
            ProcessTitle = request.ProcessTitle,
            ProcessDescription = request.ProcessDescription,
            ProcessTitleAr = request.ProcessTitleAr,
            ProcessDescriptionAr = request.ProcessDescriptionAr,
            Steps = request.Steps.Select(step => new Tamreed2.Domain.Services.ServiceStepContent
            {
                Title = step.Title,
                Description = step.Description,
                TitleAr = step.TitleAr,
                DescriptionAr = step.DescriptionAr
            }).ToList(),
            FaqTitle = request.FaqTitle,
            FaqDescription = request.FaqDescription,
            FaqTitleAr = request.FaqTitleAr,
            FaqDescriptionAr = request.FaqDescriptionAr,
            Faqs = request.Faqs.Select(faq => new Tamreed2.Domain.Services.ServiceFaqContent
            {
                Question = faq.Question,
                Answer = faq.Answer,
                QuestionAr = faq.QuestionAr,
                AnswerAr = faq.AnswerAr
            }).ToList()
        };
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateService(
        [FromBody] CreateServiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateServiceCommand(
            request.Title,
            request.Name,
            request.Description,
            request.Points,
            request.TitleAr,
            request.NameAr,
            request.DescriptionAr,
            request.PointsAr,
            MapDetails(request.Details));

        var serviceId = await _sender.Send(command, cancellationToken);

        return Created(
            $"/api/services/{serviceId}",
            new { Id = serviceId });
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetServiceById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var service = await _sender.Send(
            new GetServiceByIdQuery(id),
            cancellationToken);

        if (service is null)
        {
            return NotFound(new
            {
                Message = $"Service with ID '{id}' was not found."
            });
        }

        return Ok(service);
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ServiceResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllServices(
        CancellationToken cancellationToken)
    {
        var services = await _sender.Send(
            new GetAllServicesQuery(),
            cancellationToken);

        return Ok(services);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateService(
        Guid id,
        [FromBody] UpdateServiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateServiceCommand(
            id,
            request.Title,
            request.Name,
            request.Description,
            request.Points,
            request.TitleAr,
            request.NameAr,
            request.DescriptionAr,
            request.PointsAr,
            MapDetails(request.Details));

        var serviceId = await _sender.Send(command, cancellationToken);

        return Ok(new
        {
            Id = serviceId,
            Message = "Service updated successfully."
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteService(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteServiceCommand(id),
            cancellationToken);

        return NoContent();
    }
}


