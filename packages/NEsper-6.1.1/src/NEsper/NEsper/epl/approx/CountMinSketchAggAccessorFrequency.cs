///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

using com.espertech.esper.client;
using com.espertech.esper.compat;
using com.espertech.esper.compat.collections;
using com.espertech.esper.epl.agg.access;
using com.espertech.esper.epl.expression.core;

namespace com.espertech.esper.epl.approx
{
    public class CountMinSketchAggAccessorFrequency : AggregationAccessor
    {
        private readonly ExprEvaluator _evaluator;
    
        public CountMinSketchAggAccessorFrequency(ExprEvaluator evaluator)
        {
            this._evaluator = evaluator;
        }

        public object GetValue(AggregationState aggregationState, EvaluateParams evalParams)
        {
            object value = _evaluator.Evaluate(new EvaluateParams(evalParams.EventsPerStream, true, evalParams.ExprEvaluatorContext));
            CountMinSketchAggState state = (CountMinSketchAggState) aggregationState;
            return state.Frequency(value);
        }
    
        public ICollection<EventBean> GetEnumerableEvents(AggregationState state, EvaluateParams evalParams) {
            return null;
        }
    
        public EventBean GetEnumerableEvent(AggregationState state, EvaluateParams evalParams) {
            return null;
        }
    
        public ICollection<object> GetEnumerableScalar(AggregationState state, EvaluateParams evalParams) {
            return null;
        }
    }
}
