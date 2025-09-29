//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Reflection;

namespace SethCS.Basic
{
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
    public class EqualsIgnoreAttribute : Attribute
    {
        // ---------------- Constructor ----------------

        public EqualsIgnoreAttribute() :
            this( true )
        {
        }

        public EqualsIgnoreAttribute( bool shouldIgnore )
        {
            this.ShouldIgnore = shouldIgnore;
        }

        // ---------------- Properties ----------------

        public bool ShouldIgnore { get; set; }
    }

    public static class EqualsHelpers
    {
        public static bool ArePropertiesEqual<T>( T left, T right )
        {
            if( ReferenceEquals( left, right ) )
            {
                return true;
            }
            else if( ReferenceEquals( left, null ) )
            {
                return ReferenceEquals( null, right );
            }
            else if( ReferenceEquals( null, right ) )
            {
                return ReferenceEquals( left, null );
            }

            bool areEqual = true;
            foreach( PropertyInfo property in typeof( T ).GetProperties() )
            {
                EqualsIgnoreAttribute ignore = property.GetCustomAttribute<EqualsIgnoreAttribute>();
                if( ( ignore != null ) && ignore.ShouldIgnore )
                {
                    continue;
                }

                object leftValue = property.GetValue( left );
                object rightValue = property.GetValue( right );
                if( ReferenceEquals( leftValue, null ) )
                {
                    areEqual &= ReferenceEquals( null, rightValue );
                }
                else
                {
                    areEqual &= leftValue.Equals( rightValue );
                }
            }

            return areEqual;
        }

        public static bool OperatorDoubleEqualsHelper<T>( T left, T right )
        {
            if( ReferenceEquals( left, right ) )
            {
                return true;
            }
            else if( ReferenceEquals( left, null ) )
            {
                return ReferenceEquals( null, right );
            }
            else if( ReferenceEquals( null, right ) )
            {
                return ReferenceEquals( left, null );
            }

            return left.Equals( right );
        }

        public static bool OperatorNotEqualsHelper<T>( T left, T right )
        {
            return OperatorDoubleEqualsHelper( left, right ) == false;
        }
    }
}
