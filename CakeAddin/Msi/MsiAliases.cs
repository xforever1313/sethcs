//
//          Copyright Seth Hendrick 2019.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace SethCS.CakeAddin.Msi
{
    public static class MsiAliases
    {
        [CakeMethodAlias]
        [CakeAliasCategory( "Msi" )]
        [CakeAliasCategory( "SethCS.Msi" )]
        public static string GetMsiVersion( this ICakeContext context, FilePath path )
        {
            MsiReader reader = new MsiReader();
            return reader.ReadVersion( path.ToString() );
        }
    }
}
