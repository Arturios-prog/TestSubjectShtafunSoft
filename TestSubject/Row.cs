using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSubject
{
    public class Row
    {
        public List<Object> objects { get; set; } = new List<object>();

        public Row Add(object o)
        {
            objects.Add(o);
            return this;
        }

    }
}
