///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

using com.espertech.esper.epl.expression.core;

namespace com.espertech.esper.epl.expression.visitor
{
    public class ExprNodeStreamUseCollectVisitor : ExprNodeVisitor
    {
        private readonly IList<ExprStreamRefNode> referenced = new List<ExprStreamRefNode>();

        public bool IsVisit(ExprNode exprNode)
        {
            return true;
        }

        public IList<ExprStreamRefNode> Referenced
        {
            get { return referenced; }
        }

        public void Visit(ExprNode exprNode)
        {
            if (!(exprNode is ExprStreamRefNode))
            {
                return;
            }
            referenced.Add((ExprStreamRefNode) exprNode);
        }
    }
}
