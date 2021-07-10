//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Seth.Analyzer.Rules
{
    public abstract class BaseRule
    {
        // ---------------- Constructor ----------------

        public BaseRule()
        {
            this.Rule = new DiagnosticDescriptor(
                this.GetType().Name,
                this.Title,
                this.MessageFormat,
                this.RuleCategory.ToString(),
                this.Serverity,
                isEnabledByDefault: this.IsEnabledByDefault,
                description: this.Description
            );
        }

        // ---------------- Properties ----------------

        public DiagnosticDescriptor Rule { get; private set; }

        protected abstract LocalizableString Title { get; }

        protected abstract LocalizableString MessageFormat { get; }

        protected abstract LocalizableString Description { get; }

        protected abstract Category RuleCategory { get; }

        protected abstract DiagnosticSeverity Serverity { get; }

        protected bool IsEnabledByDefault { get; }

        // ---------------- Functions ----------------

        public abstract void Init( AnalysisContext context );

        // ---------------- Enums ----------------

        protected enum Category
        {
            Warning,
            Error
        }
    }
}
