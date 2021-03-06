///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

using com.espertech.esper.collection;
using com.espertech.esper.epl.expression.core;

namespace com.espertech.esper.epl.expression.visitor
{
    [Serializable]
    public class ExprNodeGroupingVisitorWParent : ExprNodeVisitorWithParent
    {
        /// <summary>Ctor. </summary>
        public ExprNodeGroupingVisitorWParent()
        {
            GroupingIdNodes = new List<Pair<ExprNode, ExprGroupingIdNode>>(2);
            GroupingNodes = new List<Pair<ExprNode, ExprGroupingNode>>(2);
        }
    
        public bool IsVisit(ExprNode exprNode)
        {
            return true;
        }

        public IList<Pair<ExprNode, ExprGroupingIdNode>> GroupingIdNodes { get; private set; }

        public IList<Pair<ExprNode, ExprGroupingNode>> GroupingNodes { get; private set; }

        public void Visit(ExprNode exprNode, ExprNode parentExprNode)
        {
            if (exprNode is ExprGroupingIdNode)
            {
                GroupingIdNodes.Add(
                    new Pair<ExprNode, ExprGroupingIdNode>(parentExprNode, (ExprGroupingIdNode) exprNode));
            }
            if (exprNode is ExprGroupingNode)
            {
                GroupingNodes.Add(new Pair<ExprNode, ExprGroupingNode>(parentExprNode, (ExprGroupingNode) exprNode));
            }
        }
    }
}
