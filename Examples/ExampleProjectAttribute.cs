using System;

namespace Examples
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExampleProjectAttribute
        : Attribute
    {
        public readonly string Caption;

        public ExampleProjectAttribute(string caption)
        {
            Caption = caption;
        }
    }
}