namespace AttributesMyNUnit;

using System;

/// <summary>
/// Атрибут, которым обозначают методы, которые вызываются перед тестами
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class BeforeClass : Attribute
{
    public BeforeClass()
    {
            
    }
}