using System;

namespace MyNUnit
{
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
}