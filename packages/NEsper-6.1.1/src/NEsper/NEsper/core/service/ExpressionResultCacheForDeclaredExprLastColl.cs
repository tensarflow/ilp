///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

using com.espertech.esper.client;

namespace com.espertech.esper.core.service
{
	/// <summary>
	/// On the level of expression declaration:
	/// a) for non-enum evaluation and for enum-evaluation a separate cache
	/// b) The cache is keyed by the prototype-node and verified by a events-per-stream (EventBean[]) that is maintained or rewritten.
	/// NOTE: ExpressionResultCacheEntry should not be held onto since the instance returned can be reused.
	/// </summary>
	public interface ExpressionResultCacheForDeclaredExprLastColl
    {
	    ExpressionResultCacheEntry<EventBean[], ICollection<EventBean>> GetDeclaredExpressionLastColl(object node, EventBean[] eventsPerStream);
	    void SaveDeclaredExpressionLastColl(object node, EventBean[] eventsPerStream, ICollection<EventBean> result);
	}
} // end of namespace
