using Tamreed2.Application.Services.Commands.CreateService;
using Xunit;

namespace Tamreed2.UnitTests.Application.Services.Commands.CreateService;

public class CreateServiceCommandValidatorTests
{
    private readonly CreateServiceCommandValidator _validator;

    public CreateServiceCommandValidatorTests()
    {
        _validator = new CreateServiceCommandValidator();
    }

    [Fact]
    public void Validate_WithValidCommand_ShouldPass()
    {
        var command = new CreateServiceCommand(
            "Home Nursing",
            "Professional nursing services at home.",
            500m);

        var result = _validator.Validate(command);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithInvalidName_ShouldFail(string? name)
    {
        var command = new CreateServiceCommand(
            name!,
            "Professional nursing services at home.",
            500m);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            error => error.PropertyName == "Name");
    }

    [Fact]
    public void Validate_WithNameExceeding150Characters_ShouldFail()
    {
        var name = new string('A', 151);

        var command = new CreateServiceCommand(
            name,
            "Professional nursing services at home.",
            500m);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            error => error.PropertyName == "Name");
    }

    [Fact]
    public void Validate_WithNameOfExactly150Characters_ShouldPass()
    {
        var name = new string('A', 150);

        var command = new CreateServiceCommand(
            name,
            "Professional nursing services at home.",
            500m);

        var result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithInvalidDescription_ShouldFail(
        string? description)
    {
        var command = new CreateServiceCommand(
            "Home Nursing",
            description!,
            500m);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            error => error.PropertyName == "Description");
    }

    [Fact]
    public void Validate_WithDescriptionExceeding1000Characters_ShouldFail()
    {
        var description = new string('A', 1001);

        var command = new CreateServiceCommand(
            "Home Nursing",
            description,
            500m);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            error => error.PropertyName == "Description");
    }

    [Fact]
    public void Validate_WithDescriptionOfExactly1000Characters_ShouldPass()
    {
        var description = new string('A', 1000);

        var command = new CreateServiceCommand(
            "Home Nursing",
            description,
            500m);

        var result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-500)]
    public void Validate_WithInvalidPrice_ShouldFail(decimal price)
    {
        var command = new CreateServiceCommand(
            "Home Nursing",
            "Professional nursing services at home.",
            price);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            error => error.PropertyName == "Price");
    }

    [Fact]
    public void Validate_WithPositivePrice_ShouldPass()
    {
        var command = new CreateServiceCommand(
            "Home Nursing",
            "Professional nursing services at home.",
            0.01m);

        var result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }
}