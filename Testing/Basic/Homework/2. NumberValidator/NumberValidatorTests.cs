using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true, TestName = "Invalid_Precision_Less_Than_Zero")]
    [TestCase(0, 2, false, TestName = "Invalid_Precision_Is_Zero")]
    [TestCase(3, -1, true, TestName = "Invalid_Scale_Less_Than_Zero")]
    [TestCase(2, 2, true, TestName = "Invalid_Scale_Equals_Precision")]
    [TestCase(2, 3, false, TestName = "Invalid_Scale_More_Than_Precision")]
    public void Constructor_InvalidParameters_ThrowsArgumentException(int precision, int scale, bool onlyPositive)
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale, onlyPositive));
    }

    [TestCase(1, 0, true, TestName = "Valid_Constructor_1_0")]
    [TestCase(17, 2, false, TestName = "Valid_Constructor_17_2")]
    public void Constructor_ValidParameters_DoesNotThrow(int precision, int scale, bool onlyPositive)
    {
        Assert.DoesNotThrow(() => new NumberValidator(precision, scale, onlyPositive));
    }


    // Тест, если пользователь введет вместо точки запятую
    [TestCase(17, 2, true, "0,0", ExpectedResult = true, TestName = "Valid_Positive_With_Comma")]
    // Тест на пустое значение
    [TestCase(17, 2, true, "", ExpectedResult = false, TestName = "Invalid_Empty_String")]
    // Проверка на скрытый +
    [TestCase(3, 2, true, "1.23", ExpectedResult = true, TestName = "Valid_Without_Sign_Like_Precision")]
    // Неверное значение заканчивается на точку
    [TestCase(17, 2, true, "1.", ExpectedResult = false, TestName = "Invalid_EndsWithDot")]
    // Тест на неправильное значение onlyPositive
    [TestCase(3, 2, true, "-1.23", ExpectedResult = false, TestName = "Invalid_Negative")]


    [TestCase(17, 2, false, "-1.23", ExpectedResult = true, TestName = "Valid_Negative")]
    [TestCase(17, 2, true, "0.0", ExpectedResult = true, TestName = "Valid_Positive_With_Dot")]
    [TestCase(17, 2, true, "0", ExpectedResult = true, TestName = "Valid_Integer")]
    [TestCase(4, 2, true, "+1.23", ExpectedResult = true, TestName = "Valid_Plus_Sign_More_Precision")]
    [TestCase(5, 2, true, "123.45", ExpectedResult = true, TestName = "Valid_Number_With_Scale")]
    [TestCase(17, 2, true, null, ExpectedResult = false, TestName = "Invalid_Null")]
    [TestCase(3, 2, true, "00.00", ExpectedResult = false, TestName = "Invalid_Too_Many_Digits")]
    [TestCase(3, 2, true, "-0.00", ExpectedResult = false, TestName = "Invalid_Negative_Too_Long")]
    [TestCase(3, 2, true, "+0.00", ExpectedResult = false, TestName = "Invalid_Positive_Too_Long")]
    [TestCase(3, 2, true, "+1.23", ExpectedResult = false, TestName = "Invalid_Plus_Sign_More_Precision")]
    [TestCase(17, 2, true, "0.000", ExpectedResult = false, TestName = "Invalid_Too_After_Dot")]
    [TestCase(17, 2, true, "a.sd", ExpectedResult = false, TestName = "Invalid_Not_Number")]
    public bool IsValidNumber_ReturnsExpectedResult(int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        return validator.IsValidNumber(value);
    }
}