namespace AttributesMyNUnit;

using System;

/// <summary>
/// Атрибут, которым обозначают методы, которые вызываются перед каждым тестом
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class Before : Attribute
{
    public Before()
    {
            
    }
}