using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, true, TestName = "Precision less than zero")]
    [TestCase(0, false, TestName = "Precision is zero")]
    public void Constructor_InvalidPrecision_ThrowsArgumentException(int precision, bool onlyPositive)
    {
        var exception = Assert.Throws<ArgumentException>(() => new NumberValidator(precision, 2, onlyPositive));
        Assert.That(exception.Message, Is.EqualTo("precision must be a positive number"));
    }

    [TestCase(3, -1, true, TestName = "Scale less than zero")]
    [TestCase(2, 2, true, TestName = "Scale equals precision")]
    [TestCase(2, 3, false, TestName = "Scale exceeds precision")]
    public void Constructor_InvalidScale_ThrowsArgumentException(int precision, int scale, bool onlyPositive)
    {
        var exception = Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale, onlyPositive));
        Assert.That(exception.Message, Is.EqualTo("precision must be a non-negative number less or equal than precision"));
    }

    [TestCase("0.0", TestName = "Valid: Positive zero with dot")]
    [TestCase("0,0", TestName = "Valid: Positive zero with comma")]
    [TestCase("123.45", TestName = "Valid: Standard number with dot")]
    public void IsValidNumber_ValidSeparators_ReturnsTrue(string value)
    {
        var validator = new NumberValidator(17, 2, true);
        Assert.That(validator.IsValidNumber(value), Is.True);
    }

    [TestCase(3, true, "1.23", TestName = "Valid: Number without sign")]
    [TestCase(4, true, "+1.23", TestName = "Valid: Plus sign fits precision")]
    [TestCase(17, false, "-1.23", TestName = "Valid: Negative number allowed")]
    public void IsValidNumber_SignsAndOnlyPositive_ReturnsTrue(int precision, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, 2, onlyPositive);
        Assert.That(validator.IsValidNumber(value), Is.True);
    }

    [TestCase("", TestName = "Empty string")]
    [TestCase(null, TestName = "Null value")]
    [TestCase("1.", TestName = "Ends with dot")]
    [TestCase("a.sd", TestName = "Contains letters")]
    public void IsValidNumber_InvalidFormats_ReturnsFalse(string value)
    {
        var validator = new NumberValidator(17, 2, true);
        Assert.That(validator.IsValidNumber(value), Is.False);
    }

    [TestCase("00.00", TestName = "Total length exceeds precision")]
    [TestCase("-0.00", TestName = "Negative zero exceeds precision")]
    [TestCase("+0.00", TestName = "Positive zero exceeds precision")]
    [TestCase("+1.23", TestName = "Plus sign causes precision overflow")]
    public void IsValidNumber_PrecisionExceeded_ReturnsFalse(string value)
    {
        var validator = new NumberValidator(3, 2, true);
        Assert.That(validator.IsValidNumber(value), Is.False);
    }

    [Test]
    public void IsValidNumber_ScaleExceeded_ReturnsFalse()
    {
        var validator = new NumberValidator(17, 2, true);
        Assert.That(validator.IsValidNumber("0.000"), Is.False);
    }

    [Test]
    public void IsValidNumber_NegativeNumber_WhenOnlyPositiveRequired_ReturnsFalse()
    {
        var validator = new NumberValidator(17, 2, true);
        Assert.That(validator.IsValidNumber("-1.23"), Is.False);
    }
}