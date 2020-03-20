using System;
using System.Collections.Generic;
using System.Text;

namespace Dill.Attributes
{
    public class ThenAttribute : DillStepAttribute
    {
        public ThenAttribute(string value) : base(value)
        {

        }
    }
}
