///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

using com.espertech.esper.compat.collections;
using com.espertech.esper.view.window;

namespace com.espertech.esper.view.ext
{
	/// <summary>
	/// Provides random access into a rank-window's data.
	/// </summary>
	public interface IStreamSortRankRandomAccess : RandomAccessByIndex
	{
	    void Refresh(OrderedDictionary<object, object> sortedEvents, int currentSize, int maxSize);
	}
} // end of namespace
