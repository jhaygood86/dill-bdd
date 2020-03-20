using System;
using System.Collections.Generic;
using System.Text;

namespace Dill.Attributes
{
    public abstract class DillStepAttribute : Attribute
    {
        public string Value { get; set; }

        protected DillStepAttribute(string value)
        {
            Value = value;
        }
    }
}
