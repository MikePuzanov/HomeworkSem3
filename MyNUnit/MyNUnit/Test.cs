using System;

namespace MyNUnit
{
    public class Test : Attribute
    {
        public Type Expected { get; set; }
        
        public string Ignore { get; set; }

        public Test()
        {
            
        }
    }
}