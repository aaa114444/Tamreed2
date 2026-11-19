using Tamreed2.Application.Services.Commands.UpdateService;
using Xunit;

namespace Tamreed2.UnitTests.Application.Services.Commands.UpdateService;

public class UpdateServiceCommandValidatorTests
{
    private readonly UpdateServiceCommandValidator _validator = new();

    private static UpdateServiceCommand ValidCommand() =>
        new(
            Guid.NewGuid(),
            "Home Nursing",
            "Nursing",
            "Professional nursing services.",
            new[] { "Point 1" },
            "??????? ???????",
            "?????",
            "????? ????? ??????",
            new[] { "???? 1" });

    [Fact]
    public void Validate_WithValidCommand_ShouldPass()
    {
        Assert.True(_validator.Validate(ValidCommand()).IsValid);
    }

    [Fact]
    public void Validate_WithEmptyServiceId_ShouldFail()
    {
        var command = ValidCommand() with { ServiceId = Guid.Empty };
        Assert.False(_validator.Validate(command).IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithInvalidTitle_ShouldFail(string? title)
    {
        var command = ValidCommand() with { Title = title!, TitleAr = null! };
        Assert.False(_validator.Validate(command).IsValid);
    }
    [Fact]
    public void Validate_WithArabicOnlyTitle_ShouldPass()
    {
        var command = ValidCommand() with { Title = null! };
        Assert.True(_validator.Validate(command).IsValid);
    }
}
