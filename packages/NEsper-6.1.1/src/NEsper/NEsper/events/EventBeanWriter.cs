///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////


using System;

using com.espertech.esper.client;

namespace com.espertech.esper.events
{
    /// <summary>
    /// Interface for writing a set of event properties to an event.
    /// </summary>
    public interface EventBeanWriter
    {
        /// <summary>
        /// Write property values to the event.
        /// </summary>
        /// <param name="values">to write</param>
        /// <param name="theEvent">to write to</param>
        void Write(Object[] values, EventBean theEvent);
    }
}
