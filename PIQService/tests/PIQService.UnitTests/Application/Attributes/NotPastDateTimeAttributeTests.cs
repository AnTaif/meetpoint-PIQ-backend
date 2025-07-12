using FluentAssertions;
using PIQService.Application.Attributes;

namespace PIQService.UnitTests.Application.Attributes;

[TestFixture]
public class NotPastDateTimeAttributeTests
{
    private NotPastDateTimeAttribute attribute;

    [SetUp]
    public void SetUp()
    {
        attribute = new NotPastDateTimeAttribute();
    }

    [TestCase(1, true)]
    [TestCase(-1, false)]
    public void IsValid_WithDifferentMinutesOffsets(int minutesOffset, bool expected)
    {
        var date = DateTime.UtcNow.AddMinutes(minutesOffset);

        var actual = attribute.IsValid(date);

        actual.Should().Be(expected);
    }

    [Test]
    public void IsValid_WhenValueIsNotDateTime_ReturnsFalse()
    {
        var actual = attribute.IsValid("not a date");

        actual.Should().BeFalse();
    }

    [Test]
    public void IsValid_WhenValueIsNull_ReturnsFalse()
    {
        var actual = attribute.IsValid(null);

        actual.Should().BeFalse();
    }
}