///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

using com.espertech.esper.epl.expression.core;

namespace com.espertech.esper.epl.join.table
{
    /// <summary>
    /// Table of events allowing add and remove. Lookup in table is coordinated
    /// through the underlying implementation.
    /// </summary>
    public interface EventTableFactory
    {
        Type EventTableType { get; }

        EventTable[] MakeEventTables(EventTableFactoryTableIdent tableIdent, ExprEvaluatorContext exprEvaluatorContext);
    
        string ToQueryPlan();
    }
} // end of namespace
