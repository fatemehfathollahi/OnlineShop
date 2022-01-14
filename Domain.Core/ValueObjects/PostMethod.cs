using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.ValueObjects
{
    public class PostMethod
    {
        public enum PostType
        {
            Normal,
            Express
        }

        public readonly PostType PType;

        public PostMethod(PostType pType)
        {
            this.PType = pType;
        }
    }
}
