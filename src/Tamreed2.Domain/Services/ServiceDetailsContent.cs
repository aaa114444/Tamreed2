namespace Tamreed2.Domain.Services;

public sealed class ServiceDetailsContent
{
    public string AboutTitle { get; set; } = "";
    public string AboutText { get; set; } = "";
    public string AboutTitleAr { get; set; } = "";
    public string AboutTextAr { get; set; } = "";

    public string AudienceTitle { get; set; } = "";
    public string AudienceDescription { get; set; } = "";
    public string AudienceTitleAr { get; set; } = "";
    public string AudienceDescriptionAr { get; set; } = "";
    public List<string> AudienceItems { get; set; } = new();
    public List<string> AudienceItemsAr { get; set; } = new();

    public string IncludedTitle { get; set; } = "";
    public string IncludedDescription { get; set; } = "";
    public string IncludedTitleAr { get; set; } = "";
    public string IncludedDescriptionAr { get; set; } = "";
    public List<string> IncludedItems { get; set; } = new();
    public List<string> IncludedItemsAr { get; set; } = new();

    public string ProcessTitle { get; set; } = "";
    public string ProcessDescription { get; set; } = "";
    public string ProcessTitleAr { get; set; } = "";
    public string ProcessDescriptionAr { get; set; } = "";
    public List<ServiceStepContent> Steps { get; set; } = new();

    public string FaqTitle { get; set; } = "";
    public string FaqDescription { get; set; } = "";
    public string FaqTitleAr { get; set; } = "";
    public string FaqDescriptionAr { get; set; } = "";
    public List<ServiceFaqContent> Faqs { get; set; } = new();
}

public sealed class ServiceStepContent
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string TitleAr { get; set; } = "";
    public string DescriptionAr { get; set; } = "";
}

public sealed class ServiceFaqContent
{
    public string Question { get; set; } = "";
    public string Answer { get; set; } = "";
    public string QuestionAr { get; set; } = "";
    public string AnswerAr { get; set; } = "";
}
