///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;


namespace com.espertech.esper.client.soda
{
    /// <summary>
    /// Pattern observer expression observes occurances such as timer-at (crontab) and timer-interval.
    /// </summary>
    [Serializable]
    public class PatternObserverExpr
        : EPBaseNamedObject
        , PatternExpr
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public PatternObserverExpr()
        {
        }

        public string TreeObjectName { get; set; }

        /// <summary>Ctor - for use to create a pattern expression tree, without pattern child expression. </summary>
        /// <param name="namespace">is the guard object namespace</param>
        /// <param name="name">is the guard object name</param>
        /// <param name="parameters">is guard object parameters</param>
        public PatternObserverExpr(String @namespace, String name, IList<Expression> parameters)
            : base(@namespace, name, parameters)
        {
        }

        public List<PatternExpr> Children
        {
            get { return new List<PatternExpr>(); }
            set
            {
                // this expression has no child expressions
            }
        }

        public PatternExprPrecedenceEnum Precedence
        {
            get { return PatternExprPrecedenceEnum.ATOM; }
        }

        public void ToEPL(TextWriter writer, PatternExprPrecedenceEnum parentPrecedence, EPStatementFormatter formatter)
        {
            if (Precedence.GetLevel() < parentPrecedence.GetLevel())
            {
                writer.Write("(");
                ToPrecedenceFreeEPL(writer);
                writer.Write(")");
            }
            else
            {
                ToPrecedenceFreeEPL(writer);
            }
        }

        /// <summary>Renders the expressions and all it's child expression, in full tree depth, as a string in language syntax. </summary>
        /// <param name="writer">is the output to use</param>
        public void ToPrecedenceFreeEPL(TextWriter writer)
        {
            base.ToEPL(writer);
        }
    }
}
