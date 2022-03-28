namespace MyNUnit;

using System;
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
    /// <summary>
    /// запуск тестов из .dll 
    /// </summary>
    /// <param name="path">путь до .dll</param>
    /// <returns>Кортеж из (названия теста, результат о нем)</returns>
    public (string, string)[] RunTests(string path)
    {
        var dlls = Directory.GetFiles(path, "*.dll");
        var tasks = new Task<List<(string, string)>>[dlls.Length];
        var message = new List<(string, string)>();
        Parallel.ForEach(dlls,dll => DoTestFromDll(dll, message));
        return message.ToArray();
    }
        
    private void DoTestFromDll(string dllPath, List<(string, string)> message)
    {
        var dll = Assembly.LoadFrom(dllPath);
        var classes = dll.ExportedTypes.Where(t => t.IsClass);
        Parallel.ForEach(classes, c => DoWorkInClass(c, message));
    }

    private void DoWorkInClass(Type classFromDll, List<(string, string)> messagesForUser)
    {
        var after = new List<MethodInfo>();
        var before = new List<MethodInfo>();
        var beforeClass = new List<MethodInfo>();
        var afterClass = new List<MethodInfo>();
        var tests = new List<MethodInfo>();
        var methods = classFromDll.GetMethods();
        Parallel.ForEach(methods, method => GetAttributesAndDoBeforeAndAfterClass(method, after, before, beforeClass, afterClass, tests, messagesForUser));
        Parallel.ForEach(beforeClass, method => RunMethodsWithAttributes(method, messagesForUser, null));
            
        if (tests.Count < 1)
        {
            return;
        }
        messagesForUser.Add(($"Проверка тестов из {classFromDll.Name}", ""));

        Parallel.ForEach(tests, test => DoTest(test, messagesForUser, before, after, classFromDll));
        Parallel.ForEach(afterClass, method => RunMethodsWithAttributes(method, messagesForUser, null));
    }

    private void DoTest(MethodInfo test, List<(string, string)> messagesFromCurrentTest, List<MethodInfo> before,
        List<MethodInfo> after, Type classDll)
    {
        var attribute = (Test)Attribute.GetCustomAttribute(test, typeof(Test));
        if (attribute.Ignore != null)
        {
            return;
        }
        object classInstance = Activator.CreateInstance(classDll);    
        Parallel.ForEach(before, method => RunMethodsWithAttributes(method, messagesFromCurrentTest, classInstance));
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
                message = ($"Тест {test.Name} не пройден: ожидалось исключения типа {attribute.Expected}, возникло {exception.InnerException.GetType()}", $"Время: {watch.ElapsedMilliseconds} ms");
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
                    message = ($"Тест {test.Name} не пройден: ожидалось исключения типа {attribute.Expected}", $"Время: {watch.ElapsedMilliseconds} ms");
                }
            }
        }
        messagesFromCurrentTest.Add(message);
            
        Parallel.ForEach(after, method => RunMethodsWithAttributes(method, messagesFromCurrentTest, classInstance));
    }

    private void AddMethod(Attribute[] attributes, MethodInfo method, List<MethodInfo> list)
    {
        if (attributes.Length != 0)
        {
            list.Add(method);
        }
    }

    private int CheckAttributesInMethod(Attribute[] attribute)
    {
        if (attribute.Length > 0)
        {
            return 1;
        }

        return 0;
    }
        
    private void RunMethodsWithAttributes(MethodInfo method, List<(string, string)> messagesForUser, object ClassIntstase)
    {
        try
        {
            method.Invoke(ClassIntstase, null);
        }
        catch (Exception e)
        {
            messagesForUser.Add(($"В методе {method.Name} возникло исключение: {e.InnerException.GetType()}", ""));
        }
    }

    private void GetAttributesAndDoBeforeAndAfterClass(MethodInfo method, List<MethodInfo> after,
        List<MethodInfo> before, List<MethodInfo> beforeClass, List<MethodInfo> afterClass, List<MethodInfo> tests,
        List<(string, string)> messagesForUser)
    {
        var attributesTest = Attribute.GetCustomAttributes(method).Where(t => t.GetType() == typeof(Test)).ToArray();
        var attributesBefore = Attribute.GetCustomAttributes(method).Where(t => t.GetType() == typeof(Before)).ToArray();
        var attributesAfter = Attribute.GetCustomAttributes(method).Where(t => t.GetType() == typeof(After)).ToArray();
        var attributesBeforeClass = Attribute.GetCustomAttributes(method).Where(t => t.GetType() == typeof(BeforeClass)).ToArray();
        var attributesAfterClass = Attribute.GetCustomAttributes(method).Where(t => t.GetType() == typeof(AfterClass)).ToArray();
        var count = 0;
        count += CheckAttributesInMethod(attributesTest);
        count += CheckAttributesInMethod(attributesAfter);
        count += CheckAttributesInMethod(attributesBefore);
        count += CheckAttributesInMethod(attributesAfterClass);
        count += CheckAttributesInMethod(attributesBeforeClass);
        if (count > 1)
        {
            messagesForUser.Add(($"Метод {method.Name} имеет два несовместимых атрибута", ""));
            return;
        }
        AddMethod(attributesTest, method, tests);
        AddMethod(attributesBefore, method, before);
        AddMethod(attributesAfter, method, after);
        if (attributesBeforeClass.Length != 0)
        {
            if (method.IsStatic)
            { 
                beforeClass.Add(method);
            }
            else
            {
                messagesForUser.Add(($"Метод {method.Name} содержит атрибут BeforeClass, но не является статическим", ""));
            }
        }
        if (attributesAfterClass.Length != 0)
        {
            if (method.IsStatic)
            {
                afterClass.Add(method);
            }
            else
            {
                messagesForUser.Add(($"Метод {method.Name} содержит атрибут AfterClass, но не является статическим", ""));
            }
        }
    }
}