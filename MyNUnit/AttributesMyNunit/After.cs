namespace AttributesMyNUnit;

using System;

/// <summary>
/// Атрибут, которым обозначают методы, которые вызываются после каждого тестом
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class After : Attribute
{
    public After()
    {
            
    }
}