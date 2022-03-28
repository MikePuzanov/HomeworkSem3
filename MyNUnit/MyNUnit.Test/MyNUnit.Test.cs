namespace MyNUnit.Test;

using System.Linq;
using NUnit.Framework;

public class Tests
{
    private readonly MyNUnit myNUnit = new MyNUnit();
    private readonly string[] answer = new string[]
    {
        "Проверка тестов из Test1",
        "Метод NonStaticBeforeClass содержит атрибут BeforeClass, но не является статическим",
        "Метод TestWithIncompatibleAttributes имеет два несовместимых атрибута",
        "В методе ExceptionInAfterClass возникло исключение: System.AggregateException",
        "Тест TestWithExpectedException пройден успешно",
        "Тест TestWithoutExpected пройден успешно",
        "Тест TestBefore пройден успешно",
        "Тест ExceptionExpectedButWasNull не пройден: ожидалось исключения типа System.ArgumentException",
        "Тест OneExceptionExpectedButWasAnother не пройден: ожидалось исключения типа System.ArgumentException, возникло System.AggregateException",
        "Тест NullExpectedButThrowException не пройден: возникло исключение System.ArgumentException"
    };
        
    [NUnit.Framework.Test]
    public void TestForMessages()
    {
        var result = myNUnit.RunTests("../../../../ForTests/bin/Debug/net6.0/");
        foreach (var m in result)
        {
            Assert.IsTrue(answer.Contains(m.Item1));
        }
    }
}