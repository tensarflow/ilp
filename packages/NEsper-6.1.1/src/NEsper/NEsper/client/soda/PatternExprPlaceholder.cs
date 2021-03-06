///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;

namespace com.espertech.esper.client.soda
{
    /// <summary>
    /// For use in pattern expression as a placeholder to represent its child nodes.
    /// </summary>
    [Serializable]
    public class PatternExprPlaceholder : PatternExprBase
    {
        public override void ToPrecedenceFreeEPL(TextWriter writer, EPStatementFormatter formatter)
        {
            if ((Children == null) || (Children.Count == 0))
            {
                return;
            }
            PatternExpr patternExpr = Children[0];
            if (patternExpr != null)
            {
                patternExpr.ToEPL(writer, Precedence, formatter);
            }
        }

        public override PatternExprPrecedenceEnum Precedence
        {
            get { return PatternExprPrecedenceEnum.MINIMUM; }
        }
    }
}
