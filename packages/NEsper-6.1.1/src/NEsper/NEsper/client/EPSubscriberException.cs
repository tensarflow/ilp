///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

namespace com.espertech.esper.client
{
    /// <summary>
    /// This exception is thrown to indicate that a subscriber registration failed
    /// such as when the subscribe does not expose an acceptable method to receive
    /// statement results.
    /// </summary>
    [Serializable]
    public class EPSubscriberException : EPException
    {
        /// <summary>Ctor. </summary>
        /// <param name="message">error message</param>
        public EPSubscriberException(String message)
            : base(message)
        {
        }
    }
}
