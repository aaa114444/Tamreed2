using Xunit;
using Tamreed2.Domain.Common.Models;

namespace Tamreed2.UnitTests.Domain.Common.Models;

public class EntityTests
{
    [Fact]
    public void Equals_WhenIdsAreEqual_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();

        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WhenIdsAreDifferent_ShouldReturnFalse()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WhenEntityTypesAreDifferent_ShouldReturnFalse()
    {
        // Arrange
        var id = Guid.NewGuid();

        var entity1 = new TestEntity(id);
        var entity2 = new AnotherTestEntity(id);

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Constructor_WhenIdIsEmpty_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => new TestEntity(Guid.Empty));
    }

    private class TestEntity : Entity
    {
        public TestEntity(Guid id) : base(id)
        {
        }
    }

    private class AnotherTestEntity : Entity
    {
        public AnotherTestEntity(Guid id) : base(id)
        {
        }
    }
}
