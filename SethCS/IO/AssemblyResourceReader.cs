//
//          Copyright Seth Hendrick 2016-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.IO;
using System.Reflection;

namespace SethCS.IO
{
    public static class AssemblyResourceReader
    {
        /// <summary>
        /// Reads a resource from the given assembly.
        /// </summary>
        public static string ReadStringResource( Assembly assem, string resourceName )
        {
            using( Stream stream = assem.GetManifestResourceStream( resourceName ) )
            {
                if( stream == null )
                {
                    throw new InvalidOperationException( $"Could not open stream for {resourceName}" );
                }

                using( StreamReader reader = new StreamReader( stream ) )
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
