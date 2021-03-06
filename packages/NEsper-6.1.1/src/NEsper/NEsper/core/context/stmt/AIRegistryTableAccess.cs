///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

using com.espertech.esper.compat;
using com.espertech.esper.compat.collections;
using com.espertech.esper.epl.expression.table;

namespace com.espertech.esper.core.context.stmt
{
    public interface AIRegistryTableAccess : ExprTableAccessEvalStrategy
    {
        void AssignService(int num, ExprTableAccessEvalStrategy value);
        void DeassignService(int num);
        int AgentInstanceCount { get; }
    }
}
