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
using com.espertech.esper.compat.logging;
using com.espertech.esper.epl.agg.access;
using com.espertech.esper.epl.expression.core;

namespace com.espertech.esper.epl.approx
{
    public class CountMinSketchAggAccessorDefault : AggregationAccessor {
    
        public static readonly CountMinSketchAggAccessorDefault INSTANCE = new CountMinSketchAggAccessorDefault();
    
        private CountMinSketchAggAccessorDefault() {
        }
    
        public object GetValue(AggregationState state, EvaluateParams evalParams)
        {
            return null;
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
} // end of namespace
