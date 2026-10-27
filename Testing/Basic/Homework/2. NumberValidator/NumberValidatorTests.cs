using NUnit.Framework;
using NUnit.Framework.Legacy;
using FluentAssertions;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true, "precision must be a positive number", TestName = "Precision less than zero")]
    [TestCase(0, 2, false, "precision must be a positive number", TestName = "Precision is zero")]
    public void Constructor_InvalidPrecision_ThrowsArgumentException(int precision, int scale, bool onlyPositive, string expectedMessage)
    {
        var exeption = Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale, onlyPositive));
        Assert.That(exeption.Message, Is.EqualTo(expectedMessage));
    }

    [TestCase(3, -1, true, "precision must be a non-negative number less or equal than precision", TestName = "Scale less than zero")]
    [TestCase(2, 2, true, "precision must be a non-negative number less or equal than precision", TestName = "Scale equals precision")]
    [TestCase(2, 3, false, "precision must be a non-negative number less or equal than precision", TestName = "Scale exceeds precision")]
    public void Constructor_InvalidScale_ThrowsArgumentException(int precision, int scale, bool onlyPositive, string expectedMessage)
    {
        var exeption = Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale, onlyPositive));
        Assert.That(exeption.Message, Is.EqualTo(expectedMessage));
    }

    [TestCase(17, 2, true, "0.0", TestName = "Valid: Positive zero with decimal")]
    [TestCase(17, 2, true, "0,0", TestName = "Valid: Positive zero with comma")]
    [TestCase(17, 2, true, "0", TestName = "Valid: Integer zero")]
    [TestCase(3, 2, true, "1.23", TestName = "Valid: Number without sign fits precision")]
    [TestCase(4, 2, true, "+1.23", TestName = "Valid: Plus sign fits precision")]
    [TestCase(5, 2, true, "123.45", TestName = "Valid: Standard number with scale")]
    [TestCase(17, 2, false, "-1.23", TestName = "Valid: Negative number allowed")]
    public void IsValidNumber_ValidFormats_ReturnsTrue(int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        Assert.That(validator.IsValidNumber(value), Is.True);
    }

    [TestCase(17, 2, true, "", TestName = "Empty string")]
    [TestCase(17, 2, true, null, TestName = "Null value")]
    [TestCase(17, 2, true, "1.", TestName = "Ends with dot")]
    [TestCase(17, 2, true, "a.sd", TestName = "Contains letters")]
    public void IsValidNumber_InvalidFormats_ReturnsFalse(int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        Assert.That(validator.IsValidNumber(value), Is.False);
    }

    [TestCase(17, 2, true, "0.000", TestName = "Fractional part exceeds scale")]
    public void IsValidNumber_ScaleExceeded_ReturnsFalse(int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        Assert.That(validator.IsValidNumber(value), Is.False);
    }

    [TestCase(3, 2, true, "00.00", TestName = "Total length exceeds precision")]
    [TestCase(3, 2, true, "-0.00", TestName = "Negative zero exceeds precision")]
    [TestCase(3, 2, true, "+0.00", TestName = "Positive zero exceeds precision")]
    [TestCase(3, 2, true, "+1.23", TestName = "Plus sign causes precision overflow")]
    public void IsValidNumber_PrecisionExceeded_ReturnsFalse(int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        Assert.That(validator.IsValidNumber(value), Is.False);
    }

    [TestCase(17, 2, true, "-1.23", TestName = "Negative number when only positive is required")]
    public void IsValidNumber_OnlyPositiveViolated_ReturnsFalse(int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        Assert.That(validator.IsValidNumber(value), Is.False);
    }
}