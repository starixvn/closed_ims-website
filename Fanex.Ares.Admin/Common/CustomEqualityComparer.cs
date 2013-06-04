// -----------------------------------------------------------------------
// <copyright file="CustomEqualityComparer.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.05.31">Created</change>
// </history>


namespace Fanex.Ares.Admin.Common
{
    using System.Diagnostics.CodeAnalysis;
    using System.Collections.Generic;
    using System;

    public class CustomEqualityComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y)
        {
            bool result = true;
            foreach (var propertyInfo in x.GetType().GetProperties())
            {
                if (!(propertyInfo.GetValue(x, null).Equals(propertyInfo.GetValue(y, null))))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public int GetHashCode(T obj)
        {
            // Ignore HashCode focus on Properties
            return 0;
        }
    }
}
