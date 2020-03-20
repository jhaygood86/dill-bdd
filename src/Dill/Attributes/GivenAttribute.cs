using System;
using System.Collections.Generic;
using System.Text;
using Dill.Attributes;

namespace Dill.Attributes
{
    public class GivenAttribute : DillStepAttribute
    {
        public GivenAttribute(string value) : base(value)
        {
            
        }
    }
}
