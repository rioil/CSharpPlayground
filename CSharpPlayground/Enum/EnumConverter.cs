using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlayground.Enum
{
    internal class EnumConverter<T> where T : System.Enum
    {
        public T GetValue(string name)
        {
            return default;
        }
    }
}
