namespace AttributesMyNUnit;

using System;

/// <summary>
/// Атрибут, которым обозначают методы, которые являются тестами
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class Test : Attribute
{
    public Type Expected { get; set; }
        
    public string Ignore { get; set; }

    public Test(Type expected, string ignore = null)
    {
        Expected = expected;
        Ignore = ignore;
    }
}