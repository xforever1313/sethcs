//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;

namespace Seth.Analyzer.CodeFixes
{
    /// <summary>
    /// Interface so we know which classes are code fixes so we don't have to register
    /// them all in one class like we have to do for the rules.
    /// If we want to turn off a code fix, simply have the class not implement this interface.
    /// </summary>
    public interface ICodeFix
    {
        // ---------------- Properties ----------------
        
        /// <summary>
        ///  What rule this code fix is supposed to fix.
        /// </summary>
        DiagnosticDescriptor Rule { get; }

        // ---------------- Functions ----------------

        /// <summary>
        /// Registers any <see cref="CodeAction"/>s take if <see cref="Rule"/> is found.
        /// </summary>
        Task RegisterCodeFixesAsync( CodeFixContext context, Diagnostic diagnostic );
    }
}
