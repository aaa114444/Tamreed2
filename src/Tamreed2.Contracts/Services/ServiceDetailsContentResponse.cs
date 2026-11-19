namespace Tamreed2.Contracts.Services;

public sealed record ServiceDetailsContentResponse(
    string AboutTitle,
    string AboutText,
    string AboutTitleAr,
    string AboutTextAr,
    string AudienceTitle,
    string AudienceDescription,
    string AudienceTitleAr,
    string AudienceDescriptionAr,
    IReadOnlyList<string> AudienceItems,
    IReadOnlyList<string> AudienceItemsAr,
    string IncludedTitle,
    string IncludedDescription,
    string IncludedTitleAr,
    string IncludedDescriptionAr,
    IReadOnlyList<string> IncludedItems,
    IReadOnlyList<string> IncludedItemsAr,
    string ProcessTitle,
    string ProcessDescription,
    string ProcessTitleAr,
    string ProcessDescriptionAr,
    IReadOnlyList<ServiceStepContentResponse> Steps,
    string FaqTitle,
    string FaqDescription,
    string FaqTitleAr,
    string FaqDescriptionAr,
    IReadOnlyList<ServiceFaqContentResponse> Faqs
);

public sealed record ServiceStepContentResponse(
    string Title,
    string Description,
    string TitleAr,
    string DescriptionAr
);

public sealed record ServiceFaqContentResponse(
    string Question,
    string Answer,
    string QuestionAr,
    string AnswerAr
);
