namespace AttributesMyNUnit;

using System;

/// <summary>
/// Атрибут, которым обозначают методы, которые вызываются после каждого теста
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class After : Attribute
{
}