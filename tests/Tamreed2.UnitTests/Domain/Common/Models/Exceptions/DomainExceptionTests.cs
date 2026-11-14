using Tamreed2.Domain.Common.Exceptions;

namespace Tamreed2.UnitTests.Domain.Common.Models.Exceptions;

public class DomainExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        const string message = "Booking date is invalid.";

        // Act
        var exception = new DomainException(message);

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_ShouldSetMessage()
    {
        // Arrange
        const string message = "A domain operation failed.";
        var innerException = new InvalidOperationException(
            "Original error.");

        // Act
        var exception = new DomainException(
            message,
            innerException);

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_ShouldPreserveInnerException()
    {
        // Arrange
        const string message = "A domain operation failed.";
        var innerException = new InvalidOperationException(
            "Original error.");

        // Act
        var exception = new DomainException(
            message,
            innerException);

        // Assert
        Assert.Same(innerException, exception.InnerException);
    }

    [Fact]
    public void DomainException_ShouldInheritFromException()
    {
        // Arrange
        var exception = new DomainException("Test error.");

        // Assert
        Assert.IsAssignableFrom<Exception>(exception);
    }
}
