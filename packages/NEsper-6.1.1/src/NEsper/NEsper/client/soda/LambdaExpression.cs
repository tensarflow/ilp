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
    /// Lambda-expression is an expression of the form "parameter =&gt; body" where-in the "=&gt;" reads as goes-to.
    /// <para/>
    /// The form "x =&gt; x * x" reads as "x goes to x times x", for an example expression that yields x multiplied by x.
    /// <para/>
    /// Used with expression declaration and with enumeration methods, for example, to parameterize by an expression.
    /// </summary>
    [Serializable]
    public class LambdaExpression : ExpressionBase
    {
        /// <summary>Ctor. </summary>
        public LambdaExpression() {
        }
    
        /// <summary>Ctor. </summary>
        /// <param name="parameters">the lambda expression parameters</param>
        public LambdaExpression(IList<String> parameters)
        {
            Parameters = parameters;
        }

        /// <summary>Returns the lambda expression parameters. </summary>
        /// <value>lambda expression parameters</value>
        public IList<string> Parameters { get; set; }

        public override ExpressionPrecedenceEnum Precedence
        {
            get { return ExpressionPrecedenceEnum.MINIMUM; }
        }

        public override void ToPrecedenceFreeEPL(TextWriter writer) 
        {
            if (Parameters.Count > 1) {
                writer.Write("(");

                var delimiter = "";
                foreach (var parameter in Parameters) {
                    writer.Write(delimiter);
                    writer.Write(parameter);
                    delimiter = ",";
                }
                writer.Write(")");
            }
            else {
                writer.Write(Parameters[0]);
            }
            writer.Write(" => ");
            Children[0].ToEPL(writer, Precedence);
        }
    }
}
