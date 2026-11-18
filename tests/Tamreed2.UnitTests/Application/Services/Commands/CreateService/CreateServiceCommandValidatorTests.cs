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
        var command = ValidCommand() with { Title = title! };
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
    }
}
