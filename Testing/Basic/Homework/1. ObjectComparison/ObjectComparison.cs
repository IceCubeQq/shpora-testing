using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        actualTsar.Should().BeEquivalentTo(expectedTsar, options => options
            .Excluding(p => p.Id)
            .Excluding(p => p.Parent.Id));
        // Благодаря этому методу мы  сокращаем количество строк до одной, и при этом будут учитываться сразу все поля объекта,
        // даже если добавим новые
        // Также лучше прописать IgnoringCyclicReferences, чтобы избежать циклов в сравнениях
        // Из интересного, если мы не хотим сравнивать сразу по всем полям, мы может в Excluding задать поля, которые не нужно сравнивать (например, id, который всегда уникален))

        // Нам не нужно создавать свой метод для сравнения, в котором можно ошибиться.
        // Там нужно вручную добавлять новые поля, что расходует время, а также это можно просто забыть сделать, из-за чего логика сравнения
        // сломается.
        // Еще, как я понял, BeEquivalentTo имеет подробные сообщения об ошибках, где можно сразу увидеть, какое поле не совпало. Вручную это опять же было бы
        // сложнее поддерживать
        // Ну и в целом наш подход занимает намного меньше места и более читаем.

        // Перепишите код на использование Fluent Assertions.
        // ClassicAssert.AreEqual(actualTsar.Name, expectedTsar.Name);
        // ClassicAssert.AreEqual(actualTsar.Age, expectedTsar.Age);
        // ClassicAssert.AreEqual(actualTsar.Height, expectedTsar.Height);
        // ClassicAssert.AreEqual(actualTsar.Weight, expectedTsar.Weight);

        // ClassicAssert.AreEqual(expectedTsar.Parent!.Name, actualTsar.Parent!.Name);
        // ClassicAssert.AreEqual(expectedTsar.Parent.Age, actualTsar.Parent.Age);
        // ClassicAssert.AreEqual(expectedTsar.Parent.Height, actualTsar.Parent.Height);
        // ClassicAssert.AreEqual(expectedTsar.Parent.Parent, actualTsar.Parent.Parent);
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
    }

    private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
}
