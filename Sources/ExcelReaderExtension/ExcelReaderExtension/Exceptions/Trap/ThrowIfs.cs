using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelReaderExtension.Exceptions.Trap
{
    public static class ThrowIfs
    {
        public static void NullOrEmpty<T>(List<T> sources, string name)
        {
            if (sources == null || sources.Any() == false)
            {
                var message = $"{name} must not null or empty";
                throw new ArgumentException(message);
            }
        }
    }
}