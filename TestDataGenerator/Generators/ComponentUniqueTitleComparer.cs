using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataGenerator.Models;

namespace TestDataGenerator.Generators
{
    public class ComponentUniqueTitleComparer : IEqualityComparer<Component>
    {
        public bool Equals(Component? x, Component? y)
        {
            return x is not null && y is not null && x.TitleUa == y.TitleUa;
        }

        public int GetHashCode([DisallowNull] Component obj)
        {
            return obj.TitleUa.GetHashCode();
        }
    }
}
