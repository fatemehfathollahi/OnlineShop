using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.ValueObjects
{
    public class PackagingMethod
    {
        public enum PackagingType
        {
            Normal,
            Breakable
        }

        public readonly PackagingType PackageType;

        public PackagingMethod(PackagingType PackageType)
        {
            this.PackageType = PackageType;
        }

    }
}
