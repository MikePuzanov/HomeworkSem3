using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MyNUnit
{
    public class MyNUnit
    {
        public (string, string)[] RunTests(string path)
        {
            var dll = Directory.GetFiles(path, "*.dll");
            var tasks = new Task<List<(string, string)>>[dll.Length];
            for (int i = 0; i < dll.Length; i++)
            {
                var localI = i;
                tasks[i] = Task.Run(() => DoTestFromDll(dll[localI]));
            }
            var infoAboutTests = new List<(string, string)>();
            for (int i = 0; i < tasks.Length; i++)
            {
                infoAboutTests = infoAboutTests.Union(tasks[i].Result).ToList();
            }

            if (infoAboutTests.Count == 0)
            {
                return new[] { ("По заданному пути не было найдено тестов", "") };
            }
            return infoAboutTests.ToArray();
        }

        private List<(string, string)> DoTestFromDll(string dllPath)
        {
            var messages = new List<(string, string)>();
            var dll = Assembly.LoadFrom(dllPath);
            var classes = dll.ExportedTypes.Where(t => t.IsClass);
            foreach (Type t in classes)
            {
                var infoAboutTests = DoWorkInClass(t);
                messages = messages.Union(infoAboutTests).ToList();
            }
            return messages;
        }

        private void RunMethodsWithAttributes(MethodInfo[] methodInfos, List<(string, string)> messagesForUser, object ClassIntstase)
        {
            foreach (var method in methodInfos)
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

        private List<(string, string)> DoWorkInClass(Type classDll)
        {
            var after = new List<MethodInfo>();
            var before = new List<MethodInfo>();
            var beforeClass = new List<MethodInfo>();
            var afterClass = new List<MethodInfo>();
            var tests = new List<MethodInfo>();
            var messagesForUser = new List<(string, string)>();
            var methods = classDll.GetMethods();
            foreach (var method in methods)
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
                    continue;
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

            if (tests.Count < 1)
            {
                return messagesForUser;
            }
            
            messagesForUser.Add(($"Проверка тестов из {classDll.Name}", ""));
            
            RunMethodsWithAttributes(beforeClass.ToArray(), messagesForUser, null);
            var tasks = new Task<List<(string, string)>>[tests.Count];
            for (int i = 0; i < tests.Count; i++)
            {   
                var test = tests[i];
                var user = messagesForUser;
                tasks[i] = Task.Run(() =>
                {
                    var messagesFromCurrentTest = new List<(string, string)>();
                    var attribute = (Test)Attribute.GetCustomAttribute(test, typeof(Test));
                    if (attribute.Ignore != null)
                    {
                        return messagesFromCurrentTest;
                    }
                    object classInstance = Activator.CreateInstance(classDll);    
                    RunMethodsWithAttributes(before.ToArray(), messagesFromCurrentTest, classInstance);
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
                    user.Add(message);
                    RunMethodsWithAttributes(after.ToArray(), messagesFromCurrentTest, classInstance);
                    return messagesFromCurrentTest;
                });
            }
            RunMethodsWithAttributes(afterClass.ToArray(), messagesForUser, null);
            foreach (var task in tasks)
            {
                messagesForUser = messagesForUser.Union(task.Result).ToList();
            }
            return messagesForUser;
        }
    }
}