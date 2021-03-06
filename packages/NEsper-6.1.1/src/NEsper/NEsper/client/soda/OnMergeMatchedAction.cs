///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System.IO;

namespace com.espertech.esper.client.soda
{
    /// <summary>
    /// Marker interface for an on-merge clause action item.
    /// </summary>
    public interface OnMergeMatchedAction 
    {
        /// <summary>
        /// RenderAny to EPL.
        /// </summary>
        /// <param name="writer">to render to</param>
        void ToEPL(TextWriter writer);
    }
}
