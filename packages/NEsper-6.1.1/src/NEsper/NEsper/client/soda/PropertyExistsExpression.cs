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
	/// Property-exists checks if a dynamic property exists.
	/// </summary>
    [Serializable]
    public class PropertyExistsExpression : ExpressionBase
	{
	    /// <summary>
	    /// Ctor - for use to create an expression tree, without child expression.
	    /// </summary>
	    public PropertyExistsExpression()
	    {
	    }

	    /// <summary>Ctor.</summary>
	    /// <param name="propertyName">is the name of the property to check existence</param>
	    public PropertyExistsExpression(String propertyName)
	    {
	        Children.Add(Expressions.GetPropExpr(propertyName));
	    }

	    public override ExpressionPrecedenceEnum Precedence
	    {
	        get { return ExpressionPrecedenceEnum.UNARY; }
	    }

	    /// <summary>Renders the clause in textual representation.</summary>
	    /// <param name="writer">to output to</param>
        public override void ToPrecedenceFreeEPL(TextWriter writer)
	    {
	        writer.Write("exists(");
            Children[0].ToEPL(writer, ExpressionPrecedenceEnum.MINIMUM);
	        writer.Write(")");
	    }
	}
} // End of namespace
