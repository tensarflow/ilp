///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

namespace com.espertech.esper.epl.expression.core
{
    /// <summary>
    /// Represents an stream property identifier in a filter expressiun tree.
    /// </summary>
    public interface ExprIdentNode 
        : ExprNode
        , ExprFilterOptimizableNode
        , ExprStreamRefNode
    {
        string UnresolvedPropertyName { get; }
        string FullUnresolvedName { get; }
        int StreamId { get; }
        string ResolvedPropertyNameRoot { get; }
        string ResolvedPropertyName { get; }
        string StreamOrPropertyName { get; set; }
        string ResolvedStreamName { get; }
        ExprIdentNodeEvaluator ExprEvaluatorIdent { get; }
    }
}
