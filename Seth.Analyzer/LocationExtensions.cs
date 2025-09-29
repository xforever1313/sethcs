//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using Microsoft.CodeAnalysis;

namespace Seth.Analyzer
{
    public static class LocationExtensions
    {
        /// <summary>
        /// Tries to get the <see cref="SyntaxNode"/> from the given <see cref="Location"/>.
        /// Returns null if there is no node.
        /// </summary>
        public static SyntaxNode GetSyntaxNode( this Location location )
        {
            if( location == null )
            {
                return null;
            }

            SyntaxTree tree = location.SourceTree;
            if( tree == null )
            {
                return null;
            }

            if( tree.TryGetRoot( out SyntaxNode root ) && ( root != null ) )
            {
                return root.FindNode( location.SourceSpan );
            }

            return null;
        }
    }
}
