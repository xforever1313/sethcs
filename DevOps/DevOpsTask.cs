//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using Cake.Common.Diagnostics;
using Cake.Frosting;

namespace DevOps
{
    public abstract class DevOpsTask : FrostingTask<BuildContext>
    {
        // ---------------- Functions ----------------

        public override void OnError( Exception exception, BuildContext context )
        {
            base.OnError( exception, context );
            context.Error( exception.ToString() );
        }
    }
}
