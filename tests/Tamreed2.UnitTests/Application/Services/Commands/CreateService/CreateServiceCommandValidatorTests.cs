using Tamreed2.Application.Services.Commands.CreateService;
using Xunit;

namespace Tamreed2.UnitTests.Application.Services.Commands.CreateService;

public class CreateServiceCommandValidatorTests
{
    private readonly CreateServiceCommandValidator _validator = new();

    private static CreateServiceCommand ValidCommand() =>
        new("Home Nursing", "Nursing", "Description",
            new[] { "Point 1" }, "??????? ???????",
            "?????", "?????", new[] { "?????? ??????" });

    [Fact]
    public void Validate_WithValidCommand_ShouldPass()
    {
        var result = _validator.Validate(ValidCommand());
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithInvalidTitle_ShouldFail(string? title)
    {
        var command = ValidCommand() with { Title = title!, TitleAr = null! };
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
    }
    [Fact]
    public void Validate_WithArabicOnlyTitle_ShouldPass()
    {
        var command = ValidCommand() with { Title = null! };
        Assert.True(_validator.Validate(command).IsValid);
    }
}
