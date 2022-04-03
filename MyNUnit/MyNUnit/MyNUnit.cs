namespace MyNUnit;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AttributesMyNUnit;

/// <summary>
/// Класс MyNUnit для запуска и проверки тестов
/// </summary>
public class MyNUnit
{
    private ConcurrentBag<(string, string)> messages;

    /// <summary>
    /// запуск тестов из .dll 
    /// </summary>
    /// <param name="path">путь до .dll</param>
    /// <returns>Кортеж из (названия теста, результат о нем)</returns>
    public (string, string)[] RunTests(string path)
    {
        var dlls = Directory.GetFiles(path, "*.dll");
        var tasks = new Task<List<(string, string)>>[dlls.Length];
        messages = new();
        Parallel.ForEach(dlls, dll => DoTestFromDll(dll));
        return messages.ToArray();
    }

    private void DoTestFromDll(string dllPath)
    {
        var dll = Assembly.LoadFrom(dllPath);
        var classes = dll.ExportedTypes.Where(t => t.IsClass);
        Parallel.ForEach(classes, c => DoWorkInClass(c));
    }

    private void DoWorkInClass(Type classFromDll)
    {
        var testAttributes = new TestAttributes();
        var methods = classFromDll.GetMethods();
        Parallel.ForEach(methods, method => GetAttributesAndDoBeforeAndAfterClass(method, testAttributes));
        RunMethodsWithAttributes(testAttributes.BeforeClass, null);

        if (testAttributes.Tests.Count < 1)
        {
            return;
        }
        messages.Add(($"Проверка тестов из {classFromDll.Name}", ""));

        Parallel.ForEach(testAttributes.Tests, test => DoTest(test, testAttributes, classFromDll));
        RunMethodsWithAttributes(testAttributes.AfterClass, null);
    }

    private void DoTest(MethodInfo test, TestAttributes testAttributes, Type classDll)
    {
        var attribute = (Test)Attribute.GetCustomAttribute(test, typeof(Test));
        if (attribute.Ignore != null)
        {
            messages.Add(($"Тест {test.Name} был игнорирован", ""));
            return;
        }
        object classInstance = Activator.CreateInstance(classDll);
        RunMethodsWithAttributes(testAttributes.Before, classInstance);
        var message = ("", "");
        var watch = new Stopwatch();
        watch.Start();
        try
        {
            test.Invoke(classInstance, null);
        }
        catch (Exception exception)
        {
            watch.Stop();
            if (attribute.Expected == null)
            {
                message = ($"Тест {test.Name} не пройден: возникло исключение {exception.InnerException.GetType()}", $"Время: {watch.ElapsedMilliseconds} ms");
            }
            else if (exception.InnerException.GetType() != attribute.Expected)
            {
                message = ($"Тест {test.Name} не пройден: ожидалось исключение типа {attribute.Expected}, возникло {exception.InnerException.GetType()}", $"Время: {watch.ElapsedMilliseconds} ms");
            }
            else
            {
                message = ($"Тест {test.Name} пройден успешно", $"Время: {watch.ElapsedMilliseconds} ms");
            }
        }
        finally
        {
            watch.Stop();
            if (message.Item1 == "")
            {
                if (attribute.Expected == null)
                {
                    message = ($"Тест {test.Name} пройден успешно", $"Время: {watch.ElapsedMilliseconds} ms");
                }
                else
                {
                    message = ($"Тест {test.Name} не пройден: ожидалось исключение типа {attribute.Expected}", $"Время: {watch.ElapsedMilliseconds} ms");
                }
            }
        }
        messages.Add(message);

        RunMethodsWithAttributes(testAttributes.After, classInstance);
    }

    private void AddMethod(Attribute[] attributes, MethodInfo method, List<MethodInfo> list)
    {
        if (attributes.Length != 0)
        {
            list.Add(method);
        }
    }

    private void RunMethodsWithAttributes(List<MethodInfo> methods, object classIntstase)
    {
        foreach (MethodInfo method in methods)
        {
            try
            {
                method.Invoke(classIntstase, null);
            }
            catch (Exception e)
            {
                messages.Add(($"В методе {method.Name} возникло исключение: {e.InnerException.GetType()}", ""));
            }
        }     
    }

    private Attribute[][] GetCountAttributes(MethodInfo method)
    {
        var types = new Type[] { typeof(Before), typeof(BeforeClass), typeof(Test), typeof(AfterClass), typeof(After)};
        var typesCount = new Attribute[5][];
        for (int i = 0; i < types.Length; i++)
        {
            typesCount[i] = Attribute.GetCustomAttributes(method).Where(t => t.GetType() == types[i]).ToArray();
        }
        return typesCount;
    }

    private int GetSumOfAttributesInMethod(Attribute[][] array)
    {
        var count = 0;
        for (int i = 0;i < array.Length;i++)
        {
            count += array[i].Length;
        }
        return count;
    }

    private void GetAttributesAndDoBeforeAndAfterClass(MethodInfo method, TestAttributes testAttributes)
    { 
        var countOfAttributes = GetCountAttributes(method);
        if (GetSumOfAttributesInMethod(countOfAttributes) > 1)
        {
            messages.Add(($"Метод {method.Name} имеет два несовместимых атрибута", ""));
            return;
        }
        AddMethod(countOfAttributes[0], method, testAttributes.Before);
        AddMethod(countOfAttributes[2], method, testAttributes.Tests);
        AddMethod(countOfAttributes[4], method, testAttributes.After);
        if (countOfAttributes[1].Length != 0)
        {
            if (method.IsStatic)
            {
                testAttributes.BeforeClass.Add(method);
            }
            else
            {
                messages.Add(($"Метод {method.Name} содержит атрибут BeforeClass, но не является статическим", ""));
            }
        }
        if (countOfAttributes[3].Length != 0)
        {
            if (method.IsStatic)
            {
                testAttributes.AfterClass.Add(method);
            }
            else
            {
                messages.Add(($"Метод {method.Name} содержит атрибут AfterClass, но не является статическим", ""));
            }
        }
    }
    private class TestAttributes
    {
        public TestAttributes()
        {
            After = new();
            Before = new();
            BeforeClass = new();
            AfterClass = new();
            Tests = new();
        }

        public List<MethodInfo> After { get; set; }
        public List<MethodInfo> Before { get; set; }
        public List<MethodInfo> BeforeClass { get; set; }
        public List<MethodInfo> AfterClass { get; set; }
        public List<MethodInfo> Tests { get; set; }
    }
}