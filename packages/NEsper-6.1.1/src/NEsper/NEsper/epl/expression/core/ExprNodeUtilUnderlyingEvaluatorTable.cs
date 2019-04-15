///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

using com.espertech.esper.client;
using com.espertech.esper.compat;
using com.espertech.esper.compat.collections;
using com.espertech.esper.epl.table.mgmt;

namespace com.espertech.esper.epl.expression.core
{
    public class ExprNodeUtilUnderlyingEvaluatorTable : ExprEvaluator
    {
        private readonly int _streamNum;
        private readonly Type _resultType;
        private readonly TableMetadata _tableMetadata;
    
        public ExprNodeUtilUnderlyingEvaluatorTable(int streamNum, Type resultType, TableMetadata tableMetadata)
        {
            _streamNum = streamNum;
            _resultType = resultType;
            _tableMetadata = tableMetadata;
        }

        public object Evaluate(EvaluateParams evaluateParams)
        {
            if (evaluateParams.EventsPerStream == null)
            {
                return null;
            }
            var @event = evaluateParams.EventsPerStream[_streamNum];
            if (@event == null) {
                return null;
            }
            return _tableMetadata.EventToPublic.ConvertToUnd(@event, evaluateParams);
        }

        public Type ReturnType
        {
            get { return _resultType; }
        }
    }
}
