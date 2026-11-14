using Tamreed2.Domain.Common.Models;

namespace Tamreed2.UnitTests.Domain.Common.Models;

public class ValueObjectTests
{
    [Fact]
    public void Equals_WhenValuesAreEqual_ShouldReturnTrue()
    {
        // Arrange
        var valueObject1 = new TestValueObject("Cairo", 100);
        var valueObject2 = new TestValueObject("Cairo", 100);

        // Act
        var result = valueObject1.Equals(valueObject2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WhenValuesAreDifferent_ShouldReturnFalse()
    {
        // Arrange
        var valueObject1 = new TestValueObject("Cairo", 100);
        var valueObject2 = new TestValueObject("Alexandria", 100);

        // Act
        var result = valueObject1.Equals(valueObject2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WhenTypesAreDifferent_ShouldReturnFalse()
    {
        // Arrange
        var valueObject1 = new TestValueObject("Cairo", 100);
        var valueObject2 = new AnotherTestValueObject("Cairo", 100);

        // Act
        var result = valueObject1.Equals(valueObject2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetHashCode_WhenValuesAreEqual_ShouldReturnSameHashCode()
    {
        // Arrange
        var valueObject1 = new TestValueObject("Cairo", 100);
        var valueObject2 = new TestValueObject("Cairo", 100);

        // Act
        var hash1 = valueObject1.GetHashCode();
        var hash2 = valueObject2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void EqualityOperator_WhenValuesAreEqual_ShouldReturnTrue()
    {
        // Arrange
        var valueObject1 = new TestValueObject("Cairo", 100);
        var valueObject2 = new TestValueObject("Cairo", 100);

        // Act & Assert
        Assert.True(valueObject1 == valueObject2);
    }

    [Fact]
    public void InequalityOperator_WhenValuesAreDifferent_ShouldReturnTrue()
    {
        // Arrange
        var valueObject1 = new TestValueObject("Cairo", 100);
        var valueObject2 = new TestValueObject("Alexandria", 100);

        // Act & Assert
        Assert.True(valueObject1 != valueObject2);
    }

    private class TestValueObject : ValueObject
    {
        public string Name { get; }
        public int Value { get; }

        public TestValueObject(string name, int value)
        {
            Name = name;
            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
            yield return Value;
        }
    }

    private class AnotherTestValueObject : ValueObject
    {
        public string Name { get; }
        public int Value { get; }

        public AnotherTestValueObject(string name, int value)
        {
            Name = name;
            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
            yield return Value;
        }
    }
}