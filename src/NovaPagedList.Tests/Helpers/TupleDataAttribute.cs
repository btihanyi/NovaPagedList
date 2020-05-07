using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;

namespace NovaPagedList.Tests
{
    public class TupleDataAttribute : MemberDataAttributeBase
    {
        public TupleDataAttribute(string memberName, params object[] parameters)
            : base(memberName, parameters)
        {
        }

        protected override object[] ConvertDataItem(MethodInfo testMethod, object item)
        {
            if (item == null)
            {
                return null;
            }

            if (item is ITuple tuple)
            {
                object[] data = new object[tuple.Length];
                for (int i = 0; i < tuple.Length; i++)
                {
                    data[i] = tuple[i];
                }
                return data;
            }
            else
            {
                throw new ArgumentException($"The {MemberName} on {MemberType ?? testMethod.DeclaringType} yielded an item that is not a Tuple.");
            }
        }
    }
}
