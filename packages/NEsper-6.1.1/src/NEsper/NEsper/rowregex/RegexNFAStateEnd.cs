///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

using com.espertech.esper.client;
using com.espertech.esper.compat;
using com.espertech.esper.compat.collections;
using com.espertech.esper.epl.expression.core;
using com.espertech.esper.epl.expression;

namespace com.espertech.esper.rowregex
{
    /// <summary>
    /// End state in the regex NFA states.
    /// </summary>
    public class RegexNFAStateEnd
        : RegexNFAStateBase
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public RegexNFAStateEnd()
            : base("endstate", null, -1, false, null)
        {
        }
    
        public override bool Matches(EventBean[] eventsPerStream, ExprEvaluatorContext exprEvaluatorContext)
        {
            throw new UnsupportedOperationException();
        }
    
        public IList<RegexNFAState> GetNextStates()
        {
            return Collections.GetEmptyList<RegexNFAState>();
        }

        public override bool IsExprRequiresMultimatchState
        {
            get { throw new UnsupportedOperationException(); }
        }
    }
}
