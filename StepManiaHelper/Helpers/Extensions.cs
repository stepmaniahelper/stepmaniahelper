using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    public static class StepManiaHelperExtensions
    {
        public static void AddIfUnique<T>(this List<T> list, T item)
        {
            if (list.Contains(item) == false)
            {
                list.Add(item);
            }
        }
    }
}
