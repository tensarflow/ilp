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
    /// In-expresson checks that a value is in (or not in) a set of values, equivalent to the syntax "color in ('red', 'blue')".
    /// </summary>
    [Serializable]
    public class InExpression : ExpressionBase
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public InExpression()
        {
        }
    
        /// <summary>
        /// Ctor - for use to create an expression tree, without child expression.
        /// <para />Use add methods to add child expressions to acts upon.
        /// </summary>
        /// <param name="isNotIn">true for the not-in expression, false for the in-expression</param>
        public InExpression(bool isNotIn)
        {
            IsNotIn = isNotIn;
        }
    
        /// <summary>
        /// Ctor - for use to create an expression tree, without child expression.
        /// <para />Use add methods to add child expressions to acts upon.
        /// </summary>
        /// <param name="value">an expression that provides the value to search for in the set</param>
        /// <param name="isNotIn">true for the not-in expression, false for the in-expression</param>
        /// <param name="parameters">is a set of constants to match against</param>
        public InExpression(Expression value, bool isNotIn, params object[] parameters)
        {
            IsNotIn = isNotIn;
            Children.Add(value);
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] is Expression)
                {
                    Children.Add((Expression)parameters[i]);
                }
                else
                {
                    Children.Add(new ConstantExpression(parameters[i]));
                }
            }
        }
    
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="value">expression to check</param>
        /// <param name="isNotIn">indicator whether not-in (true) or in (false)</param>
        /// <param name="parameters">expression list</param>
        public InExpression(Expression value, bool isNotIn, Expression[] parameters)
        {
            IsNotIn = isNotIn;
            Children.Add(value);
            foreach (Expression parameter in parameters) {
                Children.Add(parameter);
            }
        }

        /// <summary>
        /// Returns true for the not-in expression, or false for an in-expression.
        /// </summary>
        /// <value>true for not-in</value>
        public bool IsNotIn { get; set; }

        /// <summary>
        /// Add a constant to include in the computation.
        /// </summary>
        /// <param name="object">constant to add</param>
        /// <returns>expression</returns>
        public InExpression Add(object @object)
        {
            Children.Add(new ConstantExpression(@object));
            return this;
        }
    
        /// <summary>
        /// Add an expression to include in the computation.
        /// </summary>
        /// <param name="expression">to add</param>
        /// <returns>expression</returns>
        public InExpression Add(Expression expression)
        {
            Children.Add(expression);
            return this;
        }
    
        /// <summary>
        /// Add a property to include in the computation.
        /// </summary>
        /// <param name="propertyName">is the name of the property</param>
        /// <returns>expression</returns>
        public InExpression Add(string propertyName)
        {
            Children.Add(new PropertyValueExpression(propertyName));
            return this;
        }

        public override ExpressionPrecedenceEnum Precedence
        {
            get { return ExpressionPrecedenceEnum.RELATIONAL_BETWEEN_IN; }
        }

        public override void ToPrecedenceFreeEPL(TextWriter writer)
        {
            Children[0].ToEPL(writer, Precedence);
            if (IsNotIn)
            {
                writer.Write(" not in (");
            }
            else
            {
                writer.Write(" in (");
            }
    
            string delimiter = "";
            for (int i = 1; i < Children.Count; i++)
            {
                writer.Write(delimiter);
                Children[i].ToEPL(writer, ExpressionPrecedenceEnum.MINIMUM);
                delimiter = ",";
            }
            writer.Write(')');
        }
    }
}
