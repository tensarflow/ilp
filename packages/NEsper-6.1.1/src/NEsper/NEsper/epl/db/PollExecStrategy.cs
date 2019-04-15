///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;

using com.espertech.esper.client;
using com.espertech.esper.epl.expression.core;

namespace com.espertech.esper.epl.db
{
    /// <summary>
    /// Interface for polling data from a data source such as a relational database.
    /// Lifecycle methods are for managing connection resources.
    /// </summary>
    public interface PollExecStrategy : IDisposable
    {
        /// <summary> Start the poll, called before any poll operation.</summary>
        void Start();

        /// <summary>
        /// Poll events using the keys provided.
        /// </summary>
        /// <param name="lookupValues">is keys for exeuting a query or such</param>
        /// <param name="exprEvaluatorContext">The expression evaluator context.</param>
        /// <returns>
        /// a list of events for the keys
        /// </returns>

        IList<EventBean> Poll(Object[] lookupValues, ExprEvaluatorContext exprEvaluatorContext);

        /// <summary> Indicate we are done polling and can release resources.</summary>
        void Done();
    }
}
