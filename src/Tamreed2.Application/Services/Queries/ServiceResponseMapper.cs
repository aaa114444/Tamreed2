using Tamreed2.Contracts.Services;
using Tamreed2.Domain.Services;

namespace Tamreed2.Application.Services.Queries;

internal static class ServiceResponseMapper
{
    public static ServiceResponse Map(Service service)
    {
        var details = service.Details;

        return new ServiceResponse(
            service.Id,
            service.Title,
            service.Name,
            service.Description,
            service.Points,
            service.TitleAr,
            service.NameAr,
            service.DescriptionAr,
            service.PointsAr,
            new ServiceDetailsContentResponse(
                details.AboutTitle,
                details.AboutText,
                details.AboutTitleAr,
                details.AboutTextAr,
                details.AudienceTitle,
                details.AudienceDescription,
                details.AudienceTitleAr,
                details.AudienceDescriptionAr,
                details.AudienceItems,
                details.AudienceItemsAr,
                details.IncludedTitle,
                details.IncludedDescription,
                details.IncludedTitleAr,
                details.IncludedDescriptionAr,
                details.IncludedItems,
                details.IncludedItemsAr,
                details.ProcessTitle,
                details.ProcessDescription,
                details.ProcessTitleAr,
                details.ProcessDescriptionAr,
                details.Steps.Select(step => new ServiceStepContentResponse(
                    step.Title, step.Description, step.TitleAr, step.DescriptionAr)).ToList(),
                details.FaqTitle,
                details.FaqDescription,
                details.FaqTitleAr,
                details.FaqDescriptionAr,
                details.Faqs.Select(faq => new ServiceFaqContentResponse(
                    faq.Question, faq.Answer, faq.QuestionAr, faq.AnswerAr)).ToList()
            ),
            service.Status.ToString(),
            service.CreatedAtUtc);
    }
}
