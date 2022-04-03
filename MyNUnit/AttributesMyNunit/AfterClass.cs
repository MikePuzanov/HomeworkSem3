namespace AttributesMyNUnit;

using System;

/// <summary>
/// Атрибут, которым обозначают методы, которые вызываются после тестов
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class AfterClass : Attribute
{
}