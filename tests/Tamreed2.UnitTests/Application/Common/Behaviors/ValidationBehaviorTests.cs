
using FluentValidation;
using MediatR;
using Tamreed2.Application.Common.Behaviors;
using Xunit;

namespace Tamreed2.UnitTests.Application.Common.Behaviors;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_WithNoValidators_ShouldContinueToNextHandler()
    {
        var behavior = new ValidationBehavior<TestRequest, string>(
            Enumerable.Empty<IValidator<TestRequest>>());

        var request = new TestRequest("Home Nursing");
        var nextCalled = false;

        RequestHandlerDelegate<string> next = () =>
        {
            nextCalled = true;
            return Task.FromResult("Handler executed");
        };

        var result = await behavior.Handle(
            request, next, CancellationToken.None);

        Assert.Equal("Handler executed", result);
        Assert.True(nextCalled);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldContinueToNextHandler()
    {
        var behavior = new ValidationBehavior<TestRequest, string>(
            new[] { new TestRequestValidator() });

        var request = new TestRequest("Home Nursing");
        var nextCalled = false;

        RequestHandlerDelegate<string> next = () =>
        {
            nextCalled = true;
            return Task.FromResult("Success");
        };

        var result = await behavior.Handle(
            request, next, CancellationToken.None);

        Assert.Equal("Success", result);
        Assert.True(nextCalled);
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_ShouldThrowValidationException()
    {
        var behavior = new ValidationBehavior<TestRequest, string>(
            new[] { new TestRequestValidator() });

        var request = new TestRequest("");
        var nextCalled = false;

        RequestHandlerDelegate<string> next = () =>
        {
            nextCalled = true;
            return Task.FromResult("Should not execute");
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => behavior.Handle(
                request, next, CancellationToken.None));

        Assert.Contains(
            exception.Errors,
            error => error.ErrorMessage == "Request name is required.");

        Assert.False(nextCalled);
    }

    [Fact]
    public async Task Handle_WithMultipleValidationFailures_ShouldCollectAllErrors()
    {
        var validators = new IValidator<TestRequest>[]
        {
            new TestRequestValidator(),
            new TestRequestDescriptionValidator()
        };

        var behavior = new ValidationBehavior<TestRequest, string>(
            validators);

        var request = new TestRequest("");

        RequestHandlerDelegate<string> next = () =>
            Task.FromResult("Should not execute");

        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => behavior.Handle(
                request, next, CancellationToken.None));

        var errors = exception.Errors.ToList();

        Assert.Equal(2, errors.Count);

        Assert.Contains(
            errors,
            error => error.ErrorMessage == "Request name is required.");

        Assert.Contains(
            errors,
            error => error.ErrorMessage == "Request description is required.");
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToValidators()
    {
        using var cancellationTokenSource = new CancellationTokenSource();

        var expectedToken = cancellationTokenSource.Token;
        CancellationToken actualToken = default;

        var validator = new TestCancellationValidator(
            token => actualToken = token);

        var behavior = new ValidationBehavior<TestRequest, string>(
            new[] { validator });

        var request = new TestRequest("Home Nursing");

        RequestHandlerDelegate<string> next = () =>
            Task.FromResult("Success");

        await behavior.Handle(
            request, next, expectedToken);

        Assert.Equal(expectedToken, actualToken);
    }

    private sealed record TestRequest(string Name);

    private sealed class TestRequestValidator
        : AbstractValidator<TestRequest>
    {
        public TestRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .WithMessage("Request name is required.");
        }
    }

    private sealed class TestRequestDescriptionValidator
        : AbstractValidator<TestRequest>
    {
        public TestRequestDescriptionValidator()
        {
            RuleFor(request => request.Name)
                .MinimumLength(5)
                .WithMessage("Request description is required.");
        }
    }

    private sealed class TestCancellationValidator
        : AbstractValidator<TestRequest>
    {
        public TestCancellationValidator(
            Action<CancellationToken> captureToken)
        {
            RuleFor(request => request.Name)
                .MustAsync((_, cancellationToken) =>
                {
                    captureToken(cancellationToken);
                    return Task.FromResult(true);
                });
        }
    }
}
